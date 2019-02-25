using System.Collections.Generic;
using System.Data;
using GroceryStoreDataGenerator.Database;

namespace GroceryStoreDataGenerator
{
    public class GroceryStoreSummary
    {
        public GroceryStoreSummary(SQLiteHandler dbHandler)
        {
            var cmd = dbHandler.SQLiteConnection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT COUNT(*) FROM (SELECT DISTINCT customerNumber, datePurchased FROM scannerData)";
            NumberOfCustomers = (long)cmd.ExecuteScalar();

            cmd.CommandText = "SELECT SUM(salePrice) FROM scannerData";
            TotalSales = (double)cmd.ExecuteScalar();

            cmd.CommandText = "SELECT COUNT(*) FROM scannerData";
            TotalItemsBought = (long)cmd.ExecuteScalar();

            cmd.CommandText = "SELECT productName, COUNT(productPurchased) FROM scannerData " +
                              "JOIN products ON products.sku = scannerData.productPurchased " +
                              "GROUP BY productPurchased " +
                              "ORDER BY 2 DESC LIMIT 10";

            TopTenSellingItemsWithCounts = new List<(string Item, long Count)>(10);
            using (var reader = cmd.ExecuteReader()) while (reader.Read()) TopTenSellingItemsWithCounts.Add((reader.GetString(0), reader.GetInt64(1)));
        }

        public long NumberOfCustomers { get; }

        public double TotalSales { get; }

        public long TotalItemsBought { get; }

        public List<(string Item, long Count)> TopTenSellingItemsWithCounts { get; }
    }
}