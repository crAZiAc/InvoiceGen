using InvoiceGen.Api.Interfaces;
using InvoiceGen.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace InvoiceGen.Api.Controllers
{
    public class InvoiceController
    {
        protected IInvoiceService _invoiceService;
        public InvoiceController(IInvoiceService service) 
        {
            _invoiceService = service;
        } // ct

        public async Task<List<Invoice>> GetInvoices()
        {
            return await _invoiceService.GetInvoicesAsync();
        } // end f

        public async Task<Invoice> GetInvoice(int invoiceNumber)
        {
            return await _invoiceService.GetInvoiceAsync(invoiceNumber.ToString());
        } // end f

        public async Task<Invoice> AddInvoice(Invoice invoice)
        {
            // Check if seller address is there

            return await _invoiceService.GetInvoiceAsync(invoice.InvoiceNumber.ToString());
        } // end f


    } // end c
} // end ns
