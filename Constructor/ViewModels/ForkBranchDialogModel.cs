using Constructor.Database;
using Starcounter.Nova;
using Starcounter.Palindrom;
using Starcounter.Palindrom.Transient;

namespace Constructor.ViewModels
{
    public class ForkBranchDialogModel : TransientViewModel
    {
        private bool isVisible;

        public bool IsVisible
        {
            get => isVisible;
            set
            {
                if (value)
                    Name = $"{ParentPage?.Repository?.CurrentBranch?.Name} - fork";
                isVisible = value;
            }
        }

        public string Name { get; set; }

        private ProductPage ParentPage { get; }

        public ForkBranchDialogModel(ProductPage parentPage, IPalindromContext context) : base(context)
        {
            ParentPage = parentPage;
        }

        public void Submit()
        {
            IsVisible = false;
            Branch branch = null;
            ProductPage parent = ParentPage;
            Db.Transact(() =>
            {
                if (!parent.Repository.CurrentCommit.IsClosed)
                    return;
                Branch current = parent.Repository.CurrentBranch;
                var name = Name.Trim();
                if (string.IsNullOrEmpty(name))
                    name = $"{current.Name}-fork";
                branch = Branch.Create(name, current);
                parent.SelectBranch(branch);
            });
            parent.Branches.Add(new BranchModel(branch, ParentPage, Context));
            parent.AddedToCollection(p => p.Branches);
        }

        public void Cancel() => IsVisible = false;
    }
}