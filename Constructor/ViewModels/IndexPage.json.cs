using System.Linq;
using Starcounter;
using Starcounter.Linq;
using Constructor.Database;
using Starcounter.Nova;

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
            Products.Data = DbLinq.Objects<Product>()
                .OrderBy(x => x.Name)
                .ThenBy(x => Db.GetOid(x))
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
            public IndexPage ParentPage => Parent.Parent as IndexPage;
            public string ObjectNoStr => Data != null ? Db.GetOid(Data).ToString() : null;

            protected void Handle(Input.DeleteTrigger action)
            {
                ParentPage.Products.Remove(this);

                Db.Transact(() =>
                {
                    var repository = Data.Repository;
                    repository.PreDelete();
                    Db.Delete(repository);
                }, new TransactOptions(() => { }));
            }
        }

        [IndexPage_json.NewProduct]
        partial class IndexPage_NewProduct : Json
        {
            public IndexPage ParentPage => Parent as IndexPage;

            protected void Handle(Input.SubmitTrigger action)
            {
                if (string.IsNullOrWhiteSpace(Name))
                {
                    return;
                }

                var name = Name.Trim();

                Db.Transact(() =>
                {
                    Repository repository = Repository.Create(Name + " Repository");
                    Product product = Product.Create(repository);
                    product.Name = name;
                }, new TransactOptions(() => { }));

                action.Cancel();
                VisibleTrigger = false;
                ParentPage.RefreshProducts();
            }

            protected void Handle(Input.CancelTrigger action)
            {
                action.Cancel();
                VisibleTrigger = false;
            }
        }

        [IndexPage_json.Products.Repository]
        partial class IndexPage_Products_Repository : Json, IBound<Repository>
        {
            protected override void OnData()
            {
                base.OnData();

                {
                    var count = Data?.Branches.Count() ?? 0;

                    switch (count)
                    {
                        case 0:
                            BranchCountStr = "No branches";
                            break;
                        case 1:
                            BranchCountStr = "One branch";
                            break;
                        default:
                            BranchCountStr = $"{count} branches";
                            break;
                    }
                }

                {
                    var count = Data?.Commits.Count() ?? 0;

                    switch (count)
                    {
                        case 0:
                            CommitCountStr = "No commits";
                            break;
                        case 1:
                            CommitCountStr = "One commit";
                            break;
                        default:
                            CommitCountStr = $"{count} commits";
                            break;
                    }
                }
            }
        }
    }
}