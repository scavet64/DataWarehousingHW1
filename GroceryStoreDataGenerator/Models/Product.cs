using System;

namespace GroceryStoreDataGenerator.Models
{
    public class Product
    {
        public string Manufacturer { get; set; }
        public string ProductName { get; set; }
        public string Size { get; set; }
        public string SKU { get; set; }
        public string ItemType { get; set; }
        public double BasePrice { get; set; }
        public int ItemsLeft { get; set; }
        public int TotalCasesOrdered { get; set; }
    }
}