using System;
using System.Collections.Generic;

namespace GroceryStoreDataGenerator.ProductStatisticsService
{
    public interface IProductStatisticService
    {
        List<Product> GetProductsBasedOnStats();
    }
}
