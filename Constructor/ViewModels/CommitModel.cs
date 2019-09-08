using Constructor.Database;
using Starcounter.Nova;
using Starcounter.Palindrom;
using Starcounter.Palindrom.Transient;

namespace Constructor.ViewModels
{
    public class CommitModel : TransientViewModel
    {
        public string ObjectNoStr { get; }
        public string Name => Commit.Name;

        [Member(unmonitored: true)]
        public bool IsCurrent => ParentPage?.Repository?.CurrentCommit?.Equals(Commit) ?? false;

        internal Commit Commit { get; }
        private ProductPage ParentPage { get; }

        public CommitModel(Commit commit, ProductPage parentPage, IPalindromContext context) : base(context)
        {
            Commit = commit;
            ParentPage = parentPage;
            ObjectNoStr = Db.GetOid(commit).ToString();
        }

        public void Select() => ParentPage.SelectCommit(Commit);
    }
}