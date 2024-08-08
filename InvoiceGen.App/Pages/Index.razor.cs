using BlazorBootstrap;
using InvoiceGen.Api;
using InvoiceGen.Api.Controllers;
using InvoiceGen.Api.Interfaces;
using InvoiceGen.Api.Models;
using InvoiceGen.Api.Services;
using InvoiceGen.App.Shared;
using Microsoft.AspNetCore.Components;
namespace InvoiceGen.App.Pages
{
    public partial class Index
    {
        [Inject] protected IInvoiceService _invoiceService { get; set; }
        [Inject] protected IAddressService _addressService { get; set; }


        protected InvoiceController _invoiceController;
        protected AddressController _addressController;
        protected List<QuarterReport> _reports;

        private Grid<QuarterReport> grid = default!;

        protected override async Task OnInitializedAsync()
        {
            _invoiceController = new InvoiceController(_invoiceService, _addressService);
            _addressController = new AddressController(_addressService);
            _reports = await _invoiceController.GetQuarterReports();

        }
    } // end c
} // end ns
