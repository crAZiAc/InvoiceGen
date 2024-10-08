﻿using InvoiceGen.Api.Interfaces;
using InvoiceGen.Api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace InvoiceGen.Api.Controllers
{
    public class InvoiceController
    {
        protected IInvoiceService _invoiceService;
        protected IAddressService _addressService;
        public InvoiceController(IInvoiceService invoiceService, IAddressService addressService)
        {
            _invoiceService = invoiceService;
            _addressService = addressService;
        } // ct

        public async Task<List<Invoice>> GetInvoices()
        {
            var invoices = await _invoiceService.GetInvoicesAsync();
            foreach (var invoice in invoices)
            {
                // Fill Seller and Customer Addresses
                var seller = await _addressService.GetAddressAsync(invoice.SellerAddressId);
                var customer = await _addressService.GetAddressAsync(invoice.CustomerAddressId);

                if (seller != null & customer != null)
                {
                    invoice.SellerAddress = seller;
                    invoice.CustomerAddress = customer;
                }
            }
            return invoices;
        } // end f

        /// <summary>
        /// Gets Invoice, including addresses and OrderItems
        /// </summary>
        /// <param name="ID">This is the Invoice Identifier, technically a GUID string: the rowKey from the Azure Table</param>
        /// <returns>The requested invoice</returns>
        public async Task<Invoice> GetInvoice(string ID)
        {
            Invoice invoice = await _invoiceService.GetInvoiceAsync(ID);

            // Fill Seller and Customer Addresses
            var seller = await _addressService.GetAddressAsync(invoice.SellerAddressId);
            var customer = await _addressService.GetAddressAsync(invoice.CustomerAddressId);

            if (seller != null & customer != null)
            {
                invoice.SellerAddress = seller;
                invoice.CustomerAddress = customer;
            }
            return invoice;
        } // end f

        public async Task<Invoice> AddInvoice(Invoice invoice)
        {
            // Check if seller address is there
            var checkSeller = await _addressService.GetAddressAsync(invoice.SellerAddressId);
            if (checkSeller != null)
            {
                // Check customer
                var checkCustomer = await _addressService.GetAddressAsync(invoice.CustomerAddressId);
                if (checkCustomer != null)
                {
                    invoice.SellerAddress = checkSeller;
                    invoice.CustomerAddress = checkCustomer;
                    invoice.IssueDate = invoice.IssueDate.Value.ToUniversalTime();
                    await _invoiceService.AddInvoiceAsync(invoice);
                    return invoice;
                }
            }
            return null;
        } // end f

        public async Task<Invoice> UpdateInvoice(Invoice invoice)
        {
            // Check if seller address is there
            var checkSeller = await _addressService.GetAddressAsync(invoice.SellerAddressId);
            if (checkSeller != null)
            {
                // Check customer
                var checkCustomer = await _addressService.GetAddressAsync(invoice.CustomerAddressId);
                if (checkCustomer != null)
                {
                    invoice.SellerAddress = checkSeller;
                    invoice.CustomerAddress = checkCustomer;
                    invoice.IssueDate = invoice.IssueDate.Value.ToUniversalTime();
                    await _invoiceService.UpdateInvoiceAsync(invoice);
                    return invoice;
                }
            }
            return null;
        } // end f

        /// <summary>
        /// Deletes the invoice and associated line items
        /// </summary>
        /// <param name="ID">This is the Invoice Identifier, technically a GUID string: the rowKey from the Azure Table</param>
        /// <returns>Result for the operation</returns>
        public async Task DeleteInvoice(string ID)
        {
            await _invoiceService.DeleteInvoiceAsync(ID);
        } // end f

        /// <summary>
        /// Deletes the line item
        /// </summary>
        /// <param name="ID">This is the line item Identifier, technically a GUID string: the rowKey from the Azure Table</param>
        /// <returns>Result for the operation</returns>
        public async Task DeleteOrderItem(string ID)
        {
            await _invoiceService.DeleteOrderItemAsync(ID);
        } // end f

        public async Task<List<QuarterReport>> GetQuarterReports()
        {
            Dictionary<string, QuarterReport> reports = new Dictionary<string, QuarterReport>();

            var invoices = await _invoiceService.GetInvoicesAsync();
            foreach (var invoice in invoices.OrderBy(i => i.IssueDate))
            {
                string quarterName = $"{invoice.IssueDate.Value.Year} - kwartaal {invoice.IssueDate.Value.GetQuarter()}";
                if (reports.ContainsKey(quarterName))
                {
                    reports[quarterName].TotalAmountWithVat += invoice.TotalAmountWithVat;
                    reports[quarterName].TotalAmount += invoice.TotalAmount;
                    reports[quarterName].TotalVatAmount += invoice.TotalVat;
                    reports[quarterName].NumberOfInvoices += 1;
                }
                else
                {
                    reports.Add(quarterName, new QuarterReport
                    {
                        Quarter = quarterName,
                        TotalAmount = invoice.TotalAmount,
                        TotalAmountWithVat = invoice.TotalAmountWithVat,
                        TotalVatAmount = invoice.TotalVat,
                        NumberOfInvoices = 1
                    }
                    ); 
                }
            }
            return reports.Values.ToList();
        } // end f


    } // end c
} // end ns
