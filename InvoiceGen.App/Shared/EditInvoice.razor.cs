using BlazorBootstrap;
using InvoiceGen.Api.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;

namespace InvoiceGen.App.Shared
{
    public partial class EditInvoice
    {
        [Parameter] public Invoice invoice { get; set; }
        [Parameter] public OrderItem currentItem { get; set; }

        [Parameter] public List<Address> addresses { get; set; }
        private List<OrderItem> orderItems;

        private Grid<OrderItem> orderItemsGrid;
        private Modal modal = default!;

        [Parameter] public EventCallback<OrderItem> OnDeleteItemCallBack { get; set; }

        protected override void OnInitialized()
        {
            orderItems = invoice.Items;
            base.OnInitialized();
        }

        private async Task AddItem()
        {
            OrderItem newItem = new OrderItem();
            currentItem = newItem;
            invoice.Items.Add(currentItem);
            await orderItemsGrid.RefreshDataAsync();
        }

        private async Task SelectItem(GridRowEventArgs<OrderItem> e)
        {
            currentItem = e.Item;
        }

        private async Task RemoveOrderItem(MouseEventArgs e, OrderItem item)
        {
            await OnDeleteItemCallBack.InvokeAsync(item);
            invoice.Items.Remove(item);
            currentItem = null;
            await orderItemsGrid.RefreshDataAsync();
        }
    } // end c
} // end ns
