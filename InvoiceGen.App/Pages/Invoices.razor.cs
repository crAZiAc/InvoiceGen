using BlazorBootstrap;
using InvoiceGen.Api;
using InvoiceGen.Api.Controllers;
using InvoiceGen.Api.Interfaces;
using InvoiceGen.Api.Models;
using InvoiceGen.App.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics;
using System.Text.Json;
using System.Xml.Linq;
using Microsoft.JSInterop;

namespace InvoiceGen.App.Pages
{
    public partial class Invoices
    {
        [Inject] protected IInvoiceService _invoiceService { get; set; }
        [Inject] protected IAddressService _addressService { get; set; }
        [Inject] protected ISettingsService _settingsService { get; set; }
        [Inject] protected SelectedSellerId _selectedSellerId { get; set; }
        [Inject] protected IJSRuntime js { get; set; }


        protected List<Invoice> _invoices;
        protected List<InvoiceGen.Api.Models.Address> _addresses;
        protected InvoiceController _invoiceController;
        protected AddressController _addressController;
        protected InvoiceSettingsController _settingsController;
        protected Invoice _currentInvoice;
        protected InvoiceSettings _settings;

        private Grid<Invoice> grid = default!;
        private Modal modal = default!;
        private ConfirmDialog dialog = default!;

        private bool showPdf = false;
        private string _pdfFileName = string.Empty;
        private string _pdfFileNameShort = string.Empty;
        private string _pdfBase64 = string.Empty;
        protected override async Task OnInitializedAsync()
        {
            _invoiceController = new InvoiceController(_invoiceService, _addressService);
            _addressController = new AddressController(_addressService);
            _invoices = await _invoiceController.GetInvoices();
            _addresses = await _addressController.GetAddresses();

            _settingsController = new InvoiceSettingsController(_settingsService);
            _settings = await _settingsController.GetInvoiceSetting();
            if (_settings == null) 
            {
                _settings = await _settingsController.AddInvoiceSetting(new InvoiceSettings());
            }
            _selectedSellerId.Id = _settings.SelectedSellerAddressId;
        }

        private async Task AddInvoice()
        {
            Invoice newInvoice = new Invoice
            {
                SellerAddressId = _selectedSellerId.Id,
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


        private async Task OnGeneratePdf()
        {
            _pdfFileNameShort = $"Factuur-{_currentInvoice.InvoiceId}-{_currentInvoice.IssueDate.Value.ToShortDateString()}.pdf";
            var dirPath = Path.Combine(Environment.CurrentDirectory, "unsafe_uploads");
            if (!System.IO.Directory.Exists(dirPath))
            {
                System.IO.Directory.CreateDirectory(dirPath);
            }
            _pdfFileName = Path.Combine(dirPath, _pdfFileNameShort);
            PdfGenerator generator = new PdfGenerator();
            generator.CreatePdf(_currentInvoice, _pdfFileName);
            
            // Read the PDF file into a byte array
            byte[] pdfBytes = File.ReadAllBytes(_pdfFileName);

            // Convert the byte array to a Base64 string
            _pdfBase64 = Convert.ToBase64String(pdfBytes);
            
            await modal.HideAsync();
            showPdf = true;

            await js.InvokeAsync<object>("psInterop.saveFile", _pdfFileNameShort, _pdfBase64);

        }

        private void OnDocumentLoaded(PdfViewerEventArgs args)
        {

        }

        private void OnPageChanged(PdfViewerEventArgs args)
        {

        }

        private async Task OnHidePdfViewerClick()
        {
            showPdf = false;
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
