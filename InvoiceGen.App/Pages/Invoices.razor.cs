using BlazorBootstrap;
using InvoiceGen.Api.Controllers;
using InvoiceGen.Api.Interfaces;
using InvoiceGen.Api.Models;
using InvoiceGen.App.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace InvoiceGen.App.Pages
{
    public partial class Invoices
    {
        [Inject] protected IInvoiceService _invoiceService { get; set; }
        [Inject] protected IAddressService _addressService { get; set; }

        protected List<Invoice> _invoices;
        protected List<InvoiceGen.Api.Models.Address> _addresses;
        protected InvoiceController _invoiceController;
        protected AddressController _addressController;
        protected Invoice _currentInvoice;

        private Grid<Invoice> grid = default!;
        private Modal modal = default!;
        private ConfirmDialog dialog = default!;

        protected override async Task OnInitializedAsync()
        {
            _invoiceController = new InvoiceController(_invoiceService, _addressService);
            _addressController = new AddressController(_addressService);
            _invoices = await _invoiceController.GetInvoices();
            _addresses = await _addressController.GetAddresses();
        }

        private async Task AddInvoice()
        {
            Invoice newInvoice = new Invoice
            {
                SellerAddressId = _addresses.Where(a => a.CompanyName == "Tom Visser").FirstOrDefault().RowKey,
                CustomerAddressId = _addresses.FirstOrDefault().RowKey,
                IssueDate = DateTime.Now.ToUniversalTime()
            };
            _currentInvoice = await _invoiceController.AddInvoice(newInvoice);
            _invoices.Add(_currentInvoice);
            await grid.RefreshDataAsync();
        }

        private async Task EditInvoice(GridRowEventArgs<Invoice> e)
        {
            _currentInvoice = e.Item;
            var parameters = new Dictionary<string, object>
            {
                { "invoice", _currentInvoice },
                { "addresses", _addresses },
                { "OnDeleteItemCallBack", EventCallback.Factory.Create<OrderItem>(this,RemoveOrderItem) }
            };
            await modal.ShowAsync<EditInvoice>(title: $"Factuur: {_currentInvoice.InvoiceId}", parameters: parameters);
        }

        private async Task RemoveOrderItem(OrderItem item)
        {
            if (string.IsNullOrEmpty(item.RowKey))
            {
                // Do nothing, item had not been saved before
            }
            else
            {
                await _invoiceController.DeleteOrderItem(item.RowKey);
            }
        }

        private async Task OnHideModalClick()
        {
            await modal.HideAsync();
        }

        private async Task OnSaveClick()
        {
            // Save invoice
            _currentInvoice = await _invoiceController.UpdateInvoice(_currentInvoice);
            await modal.HideAsync();
        }

        private async Task OnRemoveClick()
        {
            var options = new ConfirmDialogOptions { IsVerticallyCentered = true };
            var confirmation = await dialog.ShowAsync(
               title: $"{_currentInvoice.InvoiceId} -  Weet je zeker dat je deze factuur wilt verwijderen?",
               message1: "De factuur wordt verwijderd.",
               message2: "Wil je verdergaan met verwijderen?",
               confirmDialogOptions: options);

            if (confirmation)
            {
                // Delete invoice
                _invoices.Remove(_currentInvoice);
                await _invoiceController.DeleteInvoice(_currentInvoice.RowKey);
                await grid.RefreshDataAsync();
                await modal.HideAsync();
            }
            else
            {
                // do something
                await modal.HideAsync();
            }
        }

    } // end c
} // end ns
