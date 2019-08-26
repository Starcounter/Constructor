using Starcounter.Nova;

namespace Constructor.Database
{
    [Database]
    public abstract class Module : Item
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
