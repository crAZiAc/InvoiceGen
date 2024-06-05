using InvoiceGen.Api.Controllers;
using InvoiceGen.Api.Interfaces;
using InvoiceGen.Api.Models;
using Microsoft.AspNetCore.Components;

namespace InvoiceGen.App.Pages
{
    public partial class Invoices
    {
        [Inject] protected IInvoiceService _invoiceService { get; set; }
        [Inject] protected IAddressService _addressService { get; set; }

        protected List<Invoice> _invoices;
        protected InvoiceController _invoiceController;

        protected override async Task OnInitializedAsync()
        { 
            _invoiceController = new InvoiceController(_invoiceService, _addressService);
            _invoices = await _invoiceController.GetInvoices();
        }
    } // end c
} // end ns
