using System;
using System.Collections.Generic;
using System.Linq;
using Constructor.Database;
using Starcounter.Nova;
using Starcounter.Palindrom;
using Starcounter.Palindrom.Transient;

namespace Constructor.ViewModels
{
    public class ProductPage : TransientViewModel
    {
        private string closeCommitName;

        public List<BranchModel> Branches { get; }
        public List<CommitModel> Commits { get; }
        public ProductModel Product { get; }

        [Member(unmonitored: true)]
        public bool IsEditing => !Repository.CurrentCommit.IsClosed;

        [Member(unmonitored: true)]
        public bool IsHistory => !Commits.Last().Commit.Equals(Repository.CurrentCommit);

        public ForkBranchDialogModel ForkBranchDialog { get; }
        public string Html => "/Constructor/ProductPage.html";

        public string CloseCommitName
        {
            get => closeCommitName;
            set
            {
                closeCommitName = value;
                this.MemberChanged(p => p.CloseCommitName);
            }
        }

        internal Repository Repository { get; }

        public ProductPage(Product product, IPalindromContext context) : base(context)
        {
            Product = new ProductModel(product, Context);
            Repository = product.Repository ?? throw new ArgumentNullException(nameof(product.Repository));

            var branches = Repository.Branches
                .OrderBy(b => Db.GetOid(b.ParentBranch))
                .ThenBy(b => Db.GetOid(b))
                .ToList();

            var branch = Repository.CurrentBranch ?? branches.Single(x => x.ParentBranch == null);

            Branches = branches
                .Select(b => new BranchModel(b, this, Context))
                .ToList();

            var commits = GetCommits(branch).ToList();
            var commit = commits[0];

            Commits = commits
                .Select(c => new CommitModel(c, this, Context))
                .ToList();

            Repository.CurrentBranch = branch;
            Repository.CurrentCommit = commit;

            ForkBranchDialog = new ForkBranchDialogModel(this, Context);
        }

        public void InsertModule()
        {
            if (Repository.CurrentCommit.IsClosed)
                return;
            var createdModule = Module.Create(Product.Product);
            Product.AddModule(createdModule);
        }

        public void CreateCommit()
        {
            Branch branch = Repository.CurrentBranch;
            Commit commit = branch.StartEdit();
            var commits = GetCommits(branch);
            PatchCommits(commits);
            CloseCommitName = commit.Name;
        }

        public void CloseCommit()
        {
            Repository.CurrentBranch.FinishEdit(CloseCommitName);
            var commits = GetCommits(Repository.CurrentBranch);
            PatchCommits(commits);
        }

        public void CancelCommit()
        {
            Repository.CurrentBranch.CancelEdit();
            var commits = GetCommits(Repository.CurrentBranch);
            PatchCommits(commits);
        }

        public void ForkBranch()
        {
            throw new NotImplementedException();
        }

        private void PatchCommits(IEnumerable<Commit> commits)
        {
            this.PatchViewModelCollection
            (
                collectionSelector: page => page.Commits,
                updatedEnumeration: commits,
                equalityItemSelector: model => model.Commit,
                listItemFactory: commit => new CommitModel(commit, this, Context)
            );
        }

        private IEnumerable<Commit> GetCommits(Branch branch)
        {
            return branch.AllCommits.Where(x => !x.IsClosed || x.Properties.Any());
        }

        internal void SelectBranch(Branch branch)
        {
            var commits = GetCommits(branch).ToList();
            var commit = commits[0];
            PatchCommits(commits);
            Repository.CurrentBranch = branch;
            Repository.CurrentCommit = commit;
        }

        internal void SelectCommit(Commit commit)
        {
            if (IsEditing) return;
            Repository.CurrentCommit = commit;
            Product.ReloadModules();
        }
    }
}