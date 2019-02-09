using System;

namespace GroceryStoreDataGenerator.Models
{
    public class Product
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Product" /> class.
        /// </summary>
        /// <param name="manufacturer">The manufacturer.</param>
        /// <param name="name">The name.</param>
        /// <param name="size">The size.</param>
        /// <param name="sKU">The s ku.</param>
        /// <param name="itemType">Type of the item.</param>
        /// <param name="basePrice">The base price.</param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public Product(string manufacturer, string productName, string size, string sKU, string itemType,
            double basePrice)
        {
            Manufacturer = manufacturer ?? throw new ArgumentNullException(nameof(manufacturer));
            ProductName = productName ?? throw new ArgumentNullException(nameof(productName));
            Size = size ?? throw new ArgumentNullException(nameof(size));
            SKU = sKU ?? throw new ArgumentNullException(nameof(sKU));
            ItemType = itemType ?? throw new ArgumentNullException(nameof(itemType));
            BasePrice = basePrice;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Product" /> class.
        /// </summary>
        public Product()
        {
        }

        public string Manufacturer { get; set; }
        public string ProductName { get; set; }
        public string Size { get; set; }
        public string SKU { get; set; }
        public string ItemType { get; set; }
        public double BasePrice { get; set; }
    }
}