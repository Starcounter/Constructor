using Constructor.Database;
using Starcounter.Nova;
using Starcounter.Palindrom;
using Starcounter.Palindrom.Database;
using Starcounter.Palindrom.Transient;

namespace Constructor.ViewModels
{
    public class BranchModel : TransientViewModel
    {
        public string ObjectNoStr { get; }
        public string Name => Branch.Name;
        public bool IsCurrent => ParentPage?.Repository?.CurrentBranch?.Equals(Branch) ?? false;

        private Branch Branch { get; }
        private ProductPage ParentPage { get; }
        private ITransactionFactory TransactionFactory { get; }

        public BranchModel(Branch branch, ProductPage parentPage, IPalindromContext context, ITransactionFactory transactionFactory) : base(context)
        {
            Branch = branch;
            ParentPage = parentPage;
            ObjectNoStr = Db.GetOid(branch).ToString();
            TransactionFactory = transactionFactory;
        }

        public void Select() => TransactionFactory.Transact(() => ParentPage.SelectBranch(Branch));
    }
}