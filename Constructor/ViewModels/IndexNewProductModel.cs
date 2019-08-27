using Constructor.Database;
using Starcounter.Palindrom;
using Starcounter.Palindrom.Database;
using Starcounter.Palindrom.Transient;

namespace Constructor.ViewModels
{
    public class IndexNewProductModel : TransientViewModel
    {
        private IndexPage Parent { get; }
        private ITransactionFactory TransactionFactory { get; }

        private bool isVisible;
        private string name;

        public bool IsVisible
        {
            get => isVisible;
            set
            {
                isVisible = value;
                this.MemberChanged(i => i.IsVisible);
            }
        }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                this.MemberChanged(i => i.Name);
            }
        }

        public IndexNewProductModel(IndexPage parent, IPalindromContext context, ITransactionFactory transactionFactory) : base(context)
        {
            Parent = parent;
            TransactionFactory = transactionFactory;
        }

        public void Submit()
        {
            if (string.IsNullOrWhiteSpace(Name))
                return;
            var trimmedName = Name.Trim();
            TransactionFactory.Transact(() =>
            {
                var repository = Repository.Create(Name + " Repository");
                var product = Product.Create(repository);
                product.Name = trimmedName;
            });
            IsVisible = false;
            Parent.RefreshProducts();
        }

        public void Cancel() => IsVisible = false;
    }
}