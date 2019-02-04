using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GroceryStoreDataGenerator
{
    //TODO: Maybe make a mapping of the category to a list of indexes. Then create a method could use this map and get direct access to the main inventory list.
    public class Inventory
    {
        private const string filename = "Products1.txt";
        public readonly List<Product> InventoryList;

        private static Inventory instance;
        public static Inventory Instance => instance ?? (instance = new Inventory());

        public Inventory()
        {
            InventoryList = new List<Product>();
            ReadFile();
        }

        private void ReadFile()
        {
            using (var reader = new StreamReader(filename))
            {
                // Get the header file information
                string header = reader.ReadLine();
                string[] headers = header.Split('|');

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split('|');
                    Product prod = new Product();

                    //Trying to do this even better with custom attributes
                    //var temp = prod.GetType().GetProperties();
                    //foreach (PropertyInfo propinfo in temp)
                    //{
                    //    string dataName = propinfo.CustomAttributes.First().ConstructorArguments.First().Value.ToString();
                    //    propinfo.SetValue(values);
                    //}

                    //Lazy way
                    for (int i = 0; i < headers.Length; i++)
                    {
                        prod.GetType().GetProperty(headers[i]).SetValue(prod, values[i]);
                    }
                    InventoryList.Add(prod);
                }
            }
        }
    }
}
