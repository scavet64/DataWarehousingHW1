using System;
using System.Collections.Generic;
using GroceryStoreDataGenerator.Models;

namespace GroceryStoreDataGenerator.ProductStatisticsService
{
    public class PeanutButterAndJellyStatService : AbstractStatService
    {
        public const string PeanutButterType = "Peanut Butter";
        private const int peanutButterPercent = 10;
        public const string JellyJamType = "Jelly/Jam";
        private const int jellyJamPercent = 90;
        private const int jellyJamWithoutPeanutButter = 5;

        public PeanutButterAndJellyStatService()
        {
        }

        public override List<Product> GetProductsBasedOnStats()
        {
            List<Product> products = new List<Product>();

            //got peanut butter?
            if (GetPercentage() <= peanutButterPercent)
            {
                products.Add(Inventory.Instance.GetRandomProductByType(PeanutButterType));
                //got jelly/jam?
                if (GetPercentage() <= jellyJamPercent)
                {
                    products.Add(Inventory.Instance.GetRandomProductByType(JellyJamType));
                }
            }
            else
            {
                //jelly/jam without peanut butter
                if (GetPercentage() <= jellyJamWithoutPeanutButter)
                {
                    products.Add(Inventory.Instance.GetRandomProductByType(JellyJamType));
                }
            }

            return products;
        }
    }
}
