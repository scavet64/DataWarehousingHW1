using System;

namespace GroceryStoreDataGenerator
{
    class Program
    {


        static void Main(string[] args)
        {
            Inventory tmp = Inventory.Instance;
            Console.WriteLine(tmp.InventoryList.Count);
            Console.ReadLine();
        }
    }
}
