using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace InvoiceGen.Api.Pdf
{
    using InvoiceGen.Api.Models;
    using QuestPDF.Drawing;
    using QuestPDF.Fluent;
    using QuestPDF.Helpers;
    using QuestPDF.Infrastructure;
    using System.Data.Common;
    using static System.Net.Mime.MediaTypeNames;

    public class InvoiceDocument : IDocument
    {
        public Invoice Model { get; }

        public InvoiceDocument(Invoice model)
        {
            Model = model;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
        public DocumentSettings GetSettings() => DocumentSettings.Default;

        public void Compose(IDocumentContainer container)
        {
            container
                .Page(page =>
                {
                    page.Margin(50);

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(ComposeContent);


                    //page.Footer().AlignCenter().Text(x =>
                    //{
                    //    x.CurrentPageNumber();
                    //    x.Span(" / ");
                    //    x.TotalPages();
                    //});
                });
        }

        void ComposeHeader(IContainer container)
        {
            var titleStyle = TextStyle.Default.FontSize(20).SemiBold().FontColor(Colors.Grey.Medium);
            var invoiceStyle = TextStyle.Default.FontSize(14).SemiBold().FontColor(Colors.Blue.Medium);

            container.Column(column =>
            {
                column.Spacing(15);
                column.Item().BorderBottom(1).PaddingBottom(5).Text(Model.SellerAddress.CompanyName).Style(titleStyle);

                column.Item().Row(row =>
                {
                    row.RelativeItem().Column(column =>
                    {

                        column.Item().Text($"Factuur: {Model.InvoiceId}").Style(invoiceStyle);


                        column.Item().Text(text =>
                        {
                            text.Span("Factuurdatum: ").SemiBold();
                            text.Span($"{Model.IssueDate:d}");
                        });

                        //column.Item().Text(text =>
                        //{
                        //    text.Span("BTW Nummer: ").SemiBold();
                        //    text.Span($"NL004478108B19");
                        //});


                    });

                    row.RelativeItem().Column(column =>
                    {
                        column.Item().Text($"{Model.SellerAddress.CompanyName}");
                        column.Item().Text($"{Model.SellerAddress.Street}");
                        column.Item().Text($"{Model.SellerAddress.Zip}  {Model.SellerAddress.City}");
                        column.Item().Text($"{Model.SellerAddress.Phone}");
                        column.Item().Text($"{Model.SellerAddress.Email}");

                    });

                    //row.ConstantItem(100).Height(50).Placeholder($"{Model.InvoiceId}");
                });
            });
        }

        void ComposeContent(IContainer container)
        {
            container.PaddingVertical(40).Column(column =>
            {
                column.Spacing(5);

                column.Item().Row(row =>
                {
                    row.RelativeItem().Component(new AddressComponent("Aan", Model.CustomerAddress));
                });

                column.Item().Row(row =>
                {
                    row.RelativeItem().Height(32, Unit.Point);
                });

                column.Item().Element(ComposeTable);

                var totalAmount = Model.TotalAmount;
                var totalAmountWithVat = Model.TotalAmountWithVat;
                var VatAmount = Model.TotalVat;
                //column.Item().Row(row =>
                //{
                //    row.RelativeItem().Column(column =>
                //    {
                //        column.Item().AlignRight().Text(text =>
                //        {
                //            text.Span($"Totaal exclusief BTW: ").FontSize(12);
                //            text.Span($"{totalAmount.ToString("C2")}").FontSize(14).SemiBold();
                //        });
                //    });
                //});

                //column.Item().Row(row =>
                //{
                //    row.RelativeItem().Column(column =>
                //    {
                //        column.Item().AlignRight().Text(text =>
                //        {
                //            text.Span($"BTW: ").FontSize(12);
                //            text.Span($"{VatAmount.ToString("C2")}").FontSize(14).SemiBold();
                //        });
                //    });
                //});

                column.Item().Row(row =>
                {
                    row.RelativeItem().Column(column =>
                    {
                        column.Item().AlignRight().Text(text =>
                        {
                            text.Span($"Totaal: ").FontSize(12);
                            text.Span($"{totalAmountWithVat.ToString("C2")}").FontSize(14).Bold();
                        });
                    });
                });

                column.Item().PaddingTop(25).Element(ComposePaymentConditions);

                if (!string.IsNullOrWhiteSpace(Model.Comments))
                    column.Item().PaddingTop(25).Element(ComposeComments);
            });
        }

        void ComposeTable(IContainer container)
        {
            container.Table(table =>
            {
                // step 1
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(25);
                    columns.RelativeColumn(3);
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                // step 2
                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).Text("#");
                    header.Cell().Element(CellStyle).Text("Omschrijving");
                    header.Cell().Element(CellStyle).AlignRight().Text("Prijs");
                    header.Cell().Element(CellStyle).AlignRight().Text("Aantal");
                    header.Cell().Element(CellStyle).AlignRight().Text("Totaal");

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                    }
                });

                // step 3
                foreach (var item in Model.Items)
                {
                    table.Cell().Element(CellStyle).Text(Model.Items.IndexOf(item) + 1);
                    table.Cell().Element(CellStyle).Text(item.Name);
                    table.Cell().Element(CellStyle).AlignRight().Text($"{item.Price.ToString("C2")}");
                    table.Cell().Element(CellStyle).AlignRight().Text(item.Quantity);
                    table.Cell().Element(CellStyle).AlignRight().Text($"{item.Amount.ToString("C2")}");

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                    }
                }
            });
        }

        void ComposeComments(IContainer container)
        {
            container.Background(Colors.Grey.Lighten3).Padding(10).Column(column =>
            {
                column.Spacing(5);
                column.Item().Text("Opmerkingen").FontSize(14);
                column.Item().Text(Model.Comments);
            });
        }

        void ComposePaymentConditions(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().AlignLeft().Text(text =>
                {
                    text.Line($"Graag het verschuldigde bedrag binnen 14 dagen overmaken naar rekeningnummer").FontSize(12);
                    text.EmptyLine();
                    text.Span($"NL98 INGB 0670 5939 82").FontSize(14).Bold();
                    text.Span($", ten name van Robyn de Ridder, of betaal bijgestuurde Tikkie").FontSize(12);
                });
            });
        }

    } // end c
} // end ns
