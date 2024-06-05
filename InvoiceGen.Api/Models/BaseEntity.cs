using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InvoiceGen.Api.Models
{
    public abstract class BaseEntity : ITableEntity
    {
        [JsonPropertyName("partitionKey")]
        public string PartitionKey { get; set; }

        [JsonPropertyName("rowKey")]
        public string RowKey { get; set; }
        [JsonPropertyName("timeStamp")]

        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

    } // end c
} // end ns
