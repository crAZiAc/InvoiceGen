using BlazorBootstrap;
using InvoiceGen.Api.Controllers;
using InvoiceGen.Api.Interfaces;
using InvoiceGen.Api.Models;
using InvoiceGen.App.Shared;
using Microsoft.AspNetCore.Components;

namespace InvoiceGen.App.Pages
{
    public partial class Invoices
    {
        [Inject] protected IInvoiceService _invoiceService { get; set; }
        [Inject] protected IAddressService _addressService { get; set; }

        protected List<Invoice> _invoices;
        protected InvoiceController _invoiceController;
        protected Invoice _currentInvoice;

        private Grid<Invoice> grid = default!;
        private Modal modal = default!;
        protected bool isEditing = false;

        protected override async Task OnInitializedAsync()
        { 
            _invoiceController = new InvoiceController(_invoiceService, _addressService);
            _invoices = await _invoiceController.GetInvoices();
        }

        private async Task AddInvoice()
        {

            //_invoices!.Add(CreateEmployee());
            //await grid.RefreshDataAsync();
        }

        private async Task EditInvoice(GridRowEventArgs<Invoice> e)
        {
            _currentInvoice = e.Item;
            var parameters = new Dictionary<string, object>
            {
                { "invoice", _currentInvoice }
            };
            await modal.ShowAsync<EditInvoice>(title: $"Factuur: {_currentInvoice.InvoiceId}", parameters: parameters);
            //_invoices!.Add(CreateEmployee());
            //await grid.RefreshDataAsync();
        }

        private async Task OnHideModalClick()
        {
            await modal.HideAsync();
        }

        private async Task OnSaveClick()
        {
            // Save invoice

            await modal.HideAsync();
        }

    } // end c
} // end ns
