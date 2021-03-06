﻿using System;
using System.Collections.Generic;
using GroceryStoreDataGenerator.Database;
using GroceryStoreDataGenerator.Models;
using GroceryStoreDataGenerator.ProductStatisticsService;

namespace GroceryStoreDataGenerator
{
    public class GroceryStoreSimulation
    {
        public const int CustomerLow = 1200;
        public const int CustomerHigh = 1250;
        public const double PriceModifier = 1.11;
        public const int MaxItems = 65;
        public const int WeekendCustomerIncrease = 50;
        public const int DaysToRunSimulation = 365;

        private readonly HashSet<DayOfWeek> deliveryDays; 
        private readonly Inventory groceryStoreInventory;
        private readonly Action<int, int> progressCallback;
        private readonly Random rng = new Random();
        private readonly List<IProductStatService> statisticsServices;
        private readonly string[] typesToIgnore;
        private readonly SQLiteHandler sqliteHandler;

        public GroceryStoreSimulation(Inventory groceryStoreInventory, SQLiteHandler sqliteHandler, Action<int, int> progressCallback = null)
        {
            this.groceryStoreInventory = groceryStoreInventory;
            this.progressCallback = progressCallback;
            this.sqliteHandler = sqliteHandler;

            deliveryDays = new HashSet<DayOfWeek> { DayOfWeek.Tuesday, DayOfWeek.Thursday, DayOfWeek.Saturday };

            statisticsServices = new List<IProductStatService>
                                 {
                                     new BabyFoodAndDiaperStatService {Inventory = groceryStoreInventory},
                                     new BreadStatService {Inventory = groceryStoreInventory},
                                     new MilkAndCerealStatService {Inventory = groceryStoreInventory},
                                     new PeanutButterAndJellyStatService {Inventory = groceryStoreInventory}
                                 };

            typesToIgnore = new[]
                            {
                                BabyFoodAndDiaperStatService.BabyFoodType,
                                BabyFoodAndDiaperStatService.DiaperType,
                                BreadStatService.BreadType,
                                MilkAndCerealStatService.CerealType,
                                MilkAndCerealStatService.MilkType,
                                PeanutButterAndJellyStatService.JellyJamType,
                                PeanutButterAndJellyStatService.PeanutButterType
                            };
        }

        public void RunSimulation()
        {
            var currentDate = new DateTime(2019, 1, 1);

            for (var i = 0; i < DaysToRunSimulation; i++)
            {
                SimulateDay(currentDate);
                progressCallback?.Invoke(i, DaysToRunSimulation);
                currentDate = currentDate.AddDays(1);
            }
        }

        private void SimulateDay(DateTime currentDate)
        {
            groceryStoreInventory.MilkDelivery();
            if (deliveryDays.Contains(currentDate.DayOfWeek))
            {
                groceryStoreInventory.OtherDeliveries();
            }

            var customersForDay = GetCustomersForDay(currentDate);

            for (var j = 0; j < customersForDay; j++)
            {
                SimulateCustomer(currentDate, j);
            }
        }

        private void SimulateCustomer(DateTime currentDate, int customerNumber)
        {
            var itemsToPurchase = rng.Next(1, MaxItems + 1);
            var itemsLeft = itemsToPurchase;

            var itemsPurchased = new List<Product>(itemsToPurchase);

            // Handle each of the specific stat based products
            foreach (var statService in statisticsServices)
            {
                itemsLeft = itemsToPurchase - itemsPurchased.Count;
                if (itemsLeft == 0)
                {
                    break;
                }

                itemsPurchased.AddRange(statService.GetProductsBasedOnStats(itemsLeft));
            }

            //handle everything else
            itemsPurchased.AddRange(groceryStoreInventory.PurchaseRandomProducts(itemsLeft, typesToIgnore));

            foreach (var product in itemsPurchased)
            {
                //Add Scanner data for each item the customer bought
                var scannerData = new ScannerData(product.SKU, currentDate.ToShortDateString(), product.BasePrice * PriceModifier, 
                                                  customerNumber, product.ItemsLeft, product.TotalCasesOrdered);

                sqliteHandler.Insert(scannerData);
            }
        }

        private int GetCustomersForDay(DateTime currentDate)
        {
            var customersForDay = new Random().Next(CustomerLow, CustomerHigh);
            if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday)
            {
                customersForDay += WeekendCustomerIncrease;
            }

            return customersForDay;
        }
    }
}