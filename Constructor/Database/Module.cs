using Starcounter.Nova;

namespace Constructor.Database
{
    [Database]
    public class Module : Item
    {
        public Product Product { get; set; }

        public Module(Product product) : base(product?.Repository)
        {
            Product = product;
        }

        public int Quantity
        {
            get => Property.GetIntValue(Repository.CurrentCommit, this) ?? 0;
            set => Property.SetIntValue(Repository.CurrentCommit, this, value);
        }

        public int Price
        {
            get => Property.GetIntValue(Repository.CurrentCommit, this) ?? 0;
            set => Property.SetIntValue(Repository.CurrentCommit, this, value);
        }

        /// <summary>
        /// Returns multiplication of <see cref="Quantity"/> and <see cref="Price"/>.
        /// </summary>
        public int TotalAmount => Quantity * Price;
    }
}
