using PDFGeneratorSample.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PDFGeneratorSample.Samples;

public class SampleDocument
{
    public class MyDocument(List<TableData> firstTableData, List<TableData> secondTableData)
        : IDocument
    {
        private const string CompanyName = "Your Company Name";

        public DocumentMetadata GetMetadata()
        {
            return DocumentMetadata.Default;
        }

        public void Compose(IDocumentContainer container)
        {
            container
                .Page(page =>
                {
                    page.Margin(50);

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(ComposeContent);
                    page.Footer().Element(ComposeFooter);
                });
        }

        // Compose the header with company logo
        private void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                // Left side: Company logo placeholder
                row.RelativeItem().Column(column =>
                {
                    // In a real application, replace this with actual logo loading
                    column.Item().Height(50).Background(Colors.Grey.Lighten2).Padding(10)
                        .Text("COMPANY LOGO")
                        .FontSize(20)
                        .FontColor(Colors.Blue.Medium);
                });

                // Right side: Company name and contact info
                row.RelativeItem().Column(column =>
                {
                    column.Item().AlignRight().Text(text =>
                    {
                        text.Span(CompanyName).FontSize(18).Bold();
                        text.Line("");
                        text.Span("123 Business Street").FontSize(10);
                        text.Line("");
                        text.Span("City, Country, 12345").FontSize(10);
                        text.Line("");
                        text.Span("Phone: (123) 456-7890").FontSize(10);
                    });
                });
            });
        }

        // Compose the main content with title page and tables page
        private void ComposeContent(IContainer container)
        {
            container.PaddingVertical(40).Column(column =>
            {
                // First page: Title page
                column.Item().Component(new TitlePage());

                // Second page: Tables page
                column.Item().PageBreak();
                column.Item().Component(new TablesPage(firstTableData, secondTableData));
            });
        }

        // Compose the footer with pagination
        private void ComposeFooter(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().AlignCenter().Text(text =>
                {
                    text.DefaultTextStyle(style => style.FontSize(10));
                    text.CurrentPageNumber();
                    text.Span(" / ");
                    text.TotalPages();
                });
                // Footer text
                column.Item().Height(10);
                column.Item().AlignCenter().Text("This is a test footer")
                    .FontSize(10)
                    .FontColor(Colors.Grey.Medium);
            });
        }
    }

    // Title page component
    public class TitlePage : IComponent
    {
        public void Compose(IContainer container)
        {
            container.Column(column =>
            {
                // Add some spacing at the top
                column.Item().Height(100);

                // Title
                column.Item().AlignCenter().Text("REPORT TITLE")
                    .FontSize(28)
                    .Bold();

                column.Item().Height(50);

                // Subtitle
                column.Item().AlignCenter().Text("Detailed Report")
                    .FontSize(18);

                column.Item().Height(100);

                // Date
                column.Item().AlignCenter().Text($"Generated on: {DateTime.Now:MMMM d, yyyy}")
                    .FontSize(12);

                // Add content at the bottom of the page
                column.Item().AlignBottom().AlignCenter().Text("CONFIDENTIAL")
                    .FontSize(14)
                    .FontColor(Colors.Red.Medium);
            });
        }
    }

    // Tables page component
    public class TablesPage(List<TableData> firstTableData, List<TableData> secondTableData)
        : IComponent
    {
        public void Compose(IContainer container)
        {
            container.Column(column =>
            {
                // Section title
                column.Item().Text("Report Tables")
                    .FontSize(16)
                    .Bold();

                column.Item().Height(20);

                // First table (with borders)
                column.Item().Text("Table 1: Products (with borders)")
                    .FontSize(14)
                    .Bold();

                column.Item().Height(10);

                column.Item().Table(table =>
                {
                    // Define columns
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.RelativeColumn(3);
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    // Table header
                    table.Header(header =>
                    {
                        header.Cell().Border(1).Background(Colors.Grey.Lighten3).Text("ID").Bold();
                        header.Cell().Border(1).Background(Colors.Grey.Lighten3).Text("Name").Bold();
                        header.Cell().Border(1).Background(Colors.Grey.Lighten3).Text("Price").Bold();
                        header.Cell().Border(1).Background(Colors.Grey.Lighten3).Text("Quantity").Bold();
                        header.Cell().Border(1).Background(Colors.Grey.Lighten3).Text("Total").Bold();
                    });

                    // Table content
                    foreach (var item in firstTableData)
                    {
                        table.Cell().Border(1).Text(item.Id.ToString());
                        table.Cell().Border(1).Text(item.Name);
                        table.Cell().Border(1).Text($"${item.Price}").AlignRight();
                        table.Cell().Border(1).Text(item.Quantity.ToString()).AlignRight();
                        table.Cell().Border(1).Text($"${item.Total}").AlignRight();
                    }
                });

                column.Item().Height(40);

                // Second table (without borders)
                column.Item().Text("Table 2: Services (without borders)")
                    .FontSize(14)
                    .Bold();

                column.Item().Height(10);

                column.Item().Table(table =>
                {
                    // Define columns
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.RelativeColumn(3);
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    // Table header
                    table.Header(header =>
                    {
                        header.Cell().Background(Colors.Grey.Lighten3).Text("ID").Bold();
                        header.Cell().Background(Colors.Grey.Lighten3).Text("Name").Bold();
                        header.Cell().Background(Colors.Grey.Lighten3).Text("Price").Bold();
                        header.Cell().Background(Colors.Grey.Lighten3).Text("Quantity").Bold();
                        header.Cell().Background(Colors.Grey.Lighten3).Text("Total").Bold();
                    });

                    // Table content
                    foreach (var item in secondTableData)
                    {
                        table.Cell().Text(item.Id.ToString());
                        table.Cell().Text(item.Name);
                        table.Cell().Text($"${item.Price}").AlignRight();
                        table.Cell().Text(item.Quantity.ToString()).AlignRight();
                        table.Cell().Text($"${item.Total}").AlignRight();
                    }
                });
            });
        }
    }
}