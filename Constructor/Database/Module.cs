using System;
using Starcounter;

namespace Constructor.Database
{
    [Database]
    public class Module : Item
    {
        public Product Product { get; set; }

        public Module(Product product) : base(product?.Repository)
        {
            this.Product = product;
        }

        public int Quantity
        {
            get
            {
                return Property.GetIntValue(this.Repository.CurrentCommit, this) ?? 0;
            }
            set
            {
                Property.SetIntValue(this.Repository.CurrentCommit, this, value);
            }
        }

        public int Price
        {
            get
            {
                return Property.GetIntValue(this.Repository.CurrentCommit, this) ?? 0;
            }
            set
            {
                Property.SetIntValue(this.Repository.CurrentCommit, this, value);
            }
        }

        /// <summary>
        /// Returns multiplication of <see cref="Quantity"/> and <see cref="Price"/>.
        /// </summary>
        public int TotalAmount
        {
            get
            {
                return this.Quantity * this.Price;
            }
        }
    }
}
