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
            Branches = new List<BranchModel>();
            Commits = new List<CommitModel>();
            Product = new ProductModel(product, Context);
            Repository = product.Repository ?? throw new ArgumentNullException(nameof(product.Repository));

            var branches = Repository.Branches
                .OrderBy(b => Db.GetOid(b.ParentBranch))
                .ThenBy(b => Db.GetOid(b))
                .ToList();
            var branch = Repository.CurrentBranch ?? branches.Single(x => x.ParentBranch == null);
            ReplaceBranches(branches);
            SelectBranch(branch);

            ForkBranchDialog = new ForkBranchDialogModel(this, Context);
        }

        public void InsertModule()
        {
            if (Repository.CurrentCommit.IsClosed)
                return;
            Module.Create(Product.Product);
        }

        public void CreateCommit()
        {
            Branch branch = Repository.CurrentBranch;
            Commit commit = branch.StartEdit();
            var commits = GetCommits(branch);
            ReplaceCommits(commits);
            CloseCommitName = commit.Name;
        }

        public void CloseCommit()
        {
            Repository.CurrentBranch.FinishEdit(CloseCommitName);
            var commits = GetCommits(Repository.CurrentBranch);
            ReplaceCommits(commits);
        }

        public void CancelCommit()
        {
            Repository.CurrentBranch.CancelEdit();
            var commits = GetCommits(Repository.CurrentBranch);
            ReplaceCommits(commits);
        }

        public void ForkBranch()
        {
            // do nothing?
        }

        private void ReplaceBranches(IEnumerable<Branch> branches)
        {
            Branches.Clear();
            var convertedBranches = branches.Select(b => new BranchModel(b, this, Context));
            Branches.AddRange(convertedBranches);
            this.MemberChanged(p => p.Branches);
        }

        private void ReplaceCommits(IEnumerable<Commit> commits)
        {
            Commits.Clear();
            var newCommits = commits.Select(c => new CommitModel(c, this, Context));
            Commits.AddRange(newCommits);
            this.MemberChanged(p => p.Commits);
        }

        private IEnumerable<Commit> GetCommits(Branch branch)
        {
            return branch.AllCommits.Where(x => !x.IsClosed || x.Properties.Any());
        }

        internal void SelectBranch(Branch branch)
        {
            var commits = GetCommits(branch).ToList();
            var commit = commits[0];
            ReplaceCommits(commits);
            Repository.CurrentBranch = branch;
            Repository.CurrentCommit = commit;
        }

        internal void SelectCommit(Commit commit)
        {
            if (IsEditing) return;
            Repository.CurrentCommit = commit;
        }
    }
}