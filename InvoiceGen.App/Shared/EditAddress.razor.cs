using BlazorBootstrap;
using InvoiceGen.Api.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;

namespace InvoiceGen.App.Shared
{
    public partial class EditAddress
    {
        [Parameter] public Address address { get; set; }

        [Parameter] public EventCallback<Address> OnDeleteItemCallBack { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
      
    } // end c
} // end ns
