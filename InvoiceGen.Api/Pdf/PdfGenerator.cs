using InvoiceGen.Api.Models;
using InvoiceGen.Api.Pdf;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceGen.Api
{
    public class PdfGenerator
    {
        public PdfGenerator() 
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }
        public void CreatePdf(Invoice model, string outputFile)
        {
            var document = new InvoiceDocument(model);
            document.GeneratePdf(outputFile);
        }
    } // end c
} // end ns
