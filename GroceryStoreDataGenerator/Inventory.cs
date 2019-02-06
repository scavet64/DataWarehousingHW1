using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using GroceryStoreDataGenerator.Models;

namespace GroceryStoreDataGenerator
{
    public class Inventory
    {
        private readonly Dictionary<string, List<Product>> _itemTypeToProductList;

        public Inventory(string filename)
        {
            _itemTypeToProductList = new Dictionary<string, List<Product>>();
            ReadFile(filename);
        }

        private void ReadFile(string filename)
        {
            using (var reader = new StreamReader(filename))
            {
                // Get the header file information
                string[] headers = reader.ReadLine().Split('|');

                while (!reader.EndOfStream)
                {
                    string[] values = reader.ReadLine().Split('|');
                    Product product = new Product();

                    for (int i = 0; i < headers.Length; i++)
                    {
                        object value;
                        PropertyInfo propertyInfo = product.GetType().GetProperty(headers[i]);
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
                    if (!_itemTypeToProductList.ContainsKey(product.ItemType))
                    {
                        //First time that we saw this key, Make a new list and add it to the map
                        _itemTypeToProductList.Add(product.ItemType, new List<Product>());
                    }
                    _itemTypeToProductList[product.ItemType].Add(product);

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
            if (!_itemTypeToProductList.ContainsKey(type))
            {
                throw new ArgumentException($"Item type {type} does not exist in the inventory.");
            }

            return _itemTypeToProductList[type];
        }

        public Product GetRandomProductByType(string type)
        {
            List<Product> products = GetProductsByType(type);
            return products[new Random().Next(products.Count)];
        }

        public List<Product> GetRandomProducts(int numberOfProducts, params string[] typeToIgnore)
        {
            List<Product> randomProducts = new List<Product>();

            List<Product> searchList = new List<Product>();
            foreach (var key in _itemTypeToProductList.Keys)
            {
                if (!typeToIgnore.Contains(key))
                {
                    searchList.AddRange(_itemTypeToProductList[key]);
                }
            }

            Random rng = new Random();
            for (int i = 0; i < numberOfProducts; i++)
            {
                randomProducts.Add(searchList[rng.Next(searchList.Count)]);
            }

            return randomProducts;
        }

        //testing
        public int GetCount()
        {
            return _itemTypeToProductList.Values.Count;
        }
    }
}
