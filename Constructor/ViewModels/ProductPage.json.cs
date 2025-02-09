using System;
using System.Linq;
using System.Collections.Generic;
using Starcounter;
using Starcounter.Linq;
using Constructor.Database;

namespace Constructor.ViewModels
{
    partial class ProductPage : Json
    {
        protected Repository Repository { get; set; }

        public void Init(ulong productNo)
        {
            this.Init(Db.FromId<Product>(productNo));
        }

        public void Init(Product product)
        {
            this.Product.Data = product ?? throw new ArgumentNullException(nameof(product));
            this.Repository = product.Repository ?? throw new ArgumentNullException(nameof(product.Repository));

            List<Branch> branches = this.Repository.Branches
                .OrderBy(x => x.Parent.GetObjectNo())
                .ThenBy(x => x.GetObjectNo())
                .ToList();
            Branch branch = this.Repository.CurrentBranch;

            if (branch == null)
            {
                branch = branches.Single(x => x.Parent == null);
            }

            this.Branches.Data = branches;

            Db.Transact(() =>
            {
                this.SelectBranch(branch);
            });
        }

        public bool IsEditing => !this.Repository.CurrentCommit.IsClosed;
        public bool IsHistory => !this.Commits.Last().Data.Equals(this.Repository.CurrentCommit);

        protected List<Commit> GetCommits(Branch branch)
        {
            List<Commit> commits = branch.AllCommits.Where(x => !x.IsClosed || x.Properties.Any()).ToList();
            return commits;
        }

        protected void SelectBranch(Branch branch)
        {
            List<Commit> commits = this.GetCommits(branch);
            Commit commit = commits.First();

            this.Commits.Data = commits;
            this.Repository.CurrentBranch = branch;
            this.Repository.CurrentCommit = commit;
        }

        protected void SelectCommit(Commit commit)
        {
            if (this.IsEditing)
            {
                return;
            }

            this.Repository.CurrentCommit = commit;
        }

        protected void Handle(Input.CreateCommitTrigger action)
        {
            action.Cancel();

            Db.Transact(() =>
            {
                Branch branch = this.Repository.CurrentBranch;
                Commit commit = branch.StartEdit();
                
                this.Commits.Data = this.GetCommits(branch);
                this.CloseCommitName = commit.Name;
            });
        }

        protected void Handle(Input.CloseCommitTrigger action)
        {
            action.Cancel();

            Db.Transact(() =>
            {
                this.Repository.CurrentBranch.FinishEdit(this.CloseCommitName);
            });

            this.Commits.Data = this.GetCommits(this.Repository.CurrentBranch);
        }

        protected void Handle(Input.CancelCommitTrigger action)
        {
            action.Cancel();

            Db.Transact(() =>
            {
                this.Repository.CurrentBranch.CancelEdit();
            });

            this.Commits.Data = this.GetCommits(this.Repository.CurrentBranch);
        }

        protected void Handle(Input.InsertModuleTrigger action)
        {
            action.Cancel();

            Db.Transact(() =>
            {
                if (this.Repository.CurrentCommit.IsClosed)
                {
                    return;
                }

                new Module(this.Product.Data);
            });
        }

        [ProductPage_json.Branches]
        partial class ProductPage_Branches : Json, IBound<Branch>
        {
            public ProductPage ParentPage => this.Parent.Parent as ProductPage;
            public bool IsCurrent => this.ParentPage?.Repository?.CurrentBranch?.Equals(this.Data) ?? false;

            protected override void OnData()
            {
                base.OnData();
                this.ObjectNoStr = this.Data?.GetObjectNo().ToString();
            }

            protected void Handle(Input.SelectTrigger action)
            {
                action.Cancel();

                Db.Transact(() =>
                {
                    this.ParentPage.SelectBranch(this.Data);
                });
            }
        }

        [ProductPage_json.Commits]
        partial class ProductPage_Commits : Json, IBound<Commit>
        {
            public ProductPage ParentPage => this.Parent.Parent as ProductPage;
            public bool IsCurrent => this.ParentPage?.Repository?.CurrentCommit?.Equals(this.Data) ?? false;

            protected override void OnData()
            {
                base.OnData();
                this.ObjectNoStr = this.Data?.GetObjectNo().ToString();
            }

            protected void Handle(Input.SelectTrigger action)
            {
                action.Cancel();

                Db.Transact(() =>
                {
                    this.ParentPage.SelectCommit(this.Data);
                });
            }
        }

        [ProductPage_json.Product]
        partial class ProductPage_Product : Json, IBound<Product>
        {
            public long TotalAmount => this.Modules.Sum(x => x.TotalAmount);
        }

        [ProductPage_json.Product.Modules]
        partial class ProductPage_Product_Modules : Json, IBound<Module>
        {
            public bool IsModified
            {
                get
                {
                    if (this.Data == null)
                    {
                        return false;
                    }

                    List<Property> changes = this.Data.Repository.CurrentCommit.Properties.ToList();

                    return changes.Any(x => x.Item.Equals(this.Data));
                }
            }

            protected void Handle(Input.DeleteTrigger action)
            {
                this.Data.IsDeleted = true;
            }
        }

        [ProductPage_json.ForkBranchDialog]
        partial class ProductPage_ForkBranchDialog : Json
        {
            public ProductPage ParentPage => this.Parent as ProductPage;

            protected void Handle(Input.IsVisible action)
            {
                if (action.Value)
                {
                    this.Name = $"{this.ParentPage?.Repository?.CurrentBranch?.Name} - fork";
                }
            }

            protected void Handle(Input.CancelTrigger action)
            {
                action.Cancel();
                this.IsVisible = false;
            }

            protected void Handle(Input.SubmitTrigger action)
            {
                action.Cancel();
                this.IsVisible = false;

                Branch branch = null;
                ProductPage parent = this.ParentPage;

                Db.Transact(() =>
                {
                    if (!parent.Repository.CurrentCommit.IsClosed)
                    {
                        return;
                    }

                    Branch current = parent.Repository.CurrentBranch;
                    string name = this.Name.Trim();

                    if (string.IsNullOrEmpty(name))
                    {
                        name = $"{current.Name}-fork";
                    }

                    branch = new Branch(name, current);
                    parent.SelectBranch(branch);
                });

                parent.Branches.Add().Data = branch;
            }
        }
    }
}
