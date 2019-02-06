using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using GroceryStoreDataGenerator.Models;

namespace GroceryStoreDataGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Inventory tmp = Inventory.Instance;
            GroceryStoreSimulation simulation = new GroceryStoreSimulation();
            List<ScannerData> data = simulation.RunSimulation();

            using (var writer = new StreamWriter("data.csv"))
            using (var csv = new CsvWriter(writer))
            {
                csv.WriteRecords(data);
            }
            Console.WriteLine("done");
            Console.ReadLine();
        }
    }
}
