using BlazorBootstrap;
using InvoiceGen.Api.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace InvoiceGen.App.Shared
{
    public partial class EditInvoice
    {
        [Parameter] public Invoice invoice { get; set; }
        private EditContext? editContext;
        private List<OrderItem> orderItems;
        private OrderItem _currentItem;

        private Grid<OrderItem> orderItemsGrid;

        protected override void OnInitialized()
        {
            orderItems = invoice.Items;
            editContext = new EditContext(invoice);
            base.OnInitialized();
        }

        private async Task AddItem()
        {

            //_invoices!.Add(CreateEmployee());
            //await grid.RefreshDataAsync();
        }

        private async Task EditItem(GridRowEventArgs<OrderItem> e)
        {
            _currentItem = e.Item;
            var parameters = new Dictionary<string, object>
            {
                { "item", _currentItem }
            };
            //await modal.ShowAsync<EditInvoice>(title: $"Factuur: {_currentInvoice.InvoiceId}", parameters: parameters);
            //_invoices!.Add(CreateEmployee());
            //await grid.RefreshDataAsync();
        }
    } // end c
} // end ns
