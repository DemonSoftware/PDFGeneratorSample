using PDFGeneratorSample.Models;

namespace PDFGeneratorSample.Samples
{
    public class SampleData
    {
        public static List<TableData> GetFirstTableData()
        {
            return new List<TableData>
            {
                new TableData { Id = 1, Name = "Product A", Price = 29.99m, Quantity = 10 },
                new TableData { Id = 2, Name = "Product B", Price = 59.99m, Quantity = 5 },
                new TableData { Id = 3, Name = "Product C", Price = 14.50m, Quantity = 20 },
                new TableData { Id = 4, Name = "Product D", Price = 99.99m, Quantity = 3 },
                new TableData { Id = 5, Name = "Product E", Price = 45.75m, Quantity = 8 }
            };
        }

        public static List<TableData> GetSecondTableData()
        {
            return new List<TableData>
            {
                new TableData { Id = 6, Name = "Service X", Price = 199.99m, Quantity = 1 },
                new TableData { Id = 7, Name = "Service Y", Price = 149.50m, Quantity = 2 },
                new TableData { Id = 8, Name = "Service Z", Price = 299.99m, Quantity = 1 }
            };
        }
    }
}

