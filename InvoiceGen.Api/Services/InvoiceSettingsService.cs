using Azure.Data.Tables;
using InvoiceGen.Api.Interfaces;
using InvoiceGen.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceGen.Api.Services
{
    public class InvoiceSettingsService : ISettingsService
    {
        private TableClient _invoiceSettingsContainer;
        private string _connectionString;

        public string StorageConnectionString
        {
            get
            {
                return _connectionString;
            }
        } // end prop


        public InvoiceSettingsService(TableServiceClient dbClient, string connectionString)
        {
            _connectionString = connectionString;
            this._invoiceSettingsContainer = dbClient.GetTableClient("InvoiceSettings");
        }
        public async Task<InvoiceSettings> GetSettingAsync()
        {
            var query = from c in _invoiceSettingsContainer.Query<InvoiceSettings>()
                        select c;
            try
            {
                var invoiceSettings = query.FirstOrDefault();
                return invoiceSettings;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<InvoiceSettings> AddSettingAsync(InvoiceSettings settings)
        {
            try
            {
                settings.RowKey = Guid.NewGuid().ToString();
                await this._invoiceSettingsContainer.AddEntityAsync<InvoiceSettings>(settings);
                return settings;
            }
            catch (Exception ex)
            {

            }
            return null;
        } // end f


        public async Task UpdateSettingAsync(InvoiceSettings settings)
        {
            await _invoiceSettingsContainer.UpsertEntityAsync<InvoiceSettings>(settings);
        }
    } // end c
} // end ns
