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

        public override List<Product> GetProductsBasedOnStats(int itemsLeft)
        {
            var products = new List<Product>();

            if (GetPercentage() <= PeanutButterPercent)
            {
                products.Add(Inventory.PurchaseRandomProductByType(PeanutButterType));
                itemsLeft--;

                if (GetPercentage() <= JellyJamPercent && itemsLeft > 0)
                {
                    products.Add(Inventory.PurchaseRandomProductByType(JellyJamType));
                }
            }
            else
            {
                if (GetPercentage() <= JellyJamWithoutPeanutButter)
                {
                    products.Add(Inventory.PurchaseRandomProductByType(JellyJamType));
                }
            }

            return products;
        }
    }
}