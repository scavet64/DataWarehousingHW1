using System.Collections.Generic;
using GroceryStoreDataGenerator.Models;

namespace GroceryStoreDataGenerator.ProductStatisticsService
{
    public class MilkAndCerealStatService : AbstractStatService
    {
        public const string MilkType = "Milk";
        private const int MilkPercent = 70;
        public const string CerealType = "Cereal";
        private const int CerealPercent = 50;
        private const int CerealWithoutMilk = 5;

        public override List<Product> GetProductsBasedOnStats(int itemsLeft)
        {
            var products = new List<Product>();

            if (GetPercentage() <= MilkPercent)
            {
                products.Add(Inventory.PurchaseRandomProductByType(MilkType));
                itemsLeft--;

                if (GetPercentage() <= CerealPercent && itemsLeft > 0)
                {
                    products.Add(Inventory.PurchaseRandomProductByType(CerealType));
                }
            }
            else
            {
                if (GetPercentage() <= CerealWithoutMilk)
                {
                    products.Add(Inventory.PurchaseRandomProductByType(CerealType));
                }
            }

            return products;
        }
    }
}