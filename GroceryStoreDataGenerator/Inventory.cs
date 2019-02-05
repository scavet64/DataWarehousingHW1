using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace GroceryStoreDataGenerator
{
    //TODO: Maybe make a mapping of the category to a list of indexes. Then create a method could use this map and get direct access to the main inventory list.
    public class Inventory
    {
        private const string filename = "Products1.txt";
        private readonly List<Product> InventoryList;
        private readonly Dictionary<string, List<int>> ItemTypeToIndexList;

        private static Inventory instance;
        public static Inventory Instance => instance ?? (instance = new Inventory());

        private Inventory()
        {
            InventoryList = new List<Product>();
            ItemTypeToIndexList = new Dictionary<string, List<int>>();
            ReadFile();
        }

        private void ReadFile()
        {
            using (var reader = new StreamReader(filename))
            {
                // Get the header file information
                string header = reader.ReadLine();
                string[] headers = header.Split('|');
                int indexCounter = 0;

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
                        object value;
                        PropertyInfo propInfo = prod.GetType().GetProperty(headers[i]);
                        if (propInfo.PropertyType == typeof(Double))
                        {
                            //Parse the double (this is for the price)
                            value = double.Parse(Regex.Match(values[i], @"(\d+(\.\d+)?)|(\.\d+)").Value);
                        }
                        else
                        {
                            value = values[i];
                        }
                        propInfo.SetValue(prod, value);
                    }
                    InventoryList.Add(prod);

                    //Get Store the product category into the map for quick, direct access to the main inventory list
                    if (!ItemTypeToIndexList.ContainsKey(prod.ItemType))
                    {
                        //First time that we saw this key, Make a new list and add it to the map
                        ItemTypeToIndexList.Add(prod.ItemType, new List<int>());
                    }
                    ItemTypeToIndexList[prod.ItemType].Add(indexCounter++);

                }
            }
        }

        /// <summary>
        /// Gets a list of products that have the passed in type.
        /// </summary>
        /// <returns>The type to get products of.</returns>
        /// <param name="type">Type.</param>
        public List<Product> GetProductsByType(string type)
        {
            List<Product> products = new List<Product>();
            List<int> indexesForType = ItemTypeToIndexList[type];
            if (indexesForType != null)
            {
                foreach (int index in indexesForType)
                {
                    products.Add(InventoryList[index]);
                }
            }

            return products;
        }

        public Product GetRandomProductByType(string type)
        {
            List<Product> products = GetProductsByType(type);
            return products[new Random().Next(products.Count)];
        }

        public List<Product> GetRandomProducts(int numberOfProds, params string[] typeToIgnore)
        {
            Random rng = new Random();
            List<Product> randomProducts = new List<Product>();
            for(int i = 0; i < numberOfProds; i++)
            {
                Product randomProd = InventoryList[rng.Next(InventoryList.Count)];
                if (!typeToIgnore.Contains(randomProd.ItemType))
                {
                    randomProducts.Add(randomProd);
                }
                else
                {
                    //This feels dirty but it makes sure the number of products is correct
                    i--;
                }
            }
            return randomProducts;
        }

        //testing
        public int getCount()
        {
            return InventoryList.Count;
        }
    }
}
