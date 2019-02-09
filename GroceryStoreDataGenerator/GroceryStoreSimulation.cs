using System;
using System.Collections.Generic;
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

        private readonly Inventory groceryStoreInventory;
        private readonly Action<int, int> progressCallback;
        private readonly Random rng = new Random();
        private readonly List<IProductStatService> statisticsServices;
        private readonly string[] typesToIgnore;

        public GroceryStoreSimulation(Inventory groceryStoreInventory, Action<int, int> progressCallback = null)
        {
            this.groceryStoreInventory = groceryStoreInventory;
            this.progressCallback = progressCallback;

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

        public List<ScannerData> RunSimulation()
        {
            var currentDate = new DateTime(2019, 1, 1);
            var scannerDataList = new List<ScannerData>();

            for (var i = 0; i < DaysToRunSimulation; i++)
            {
                SimulateDay(currentDate, scannerDataList, i);
                progressCallback?.Invoke(i, DaysToRunSimulation);
                currentDate = currentDate.AddDays(1);
            }

            return scannerDataList;
        }

        private void SimulateDay(DateTime currentDate, List<ScannerData> scannerDataList, int i)
        {
            var customersForDay = GetCustomersForDay(currentDate);

            for (var j = 0; j < customersForDay; j++)
            {
                SimulateCustomer(currentDate, scannerDataList, j);
            }
        }

        private void SimulateCustomer(DateTime currentDate, List<ScannerData> scannerDataList, int customerNumber)
        {
            var itemsPurchased = new List<Product>();

            // Handle each of the specific stat based products
            foreach (var statService in statisticsServices)
            {
                itemsPurchased.AddRange(statService.GetProductsBasedOnStats());
            }

            //handle everything else
            itemsPurchased.AddRange(groceryStoreInventory.GetRandomProducts(rng.Next(MaxItems - itemsPurchased.Count),
                                                                            typesToIgnore));

            foreach (var product in itemsPurchased)
            {
                //Add Scanner data for each item the customer bought
                scannerDataList.Add(new ScannerData(product.SKU, currentDate.ToShortDateString(),
                                                    product.BasePrice * PriceModifier, customerNumber));
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