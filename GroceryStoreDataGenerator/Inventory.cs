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

                    product.ItemsLeft = product.ItemType == "Milk"
                                            ? (int) Math.Ceiling(1.5 * DailySupplyPerItem(product.ItemType))
                                            : 3 * DailySupplyPerItem(product.ItemType);

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

        public Product PurchaseRandomProductByType(string type)
        {
            var products = GetProductsByType(type);

            var product = products[new Random().Next(products.Count)];
            while (product.ItemsLeft == 0)
            {
                product = products[new Random().Next(products.Count)];
            }

            product.ItemsLeft--;
            return product;
        }

        public List<Product> PurchaseRandomProducts(int numberOfProducts, params string[] typeToIgnore)
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
                var product = searchList[rng.Next(searchList.Count)];
                while (product.ItemsLeft == 0)
                {
                    product = searchList[rng.Next(searchList.Count)];
                }

                product.ItemsLeft--;
                randomProducts.Add(product);
            }

            return randomProducts;
        }

        public int DailySupplyPerItem(string itemType)
        {
            switch (itemType)
            {
                case "Milk":
                    return 145;
                case "Cereal":
                    return 5;
                case "Baby Food":
                    return 2;
                case "Diapers":
                    return 3;
                case "Bread":
                    return 13;
                case "Peanut Butter":
                    return 6;
                case "Jelly/Jam":
                    return 45;
                default:
                    return 23;
            }
        }

        public void MilkDelivery()
        {
            int dailySupply = (int)Math.Ceiling(1.5 * DailySupplyPerItem("Milk"));
            foreach (var milkItem in itemTypeToProductList["Milk"])
            {
                if (milkItem.ItemsLeft < dailySupply)
                {
                    var casesToOrder = (int)Math.Ceiling((dailySupply - milkItem.ItemsLeft) / 12.0) ;
                    milkItem.TotalCasesOrdered += casesToOrder;
                    milkItem.ItemsLeft += 12 * casesToOrder;
                }
            }
        }

        public void OtherDeliveries()
        {
            foreach (var category in itemTypeToProductList.Keys)
            {
                if (category == "Milk") continue;

                int dailySupply = 3 * DailySupplyPerItem(category);
                foreach (var item in itemTypeToProductList[category])
                {
                    if (item.ItemsLeft < dailySupply)
                    {
                        var casesToOrder = (int)Math.Ceiling((dailySupply - item.ItemsLeft) / 12.0);
                        item.TotalCasesOrdered += casesToOrder;
                        item.ItemsLeft += 12 * casesToOrder;
                    }
                }
            }
        }
    }
}