using Constructor.Database;
using Starcounter.Palindrom;
using Starcounter.Palindrom.Database;
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
                    Name = TransactionFactory.Read(() => $"{ParentPage?.Repository?.CurrentBranch?.Name} - fork");
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
        private ITransactionFactory TransactionFactory { get; }

        public ForkBranchDialogModel(ProductPage parentPage, IPalindromContext context, ITransactionFactory transactionFactory) : base(context)
        {
            ParentPage = parentPage;
            TransactionFactory = transactionFactory;
        }

        public void Submit()
        {
            IsVisible = false;
            Branch branch = null;
            ProductPage parent = ParentPage;

            TransactionFactory.Transact(() =>
            {
                if (!parent.Repository.CurrentCommit.IsClosed)
                    return;
                Branch current = parent.Repository.CurrentBranch;
                var trimmedName = Name.Trim();
                if (string.IsNullOrEmpty(trimmedName))
                    trimmedName = $"{current.Name}-fork";
                branch = Branch.Create(trimmedName, current);
                parent.SelectBranch(branch);
            });

            var branchModel = new BranchModel(branch, ParentPage, Context, TransactionFactory);
            parent.Branches.Add(branchModel);
            parent.AddedToCollection(p => p.Branches);
        }

        public void Cancel() => IsVisible = false;
    }
}