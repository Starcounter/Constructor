using System;
using Starcounter.Nova;

namespace Constructor.Database
{
    [Database]
    public abstract class Module : Item, IEquatable<Module>
    {
        public abstract Product Product { get; set; }

        public static Module Create(Product product)
        {
            var instance = CreateItem<Module>(product?.Repository);
            instance.Product = product;
            return instance;
        }

        public int Quantity
        {
            get => PropertyCrud.GetIntValue(this) ?? 0;
            set => PropertyCrud.SetIntValue(this, value);
        }

        public int Price
        {
            get => PropertyCrud.GetIntValue(this) ?? 0;
            set => PropertyCrud.SetIntValue(this, value);
        }

        /// <summary>
        /// Returns multiplication of <see cref="Quantity"/> and <see cref="Price"/>.
        /// </summary>
        public int TotalAmount => Quantity * Price;

        public bool Equals(Module other) => Db.Equals(this, other);
    }


}
