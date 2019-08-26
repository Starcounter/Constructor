using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Constructor.Database;
using Starcounter.Nova;
using Starcounter.Palindrom;
using Starcounter.Palindrom.Transient;

namespace Constructor.ViewModels
{
    public class ProductPage : TransientViewModel
    {
        public string Html => "/Constructor/ProductPage.html";
        public List<BranchModel> Branches { get; }
        public List<CommitModel> Commits { get; }
        public ProductModel Product { get; private set; }
        public bool IsEditing => !Repository.CurrentCommit.IsClosed;
        public bool IsHistory => !this.Commits.Last().Commit.Equals(Repository.CurrentCommit);
        public void InsertModule() { }
        public void CreateCommit() { }
        public string CloseCommitName { get; set; }
        public void CloseCommit() { }
        public void CancelCommit() { }
        public void ForkBranch() { }
        public ForkBranchDialogModel ForkBranchDialog { get; }
        protected Repository Repository { get; set; }

        public ProductPage(IPalindromContext context) : base(context) { }

        public void Init(ulong productNo)
        {
            Init(Db.Get<Product>(productNo));
        }

        public void Init(Product product)
        {
            Product = new ProductModel(product, Context);
            Repository = product.Repository ?? throw new ArgumentNullException(nameof(product.Repository));

            List<Branch> branches = Repository.Branches
                .OrderBy(x => Db.GetOid(x.Parent))
                .ThenBy(x => Db.GetOid(x))
                .ToList();
            Branch branch = Repository.CurrentBranch;

            if (branch == null)
            {
                branch = branches.Single(x => x.Parent == null);
            }

            Branches.Clear();
            var convertedBranches = branches.Select(b => new BranchModel(b, Context));
            Branches.AddRange(convertedBranches);

            Db.Transact(() => { SelectBranch(branch); });
        }

        protected List<Commit> GetCommits(Branch branch)
        {
            List<Commit> commits = branch.AllCommits.Where(x => !x.IsClosed || x.Properties.Any()).ToList();
            return commits;
        }

        protected void SelectBranch(Branch branch)
        {
            List<Commit> commits = GetCommits(branch);
            Commit commit = commits.First();

            Commits.Clear();
            Commits.AddRange(commits.Select(c => new CommitModel(c, Context)));

            Repository.CurrentBranch = branch;
            Repository.CurrentCommit = commit;
        }

        protected void SelectCommit(Commit commit)
        {
            if (IsEditing)
            {
                return;
            }

            Repository.CurrentCommit = commit;
        }


        public class BranchModel : TransientViewModel
        {
            public string ObjectNoStr { get; }
            public string Name { get; }
            public bool IsCurrent { get; }
            public void Select() { }

            public BranchModel(Branch branch, IPalindromContext context) : base(context) { }
        }

        public class CommitModel : TransientViewModel
        {
            public string ObjectNoStr { get; }
            public string Name { get; }
            public bool IsCurrent { get; }
            public void Select() { }

            internal Commit Commit { get; }

            public CommitModel(Commit commit, IPalindromContext context) : base(context)
            {
                Commit = commit;
            }
        }

        public class ProductModel : TransientViewModel
        {
            public string Name { get; set; }
            public string ImageUrl { get; set; }
            public string Description { get; set; }
            public List<ModuleModel> Modules { get; }
            public long TotalAmount { get; }

            public ProductModel(Product product, IPalindromContext context) : base(context) { }

            public class ModuleModel : TransientViewModel
            {
                public string Name { get; set; }
                public string Description { get; set; }
                public long Price { get; set; }
                public long Quantity { get; set; }
                public string ImageUrl { get; set; }
                public long TotalAmount { get; }
                public void Delete() { }
                public bool IsModified { get; }

                public ModuleModel(IPalindromContext context) : base(context) { }
            }
        }

        public class ForkBranchDialogModel : TransientViewModel
        {
            public bool IsVisible { get; set; }
            public string Name { get; set; }
            public void Submit() { }
            public void Cancel() { }

            public ForkBranchDialogModel(IPalindromContext context) : base(context) { }
        }
    }
}