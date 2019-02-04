using System;
using System.Collections.Generic;
using System.Text;

namespace GroceryStoreDataGenerator
{
    public class DataName : Attribute
    {
        public string Value;

        public DataName(string value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}
