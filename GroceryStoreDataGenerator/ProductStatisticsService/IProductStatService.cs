using System.Collections.Generic;
using GroceryStoreDataGenerator.Models;

namespace GroceryStoreDataGenerator.ProductStatisticsService
{
    public interface IProductStatService
    {
        List<Product> GetProductsBasedOnStats(int itemsLeft);
    }
}
