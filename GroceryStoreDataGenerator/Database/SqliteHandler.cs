using System;
using System.Data;
using System.Data.SQLite;
using GroceryStoreDataGenerator.Models;

namespace GroceryStoreDataGenerator.Database
{
    public class SQLiteHandler : IDisposable
    {
        private readonly SQLiteCommand command;

        public SQLiteHandler(string dbName)
        {
            SQLiteConnection.CreateFile(dbName);
            SQLiteConnection = new SQLiteConnection($"Data Source={dbName}");
            SQLiteConnection.Open();
            command = SQLiteConnection.CreateCommand();
            command.CommandType = CommandType.Text;
            CreateTables();
        }

        public SQLiteConnection SQLiteConnection { get; }

        public void Dispose()
        {
            SQLiteConnection.Close();
        }

        public void Insert(Product product)
        {
            command.CommandText = "INSERT INTO products " +
                                  "VALUES(@manufacturer, @productName, @size, @sku, @itemType, @basePrice)";
            command.Parameters.Add(new SQLiteParameter("@manufacturer", product.Manufacturer));
            command.Parameters.Add(new SQLiteParameter("@productName", product.ProductName));
            command.Parameters.Add(new SQLiteParameter("@size", product.Size));
            command.Parameters.Add(new SQLiteParameter("@sku", product.SKU));
            command.Parameters.Add(new SQLiteParameter("@itemType", product.ItemType));
            command.Parameters.Add(new SQLiteParameter("@basePrice", product.BasePrice));
            command.ExecuteNonQuery();
            command.Parameters.Clear();
        }

        public void Insert(ScannerData scannerData)
        {
            command.CommandText = "INSERT INTO scannerData (productPurchased, datePurchased, salePrice, customerNumber) " +
                                  "VALUES(@productPurchased, @datePurchased, @salePrice, @customerNumber)";
            command.Parameters.Add(new SQLiteParameter("@productPurchased", scannerData.ProductPurchased));
            command.Parameters.Add(new SQLiteParameter("@datePurchased", scannerData.DatePurchased));
            command.Parameters.Add(new SQLiteParameter("@salePrice", scannerData.SalePrice));
            command.Parameters.Add(new SQLiteParameter("@customerNumber", scannerData.CustomerNumber));
            command.ExecuteNonQuery();
            command.Parameters.Clear();
        }

        private void CreateTables()
        {
            command.CommandText = "CREATE TABLE products (" +
                                  "    manufacturer VARCHAR(255)," +
                                  "    productName VARCHAR(255)," +
                                  "    size VARCHAR(255)," +
                                  "    sku VARCHAR(255) PRIMARY KEY," +
                                  "    itemType VARCHAR(255)," +
                                  "    basePrice DOUBLE" +
                                  ")";
            command.ExecuteNonQuery();

            command.CommandText = "CREATE TABLE scannerData (" +
                                  "    transactionId INTEGER PRIMARY KEY AUTOINCREMENT," +
                                  "    productPurchased VARCHAR(255)," +
                                  "    datePurchased DATE," +
                                  "    salePrice DOUBLE," +
                                  "    customerNumber INTEGER" +
                                  ")";
            command.ExecuteNonQuery();
        }
    }
}