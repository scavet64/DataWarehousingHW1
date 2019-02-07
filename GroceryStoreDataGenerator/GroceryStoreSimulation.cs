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

        private readonly Inventory _groceryStoreInventory;
        private readonly Action<int,int> _progressCallback;
        private readonly Random _rng = new Random();
        private readonly string[] _typesToIgnore;
        private readonly List<IProductStatService> _statisticsServices;

        public GroceryStoreSimulation(Inventory groceryStoreInventory, Action<int,int> progressCallback = null)
        {
            _groceryStoreInventory = groceryStoreInventory;
            _progressCallback = progressCallback;

            _statisticsServices = new List<IProductStatService>()
            {
                new BabyFoodAndDiaperStatService {Inventory = groceryStoreInventory},
                new BreadStatService {Inventory = groceryStoreInventory},
                new MilkAndCerealStatService {Inventory = groceryStoreInventory},
                new PeanutButterAndJellyStatService {Inventory = groceryStoreInventory}
            };

            _typesToIgnore = new[]{
                BabyFoodAndDiaperStatService.babyFoodType,
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
            DateTime currentDate = new DateTime(2019, 1, 1);
            List<ScannerData> scannerDataList = new List<ScannerData>();

            for (int i = 0; i < DaysToRunSimulation; i++)
            {
                SimulateDay(currentDate, scannerDataList, i);
                _progressCallback?.Invoke(i, DaysToRunSimulation);
                currentDate = currentDate.AddDays(1);
            }

            return scannerDataList;
        }

        private void SimulateDay(DateTime currentDate, List<ScannerData> scannerDataList, int i)
        {
            int customersForDay = GetCustomersForDay(currentDate);

            for (int j = 0; j < customersForDay; j++)
            {
                SimulateCustomer(currentDate, scannerDataList, j);
            }
        }

        private void SimulateCustomer(DateTime currentDate, List<ScannerData> scannerDataList, int customerNumber)
        {
            List<Product> itemsPurchased = new List<Product>();

            // Handle each of the specific stat based products
            foreach (IProductStatService statService in _statisticsServices)
            {
                itemsPurchased.AddRange(statService.GetProductsBasedOnStats());
            }

            //handle everything else
            itemsPurchased.AddRange(_groceryStoreInventory.GetRandomProducts(_rng.Next(MaxItems - itemsPurchased.Count), _typesToIgnore));


            foreach (Product product in itemsPurchased)
            {
                //Add Scanner data for each item the customer bought
                scannerDataList.Add(new ScannerData(product.SKU, currentDate.ToShortDateString(), product.BasePrice * PriceModifier, customerNumber));
            }
        }

        private int GetCustomersForDay(DateTime currentDate)
        {
            int customersForDay = new Random().Next(CustomerLow, CustomerHigh);
            if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday)
            {
                customersForDay += WeekendCustomerIncrease;
            }
            return customersForDay;
        }
    }
}
