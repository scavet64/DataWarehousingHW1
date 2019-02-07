using System;
using System.Collections.Generic;
using System.Text;

namespace GroceryStoreDataGenerator.Models
{
    public class Product
    {
        [DataName("Manufacturer")]
        public string Manufacturer { get; set; }

        [DataName("Product Name")]
        public string ProductName { get; set; }

        [DataName("Size")]
        public string Size { get; set; }

        [DataName("SKU")]
        public string SKU { get; set; }

        [DataName("itemType")]
        public string ItemType { get; set; }

        [DataName("BasePrice")]
        public Double BasePrice { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Product"/> class.
        /// </summary>
        /// <param name="manufacturer">The manufacturer.</param>
        /// <param name="name">The name.</param>
        /// <param name="size">The size.</param>
        /// <param name="sKU">The s ku.</param>
        /// <param name="itemType">Type of the item.</param>
        /// <param name="basePrice">The base price.</param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public Product(string manufacturer, string productName, string size, string sKU, string itemType, double basePrice)
        {
            Manufacturer = manufacturer ?? throw new ArgumentNullException(nameof(manufacturer));
            ProductName = productName ?? throw new ArgumentNullException(nameof(productName));
            Size = size ?? throw new ArgumentNullException(nameof(size));
            SKU = sKU ?? throw new ArgumentNullException(nameof(sKU));
            ItemType = itemType ?? throw new ArgumentNullException(nameof(itemType));
            BasePrice = basePrice;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Product"/> class.
        /// </summary>
        public Product()
        {
        }

    }
}
