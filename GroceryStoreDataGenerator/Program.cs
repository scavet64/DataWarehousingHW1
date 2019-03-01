using System;
using GroceryStoreDataGenerator.Database;

namespace GroceryStoreDataGenerator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var sqliteHandler = new SQLiteHandler("GroceryStore.sqlite"))
            {
                Console.Write("Enter inventory file: ");
                var filename = Console.ReadLine();

                Console.WriteLine("Reading inventory list...");
                Inventory groceryStoreInventory;
                using (var transaction = sqliteHandler.SQLiteConnection.BeginTransaction())
                {
                    groceryStoreInventory = new Inventory(filename, sqliteHandler);
                    transaction.Commit();
                }

                Console.WriteLine("Running simulation...");
                using (var transaction = sqliteHandler.SQLiteConnection.BeginTransaction())
                {
                    var simulation = new GroceryStoreSimulation(groceryStoreInventory, sqliteHandler, Progress);
                    simulation.RunSimulation();
                    transaction.Commit();
                }

                var summary = new GroceryStoreSummary(sqliteHandler);

                Console.WriteLine($"\n\nNumber of customers: {summary.NumberOfCustomers}\n");
                Console.WriteLine($"Total sales: ${Math.Round(summary.TotalSales, 2)}\n");
                Console.WriteLine($"Total items bought: {summary.TotalItemsBought}\n");
                Console.WriteLine("Top 10 selling items with counts: ");
                foreach (var (item, count) in summary.TopTenSellingItemsWithCounts) Console.WriteLine($"\t{item}\t{count}");

                Console.WriteLine("Done. Press enter to exit...");
                Console.ReadLine();
            }
        }

        private static void Progress(int day, int total)
        {
            Console.Write($"Finished Day: {++day} / {total}\t|");

            var percent = day * 100 / total / 2;
            for (var i = 0; i < percent; i++) Console.Write("=");
            for (var i = percent; i < 50; i++) Console.Write("-");

            Console.Write("|\r");
        }
    }
}