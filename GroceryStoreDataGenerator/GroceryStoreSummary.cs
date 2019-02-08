using System.Collections.Generic;
using System.Linq;
using GroceryStoreDataGenerator.Models;

namespace GroceryStoreDataGenerator
{
    public class GroceryStoreSummary
    {
        public GroceryStoreSummary(List<ScannerData> data)
        {
            var distinctCustomers = data.Select(d => (d.CustomerNumber, d.DatePurchased)).Distinct();
            NumberOfCustomers = distinctCustomers.Count();

            TotalSales = data.Select(d => d.SalePrice).Sum();

            TotalItemsBought = data.Count;

            TopTenSellingItemsWithCounts = (
                from d in data
                group d.ProductPurchased by d.ProductPurchased
                into gd
                orderby gd.Count() descending
                select (gd.Key, gd.Count())
            ).Take(10).ToList();
        }

        public int NumberOfCustomers { get; }

        public double TotalSales { get; }

        public int TotalItemsBought { get; }

        public List<(string Item, int Count)> TopTenSellingItemsWithCounts { get; }
    }
}