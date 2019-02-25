using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using GroceryStoreDataGenerator.Database;
using GroceryStoreDataGenerator.Models;

namespace GroceryStoreDataGenerator
{
    public class Inventory
    {
        private readonly Dictionary<string, List<Product>> itemTypeToProductList;

        public Inventory(string filename, SQLiteHandler sqliteHandler = null)
        {
            itemTypeToProductList = new Dictionary<string, List<Product>>();
            ReadFile(filename, sqliteHandler);
        }

        private void ReadFile(string filename, SQLiteHandler sqliteHandler)
        {
            using (var reader = new StreamReader(filename))
            {
                // Get the header file information
                var headers = reader.ReadLine()?.Split('|');
                if (headers == null) throw new FormatException($"Could not read headers in file {filename}.");

                while (!reader.EndOfStream)
                {
                    var values = reader.ReadLine()?.Split('|');
                    if (values == null) throw new FormatException($"Could not read values in file {filename}.");

                    var product = new Product();
                    for (var i = 0; i < headers.Length; i++)
                    {
                        object value;
                        var propertyInfo = product.GetType().GetProperty(headers[i]);
                        if (propertyInfo.PropertyType == typeof(double))
                        {
                            //Parse the double (this is for the price)
                            value = double.Parse(Regex.Match(values[i], @"(\d+(\.\d+)?)|(\.\d+)").Value);
                        }
                        else
                        {
                            value = values[i];
                        }

                        propertyInfo.SetValue(product, value);
                    }

                    //Get Store the product category into the map for quick, direct access to the main inventory list
                    if (!itemTypeToProductList.ContainsKey(product.ItemType))
                    {
                        //First time that we saw this key, Make a new list and add it to the map
                        itemTypeToProductList.Add(product.ItemType, new List<Product>());
                    }

                    itemTypeToProductList[product.ItemType].Add(product);
                    sqliteHandler?.Insert(product);
                }
            }
        }

        /// <summary>
        ///     Gets a list of products that have the passed in type.
        /// </summary>
        /// <returns>The type to get products of.</returns>
        /// <param name="type">Type.</param>
        public List<Product> GetProductsByType(string type)
        {
            if (!itemTypeToProductList.ContainsKey(type))
            {
                throw new ArgumentException($"Item type {type} does not exist in the inventory.");
            }

            return itemTypeToProductList[type];
        }

        public Product GetRandomProductByType(string type)
        {
            var products = GetProductsByType(type);
            return products[new Random().Next(products.Count)];
        }

        public List<Product> GetRandomProducts(int numberOfProducts, params string[] typeToIgnore)
        {
            var randomProducts = new List<Product>();

            var searchList = new List<Product>();
            foreach (var key in itemTypeToProductList.Keys)
            {
                if (!typeToIgnore.Contains(key))
                {
                    searchList.AddRange(itemTypeToProductList[key]);
                }
            }

            var rng = new Random();
            for (var i = 0; i < numberOfProducts; i++)
            {
                randomProducts.Add(searchList[rng.Next(searchList.Count)]);
            }

            return randomProducts;
        }
    }
}