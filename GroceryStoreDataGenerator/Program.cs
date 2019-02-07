﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.ConstrainedExecution;
using CsvHelper;
using GroceryStoreDataGenerator.Models;

namespace GroceryStoreDataGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter inventory file: ");
            string filename = Console.ReadLine();

            Console.WriteLine("Reading inventory list...");

            Inventory groceryStoreInventory = new Inventory(filename);

            Console.WriteLine("Running simulation...");

            GroceryStoreSimulation simulation = new GroceryStoreSimulation(groceryStoreInventory, Progress);

            List<ScannerData> data = simulation.RunSimulation();

            GroceryStoreSummary summary = new GroceryStoreSummary(data);

            Console.WriteLine();

            Console.WriteLine($"Number of customers: {summary.NumberOfCustomers}");

            Console.WriteLine($"Total sales: ${Math.Round(summary.TotalSales, 2)}");

            Console.WriteLine($"Total items bought: {summary.TotalItemsBought}");

            Console.WriteLine($"Top 10 selling items with counts: ");

            foreach (var (item, count) in summary.TopTenSellingItemsWithCounts)
            {
                Console.WriteLine($"\t{item}\t{count}");
            }

            Console.WriteLine("Writing to data.csv...");

            using (var writer = new StreamWriter("data.csv"))
            using (var csv = new CsvWriter(writer))
            {
                csv.WriteRecords(data);
            }

            Console.WriteLine("Done. Press enter to exit...");
            Console.ReadLine();
        }

        private static void Progress(int day, int total)
        {
            Console.Write($"Finished Day: {++day} / {total}\t|");

            int percent = (day * 100 / total) / 2;

            for (int i = 0; i < percent; i++)
            {
                Console.Write("=");
            }

            for (int i = percent; i < 50; i++)
            {
                Console.Write("-");
            }

            Console.Write("|\r");
        }
    }
}
