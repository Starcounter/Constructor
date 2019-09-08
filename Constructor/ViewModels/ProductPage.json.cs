using System;
using System.Linq;
using System.Collections.Generic;
using Starcounter;
using Constructor.Database;
using Starcounter.Nova;

namespace Constructor.ViewModels
{
    partial class ProductPage : Json
    {
        protected Repository Repository { get; set; }

        public void Init(ulong productNo)
        {
            Init(Db.Get<Product>(productNo));
        }

        public void Init(Product product)
        {
            Product.Data = product ?? throw new ArgumentNullException(nameof(product));
            Repository = product.Repository ?? throw new ArgumentNullException(nameof(product.Repository));

            var branches = Repository.Branches
                .OrderBy(x => Db.GetOid(x.ParentBranch))
                .ThenBy(x => Db.GetOid(x))
                .ToList();
            Branch branch = Repository.CurrentBranch;

            if (branch == null)
            {
                branch = branches.Single(x => x.ParentBranch == null);
            }

            Branches.Data = branches;

            Db.Transact(() => { SelectBranch(branch); });
        }

        public bool IsEditing => !Repository.CurrentCommit.IsClosed;
        public bool IsHistory => !Commits.Last().Data.Equals(Repository.CurrentCommit);

        protected List<Commit> GetCommits(Branch branch)
        {
            var commits = branch.AllCommits.Where(x => !x.IsClosed || x.Properties.Any()).ToList();
            return commits;
        }

        protected void SelectBranch(Branch branch)
        {
            var commits = GetCommits(branch);
            Commit commit = commits.First();

            Commits.Data = commits;
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

        protected void Handle(Input.CreateCommitTrigger action)
        {
            action.Cancel();

            Db.Transact(() =>
            {
                Branch branch = Repository.CurrentBranch;
                Commit commit = branch.StartEdit();

                Commits.Data = GetCommits(branch);
                CloseCommitName = commit.Name;
            });
        }

        protected void Handle(Input.CloseCommitTrigger action)
        {
            action.Cancel();

            Db.Transact(() => { Repository.CurrentBranch.FinishEdit(CloseCommitName); });

            Commits.Data = GetCommits(Repository.CurrentBranch);
        }

        protected void Handle(Input.CancelCommitTrigger action)
        {
            action.Cancel();

            Db.Transact(() => { Repository.CurrentBranch.CancelEdit(); });

            Commits.Data = GetCommits(Repository.CurrentBranch);
        }

        protected void Handle(Input.InsertModuleTrigger action)
        {
            action.Cancel();

            Db.Transact(() =>
            {
                if (Repository.CurrentCommit.IsClosed)
                {
                    return;
                }

                Module.Create(Product.Data);
            });
        }

        [ProductPage_json.Branches]
        partial class ProductPage_Branches : Json, IBound<Branch>
        {
            public ProductPage ParentPage => Parent.Parent as ProductPage;
            public bool IsCurrent => ParentPage?.Repository?.CurrentBranch?.Equals(Data) ?? false;

            protected override void OnData()
            {
                base.OnData();
                if (Data != null)
                    ObjectNoStr = Db.GetOid(Data).ToString();
                else ObjectNoStr = null;
            }

            protected void Handle(Input.SelectTrigger action)
            {
                action.Cancel();

                Db.Transact(() => { ParentPage.SelectBranch(Data); });
            }
        }

        [ProductPage_json.Commits]
        partial class ProductPage_Commits : Json, IBound<Commit>
        {
            public ProductPage ParentPage => Parent.Parent as ProductPage;
            public bool IsCurrent => ParentPage?.Repository?.CurrentCommit?.Equals(Data) ?? false;

            protected override void OnData()
            {
                base.OnData();
                if (Data == null)
                    ObjectNoStr = null;
                else ObjectNoStr = Db.GetOid(Data).ToString();
            }

            protected void Handle(Input.SelectTrigger action)
            {
                action.Cancel();

                Db.Transact(() => { ParentPage.SelectCommit(Data); });
            }
        }

        [ProductPage_json.Product]
        partial class ProductPage_Product : Json, IBound<Product>
        {
            public long TotalAmount => Modules.Sum(x => x.TotalAmount);
        }

        [ProductPage_json.Product.Modules]
        partial class ProductPage_Product_Modules : Json, IBound<Module>
        {
            public bool IsModified
            {
                get
                {
                    if (Data == null)
                    {
                        return false;
                    }

                    var changes = Data.Repository.CurrentCommit.Properties.ToList();

                    return changes.Any(x => x.Item.Equals(Data));
                }
            }

            protected void Handle(Input.DeleteTrigger action)
            {
                Data.IsDeleted = true;
            }
        }

        [ProductPage_json.ForkBranchDialog]
        partial class ProductPage_ForkBranchDialog : Json
        {
            public ProductPage ParentPage => Parent as ProductPage;

            protected void Handle(Input.IsVisible action)
            {
                if (action.Value)
                {
                    Name = $"{ParentPage?.Repository?.CurrentBranch?.Name} - fork";
                }
            }

            protected void Handle(Input.CancelTrigger action)
            {
                action.Cancel();
                IsVisible = false;
            }

            protected void Handle(Input.SubmitTrigger action)
            {
                action.Cancel();
                IsVisible = false;

                Branch branch = null;
                ProductPage parent = ParentPage;

                Db.Transact(() =>
                {
                    if (!parent.Repository.CurrentCommit.IsClosed)
                    {
                        return;
                    }

                    Branch current = parent.Repository.CurrentBranch;
                    var name = Name.Trim();

                    if (string.IsNullOrEmpty(name))
                    {
                        name = $"{current.Name}-fork";
                    }

                    branch = Branch.Create(name, current);
                    parent.SelectBranch(branch);
                });

                parent.Branches.Add().Data = branch;
            }
        }
    }
}