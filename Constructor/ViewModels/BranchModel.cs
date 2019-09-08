using Constructor.Database;
using Starcounter.Nova;
using Starcounter.Palindrom;
using Starcounter.Palindrom.Transient;

namespace Constructor.ViewModels
{
    public class BranchModel : TransientViewModel
    {
        public string ObjectNoStr { get; }
        public string Name => Branch.Name;

        [Member(unmonitored: true)]
        public bool IsCurrent => ParentPage?.Repository?.CurrentBranch?.Equals(Branch) ?? false;

        internal Branch Branch { get; }
        private ProductPage ParentPage { get; }

        public BranchModel(Branch branch, ProductPage parentPage, IPalindromContext context) : base(context)
        {
            Branch = branch;
            ParentPage = parentPage;
            ObjectNoStr = Db.GetOid(branch).ToString();
        }

        public void Select() => ParentPage.SelectBranch(Branch);
    }
}