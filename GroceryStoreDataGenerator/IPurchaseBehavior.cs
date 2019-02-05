using System;
using System.Collections.Generic;

namespace GroceryStoreDataGenerator
{
    public interface IPurchaseBehavior
    {
        List<Product> GetPurchase();
    }
}
