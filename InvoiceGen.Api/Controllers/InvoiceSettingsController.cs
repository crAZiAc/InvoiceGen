using InvoiceGen.Api.Interfaces;
using InvoiceGen.Api.Models;
using InvoiceGen.Api.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace InvoiceGen.Api.Controllers
{
    public class InvoiceSettingsController
    {
        protected ISettingsService _settingsService;
        public InvoiceSettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        } // ct

        public async Task<InvoiceSettings> GetInvoiceSetting()
        {
            return await _settingsService.GetSettingAsync();
        } // end f

     
        public async Task<InvoiceSettings> AddInvoiceSetting(InvoiceSettings settings)
        {
            return await _settingsService.AddSettingAsync(settings);
        } // end f

        public async Task UpdateInvoiceSetting(InvoiceSettings settings)
        {
            await _settingsService.UpdateSettingAsync(settings);
        } // end f
    } // end c
} // end ns
