using BlazorBootstrap;
using InvoiceGen.Api.Controllers;
using InvoiceGen.Api.Interfaces;
using InvoiceGen.Api.Models;
using InvoiceGen.Api.Services;
using InvoiceGen.App.Shared;
using Microsoft.AspNetCore.Components;

namespace InvoiceGen.App.Pages
{
    public partial class Settings
    {
        [Inject] protected IAddressService _addressService { get; set; }
        [Inject] protected ISettingsService _settingsService { get; set; }
        [Inject] protected SelectedSellerId _selectedSellerId { get; set; }
        protected AddressController _addressController;
        protected InvoiceSettingsController _settingsController;
        protected List<Address> _addresses;

        private Grid<Address> grid = default!;
        private Modal modal = default!;
        private ConfirmDialog dialog = default!;
        private InvoiceSettings _settings;

        protected override async Task OnInitializedAsync()
        {
            _addressController = new AddressController(_addressService);
            _addresses = await _addressController.GetAddresses();
            _settingsController = new InvoiceSettingsController(_settingsService);
            _settings = await _settingsController.GetInvoiceSetting();
            if (_settings == null)
            {
                _settings = await _settingsController.AddInvoiceSetting(new InvoiceSettings());
            }
        }

        private async Task SelectionChanged(string selectedAddressId)
        {
            _selectedSellerId.Id = selectedAddressId;
            _settings.SelectedSellerAddressId = _selectedSellerId.Id;
            // Save to settings
            await _settingsController.UpdateInvoiceSetting(_settings);
        }

    } // end c
} // end ns
