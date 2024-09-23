using BlazorBootstrap;
using InvoiceGen.Api.Controllers;
using InvoiceGen.Api.Interfaces;
using InvoiceGen.Api.Models;
using InvoiceGen.Api.Services;
using InvoiceGen.App.Shared;
using Microsoft.AspNetCore.Components;

namespace InvoiceGen.App.Pages
{
    public partial class Debtor
    {
        [Inject] protected IAddressService _addressService { get; set; }
        protected List<Address> _addresses;
        protected AddressController _addressController;
        protected Address _currentAddress;

        private Grid<Address> grid = default!;
        private Modal modal = default!;
        private ConfirmDialog dialog = default!;

        protected override async Task OnInitializedAsync()
        {
            _addressController = new AddressController(_addressService);
            _addresses = await _addressController.GetAddresses();
        }

        private async Task AddAddress()
        {
            Address newAddress = new Address
            {
            };
            _currentAddress = await _addressController.AddAddress(newAddress);
            _addresses.Add(_currentAddress);
            await grid.RefreshDataAsync();
        }

        private async Task EditAddress(GridRowEventArgs<Address> e)
        {
            _currentAddress = e.Item;
            var parameters = new Dictionary<string, object>
            {
                { "address", _currentAddress },
                { "OnDeleteItemCallBack", EventCallback.Factory.Create<Address>(this,RemoveOrderItem) }
            };
            await modal.ShowAsync<EditAddress>(title: $"Klant: {_currentAddress.CompanyName}", parameters: parameters);
        }

        private async Task OnHideModalClick()
        {
            await modal.HideAsync();
        }

        private async Task OnSaveClick()
        {
            // Save address
            await _addressController.UpdateAddress(_currentAddress);
            await grid.RefreshDataAsync();
            await modal.HideAsync();
        }

        private async Task OnRemoveClick()
        {
            var options = new ConfirmDialogOptions { IsVerticallyCentered = true };
            var confirmation = await dialog.ShowAsync(
               title: $"{_currentAddress.CompanyName} -  Weet je zeker dat je deze klant wilt verwijderen?",
               message1: "De klant wordt verwijderd.",
               message2: "Wil je verdergaan met verwijderen?",
               confirmDialogOptions: options);

            if (confirmation)
            {
                // Delete adress
                _addresses.Remove(_currentAddress);
                await _addressController.DeleteAddress(_currentAddress.RowKey);
                await grid.RefreshDataAsync();
                await modal.HideAsync();
            }
            else
            {
                // do something
                await modal.HideAsync();
            }
        }

        private async Task RemoveOrderItem(Address item)
        {
            if (string.IsNullOrEmpty(item.RowKey))
            {
                // Do nothing, item had not been saved before
            }
            else
            {
                await _addressController.DeleteAddress(item.RowKey);
            }
        }

    } // end c
} // end ns
