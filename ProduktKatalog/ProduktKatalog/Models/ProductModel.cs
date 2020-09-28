using Microsoft.Azure.Cosmos.Table;
using System;

namespace ProductCatalog.Models
{
    class ProductModel : TableEntity
    {
        public ProductModel()
        {
        }

        public ProductModel(int ProductId, DateTime currentDateTime)
        {
            PartitionKey = ProductId.ToString();
            RowKey = currentDateTime.ToString();
        }

        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        
        public class ProductModelExtended : ProductModel
        {
            public DateTime RowKeyDateTime => DateTime.Parse(RowKey);
        }
    }
}
