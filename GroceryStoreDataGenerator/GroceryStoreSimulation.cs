using System;
using System.Collections.Generic;
using System.Text;

namespace GroceryStoreDataGenerator
{
    public class GroceryStoreSimulation
    {
        private readonly Random rng = new Random();
        public const int CustomerLow = 1200;
        public const int CustomerHigh = 1250;
        public const double PriceModifier = 1.11;
        public const int MaxItems = 65;
        public const int WeekendCustomerIncrease = 50;
        public const int DaysToRunSimulation = 30;

        private const string MilkType = "Milk";
        private const int milkPercent = 70;
        private const string CerealType = "Cereal";
        private const int cerealPercent = 50;
        private const int cerealWithoutMilk = 5;

        private const string babyFoodType = "Baby Food";
        private const int babyfoodPercent = 20;
        private const string DiaperType = "Diapers";
        private const int diaperPercent = 80;
        private const int diapersWithoutBabyFoodPercent = 1;

        private const string BreadType = "Bread";
        private const int breadPercent = 50;

        private const string PeanutButterType = "Peanut Butter";
        private const int peanutButterPercent = 10;
        private const string JellyJamType = "Jelly/Jam";
        private const int jellyJamPercent = 90;
        private const int jellyJamWithoutPeanutButter = 5;

        public List<ScannerData> RunSimulation()
        {
            DateTime currentDate = new DateTime(2019, 1, 1);
            List<ScannerData> scannerDataList = new List<ScannerData>();
            //Number of days to run simulation
            for (int i = 0; i < DaysToRunSimulation; i++)
            {
                //Number of customers for the day
                int customersForDay = GetCustomersForDay(currentDate);

                for (int j = 0; j < customersForDay; j++)
                {
                    List<Product> itemsPurchased = new List<Product>();
                    //Handle milk
                    itemsPurchased.AddRange(ProcessMilk());

                    //Handle baby food
                    itemsPurchased.AddRange(ProcessBabyFood());

                    //handle bread
                    itemsPurchased.AddRange(ProcessBread());

                    //handle peanut butter
                    itemsPurchased.AddRange(ProcessPeanutButter());

                    //handle everything else
                    itemsPurchased.AddRange(
                    Inventory.Instance.GetRandomProducts(rng.Next(MaxItems - itemsPurchased.Count),
                    BreadType, MilkType, CerealType, DiaperType, babyFoodType, JellyJamType, PeanutButterType));


                    foreach (Product product in itemsPurchased)
                    {
                        //Get Purchase price
                        scannerDataList.Add(new ScannerData(product.SKU, currentDate.ToShortDateString(), product.BasePrice * PriceModifier, j));
                    }
                }

                currentDate.AddDays(1);
            }
            Console.WriteLine(scannerDataList.Count);
            return scannerDataList;
        }

        private List<Product> ProcessMilk()
        {
            List<Product> products = new List<Product>();

            //got milk?
            if (getPercentage() <= milkPercent)
            {
                products.Add(Inventory.Instance.GetRandomProductByType(MilkType));
                //got cereal?
                if (getPercentage() <= cerealPercent)
                {
                    products.Add(Inventory.Instance.GetRandomProductByType(CerealType));
                }
            }
            else
            {
                //cereal without milk
                if (getPercentage() <= cerealWithoutMilk)
                {
                    products.Add(Inventory.Instance.GetRandomProductByType(CerealType));
                }
            }

            return products;
        }

        private List<Product> ProcessBabyFood()
        {
            List<Product> products = new List<Product>();

            //got milk?
            if (getPercentage() <= babyfoodPercent)
            {
                products.Add(Inventory.Instance.GetRandomProductByType(babyFoodType));
                //got cereal?
                if (getPercentage() <= diaperPercent)
                {
                    products.Add(Inventory.Instance.GetRandomProductByType(DiaperType));
                }
            }
            else
            {
                //cereal without milk
                if (getPercentage() <= diapersWithoutBabyFoodPercent)
                {
                    products.Add(Inventory.Instance.GetRandomProductByType(DiaperType));
                }
            }

            return products;
        }

        private List<Product> ProcessBread()
        {
            List<Product> products = new List<Product>();

            //got milk?
            if (getPercentage() <= breadPercent)
            {
                products.Add(Inventory.Instance.GetRandomProductByType(BreadType));
            }

            return products;
        }

        private List<Product> ProcessPeanutButter()
        {
            List<Product> products = new List<Product>();

            //got peanut butter?
            if (getPercentage() <= peanutButterPercent)
            {
                products.Add(Inventory.Instance.GetRandomProductByType(PeanutButterType));
                //got jelly/jam?
                if (getPercentage() <= jellyJamPercent)
                {
                    products.Add(Inventory.Instance.GetRandomProductByType(JellyJamType));
                }
            }
            else
            {
                //jelly/jam without peanut butter
                if (getPercentage() <= jellyJamWithoutPeanutButter)
                {
                    products.Add(Inventory.Instance.GetRandomProductByType(JellyJamType));
                }
            }

            return products;
        }

        private int getPercentage()
        {
            return rng.Next(100) + 1;
        }

        private int GetCustomersForDay(DateTime currentDate)
        {
            int customersForDay = rng.Next(CustomerLow, CustomerHigh);
            if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday)
            {
                customersForDay += WeekendCustomerIncrease;
            }
            return customersForDay;
        }
    }
}
