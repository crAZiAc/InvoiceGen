using Azure.Data.Tables;
using InvoiceGen.Api.Interfaces;
using InvoiceGen.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceGen.Api.Services
{
    public class InvoiceService : IInvoiceService
    {
        private TableClient _invoiceContainer;
        private TableClient _orderItemContainer;
        private string _connectionString;

        public InvoiceService(TableServiceClient dbClient, string connectionString)
        {
            _connectionString = connectionString;
            this._invoiceContainer = dbClient.GetTableClient("Invoice");
            this._orderItemContainer = dbClient.GetTableClient("OrderItem");
        }
        public string StorageConnectionString
        {
            get
            {
                return _connectionString;
            }
        } // end prop

        public async Task AddInvoiceAsync(Invoice invoice)
        {
            try
            {
                invoice.RowKey = Guid.NewGuid().ToString();
                invoice.InvoiceNumber = this.GetNewInvoiceId();
                await this._invoiceContainer.AddEntityAsync<Invoice>(invoice);
                if (invoice.Items != null)
                {
                    foreach (OrderItem item in invoice.Items)
                    {
                        item.RowKey = Guid.NewGuid().ToString();
                        item.RelatedInvoiceId = invoice.InvoiceId;
                        await this._orderItemContainer.AddEntityAsync<OrderItem>(item);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        } // end f

        public Task DeleteInvoiceAsync(string rowKey)
        {
            throw new NotImplementedException();
        }

        public async Task<Invoice> GetInvoiceAsync(string rowKey)
        {
            Invoice invoice = null;
            var query = from c in _invoiceContainer.Query<Invoice>()
                        where c.RowKey == rowKey
                        where c.PartitionKey == "Invoice"
                        select c;
            try
            {
                if (query.Any())
                {
                    invoice = query.FirstOrDefault();
                    var itemQuery = from c in _orderItemContainer.Query<OrderItem>()
                                where c.RelatedInvoiceId == invoice.InvoiceId
                                where c.PartitionKey == "OrderItem"
                                select c;

                    invoice.Items = new List<OrderItem>();
                    if (itemQuery.Any())
                    {
                        invoice.Items = itemQuery.ToList();
                    }
                }
                return invoice;
            }
            catch (Exception ex)
            {
                return null;
            }
        } // end f

        public async Task<List<Invoice>> GetInvoicesAsync()
        {
            var query = from c in _invoiceContainer.Query<Invoice>()
                        orderby c.InvoiceNumber
                        select c;
            try
            {
                return query.ToList();
            }
            catch (Exception ex)
            {

            }
            return null;
        } // end f

        public Task UpdateInvoiceAsync(Invoice invoice)
        {
            throw new NotImplementedException();
        }

        protected int GetNewInvoiceId()
        {
            var query = from c in _invoiceContainer.Query<Invoice>()
                        where c.PartitionKey == "Invoice"
                        orderby c.InvoiceNumber descending
                        select c;
            try
            {
                if (query.Any())
                {
                    var lastOne = query.FirstOrDefault();
                    return lastOne.InvoiceNumber + 1;
                }
                else
                    return 1;
            }
            catch (Exception ex)
            {
                return 1;
            }
        }
    } // end c
} // end ns
