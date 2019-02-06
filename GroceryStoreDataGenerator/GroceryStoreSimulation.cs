using System;
using System.Collections.Generic;
using System.Text;
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

        private readonly Random rng = new Random();
        private readonly string[] TypesToIgnore;
        private readonly List<IProductStatService> statisticsServices;

        public GroceryStoreSimulation()
        {
            statisticsServices = new List<IProductStatService>()
            {
                new BabyFoodAndDiaperStatService(),
                new BreadStatService(),
                new MilkAndCerealStatService(),
                new PeanutButterAndJellyStatService()
            };

            TypesToIgnore = new string[]{
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
                Console.WriteLine($"Finished Day {i}");
            }
            Console.WriteLine(scannerDataList.Count);
            return scannerDataList;
        }

        private void SimulateDay(DateTime currentDate, List<ScannerData> scannerDataList, int i)
        {
            int customersForDay = GetCustomersForDay(currentDate);

            for (int j = 0; j < customersForDay; j++)
            {
                SimulateCustomer(currentDate, scannerDataList, j);
            }

            currentDate.AddDays(1);
        }

        private void SimulateCustomer(DateTime currentDate, List<ScannerData> scannerDataList, int customerNumber)
        {
            List<Product> itemsPurchased = new List<Product>();

            // Handle each of the specific stat based products
            foreach (IProductStatService statService in statisticsServices)
            {
                itemsPurchased.AddRange(statService.GetProductsBasedOnStats());
            }

            //handle everything else
            itemsPurchased.AddRange(Inventory.Instance.GetRandomProducts(rng.Next(MaxItems - itemsPurchased.Count), TypesToIgnore));


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
