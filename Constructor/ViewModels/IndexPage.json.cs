using System;
using System.Linq;
using System.Collections.Generic;
using Starcounter;
using Starcounter.Linq;
using Constructor.Database;

namespace Constructor.ViewModels
{
    partial class IndexPage : Json
    {
        public void Init()
        {
            RefreshProducts();
        }

        public void RefreshProducts()
        {
            this.Products.Data = DbLinq.Objects<Product>()
                .OrderBy(x => x.Name)
                .ThenBy(x => x.GetObjectNo())
                .ToList();
        }

        protected void Handle(Input.CreateDefaultBicycleProductTrigger action)
        {
            action.Cancel();

            TestData data = new TestData();

            data.CreateDefaultBicycleProduct();
            RefreshProducts();
        }

        protected void Handle(Input.CreateDefaultComputerProductTrigger action)
        {
            action.Cancel();

            TestData data = new TestData();

            data.CreateDefaultComputerProduct();
            RefreshProducts();
        }

        [IndexPage_json.Products]
        partial class IndexPage_Products : Json, IBound<Product>
        {
            public IndexPage ParentPage => this.Parent.Parent as IndexPage;
            public string ObjectNoStr => this.Data?.GetObjectNo().ToString();

            protected void Handle(Input.DeleteTrigger action)
            {
                ParentPage.Products.Remove(this);

                Db.Transact(() =>
                {
                    this.Data.Repository.Delete();
                });
            }
        }

        [IndexPage_json.NewProduct]
        partial class IndexPage_NewProduct : Json
        {
            public IndexPage ParentPage => this.Parent as IndexPage;

            protected void Handle(Input.SubmitTrigger action)
            {
                if (string.IsNullOrWhiteSpace(this.Name))
                {
                    return;
                }

                string name = this.Name.Trim();

                Db.Transact(() =>
                {
                    Repository repository = new Repository(this.Name + " Repository");
                    Product product = new Product(repository)
                    {
                        Name = name
                    };
                });

                action.Cancel();
                this.VisibleTrigger = false;
                ParentPage.RefreshProducts();
            }

            protected void Handle(Input.CancelTrigger action)
            {
                action.Cancel();
                this.VisibleTrigger = false;
            }
        }

        [IndexPage_json.Products.Repository]
        partial class IndexPage_Products_Repository : Json, IBound<Repository>
        {
            protected override void OnData()
            {
                base.OnData();

                {
                    int count = this.Data?.Branches.Count() ?? 0;

                    switch (count)
                    {
                        case 0:
                            this.BranchCountStr = "No branches";
                            break;
                        case 1:
                            this.BranchCountStr = "One branch";
                            break;
                        default:
                            this.BranchCountStr = $"{count} branches";
                            break;
                    }
                }

                {
                    int count = this.Data?.Commits.Count() ?? 0;

                    switch (count)
                    {
                        case 0:
                            this.CommitCountStr = "No commits";
                            break;
                        case 1:
                            this.CommitCountStr = "One commit";
                            break;
                        default:
                            this.CommitCountStr = $"{count} commits";
                            break;
                    }
                }
            }
        }
    }
}
