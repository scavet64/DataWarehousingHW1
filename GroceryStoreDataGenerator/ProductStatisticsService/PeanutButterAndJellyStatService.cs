using System.Collections.Generic;
using GroceryStoreDataGenerator.Models;

namespace GroceryStoreDataGenerator.ProductStatisticsService
{
    public class PeanutButterAndJellyStatService : AbstractStatService
    {
        public const string PeanutButterType = "Peanut Butter";
        private const int PeanutButterPercent = 10;
        public const string JellyJamType = "Jelly/Jam";
        private const int JellyJamPercent = 90;
        private const int JellyJamWithoutPeanutButter = 5;

        public override List<Product> GetProductsBasedOnStats()
        {
            var products = new List<Product>();

            //got peanut butter?
            if (GetPercentage() <= PeanutButterPercent)
            {
                products.Add(Inventory.GetRandomProductByType(PeanutButterType));

                //got jelly/jam?
                if (GetPercentage() <= JellyJamPercent)
                {
                    products.Add(Inventory.GetRandomProductByType(JellyJamType));
                }
            }
            else
            {
                //jelly/jam without peanut butter
                if (GetPercentage() <= JellyJamWithoutPeanutButter)
                {
                    products.Add(Inventory.GetRandomProductByType(JellyJamType));
                }
            }

            return products;
        }
    }
}