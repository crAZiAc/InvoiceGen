using Azure.Data.Tables;
using InvoiceGen.Api.Interfaces;
using InvoiceGen.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
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

        public async Task DeleteInvoiceAsync(string rowKey)
        {
            // Get invoice
            Invoice invoice = await _invoiceContainer.GetEntityAsync<Invoice>("Invoice",rowKey);
            if (invoice != null) 
            {
                // Delete underlying line items
                var query = from c in _orderItemContainer.Query<OrderItem>()
                            where c.RelatedInvoiceId == invoice.InvoiceId
                            where c.PartitionKey == "OrderItem"
                            select c;
                if (query.Any())
                {
                    foreach (OrderItem item in query)
                    {
                        await _orderItemContainer.DeleteEntityAsync("OrderItem", item.RowKey);
                    }
                }

                // Delete the invoice
                await _invoiceContainer.DeleteEntityAsync("Invoice", rowKey);
            }
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
                var invoices = query.ToList();
                foreach(Invoice invoice in invoices)
                {
                    invoice.Items = new List<OrderItem>();
                    var itemQuery = from c in _orderItemContainer.Query<OrderItem>()
                                    where c.RelatedInvoiceId == invoice.InvoiceId
                                    where c.PartitionKey == "OrderItem"
                                    select c;
                    if (itemQuery.Any())
                    {
                        invoice.Items = itemQuery.ToList();
                    }
                }
                return invoices;
            }
            catch (Exception ex)
            {

            }
            return null;
        } // end f

        public async Task UpdateInvoiceAsync(Invoice invoice)
        {
            await _invoiceContainer.UpsertEntityAsync<Invoice>(invoice);
            if (invoice.Items != null)
            {
                foreach (OrderItem item in invoice.Items)
                {
                    if (string.IsNullOrEmpty(item.RowKey))
                    {
                        item.RowKey = Guid.NewGuid().ToString();
                    }
                    if (string.IsNullOrEmpty(item.RelatedInvoiceId))
                    {
                        item.RelatedInvoiceId = invoice.InvoiceId;
                    }
                    await this._orderItemContainer.UpsertEntityAsync<OrderItem>(item);
                }
            }
        } // end f

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
