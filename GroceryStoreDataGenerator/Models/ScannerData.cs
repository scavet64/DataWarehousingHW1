using System;

namespace GroceryStoreDataGenerator.Models
{
    public class ScannerData
    {
        public ScannerData(string productPurchased, string datePurchased, double salePrice, int customerNumber)
        {
            ProductPurchased = productPurchased;
            DatePurchased = datePurchased;
            SalePrice = Math.Round(salePrice, 2);
            CustomerNumber = customerNumber;
        }

        public string ProductPurchased { get; set; }
        public string DatePurchased { get; set; }
        public double SalePrice { get; set; }
        public int CustomerNumber { get; set; }
    }
}