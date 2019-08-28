using Constructor.Database;
using Starcounter.Palindrom;
using Starcounter.Palindrom.Transient;

namespace Constructor.ViewModels
{
    public class ForkBranchDialogModel : TransientViewModel
    {
        private bool isVisible;
        private string name;

        public bool IsVisible
        {
            get => isVisible;
            set
            {
                if (value)
                {
                    Name = $"{ParentPage?.Repository?.CurrentBranch?.Name} - fork";
                    this.MemberChanged(m => m.Name);
                }
                isVisible = value;
                this.MemberChanged(m => m.IsVisible);
            }
        }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                this.MemberChanged(m => m.Name);
            }
        }

        private ProductPage ParentPage { get; }

        public ForkBranchDialogModel(ProductPage parentPage, IPalindromContext context) : base(context)
        {
            ParentPage = parentPage;
        }

        public void Submit()
        {
            IsVisible = false;

            if (!ParentPage.Repository.CurrentCommit.IsClosed)
                return;
            Branch current = ParentPage.Repository.CurrentBranch;
            var trimmedName = Name.Trim();
            if (string.IsNullOrEmpty(trimmedName))
                trimmedName = $"{current.Name}-fork";
            var branch = Branch.Create(trimmedName, current);
            ParentPage.SelectBranch(branch);

            var branchModel = new BranchModel(branch, ParentPage, Context);
            ParentPage.Branches.Add(branchModel);
            ParentPage.AddedToCollection(p => p.Branches);
        }

        public void Cancel() => IsVisible = false;
    }
}