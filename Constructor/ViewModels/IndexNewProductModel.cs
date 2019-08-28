using Constructor.Database;
using Starcounter.Palindrom;
using Starcounter.Palindrom.Transient;

namespace Constructor.ViewModels
{
    public class IndexNewProductModel : TransientViewModel
    {
        private IndexPage Parent { get; }

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

        public IndexNewProductModel(IndexPage parent, IPalindromContext context) : base(context)
        {
            Parent = parent;
        }

        public void Submit()
        {
            if (string.IsNullOrWhiteSpace(Name))
                return;
            var trimmedName = Name.Trim();
            var repository = Repository.Create(Name + " Repository");
            var product = Product.Create(repository);
            product.Name = trimmedName;
            IsVisible = false;
            Parent.RefreshProducts();
        }

        public void Cancel() => IsVisible = false;
    }
}