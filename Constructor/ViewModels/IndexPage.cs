using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Constructor.Database;
using Starcounter.Linq;
using Starcounter.Nova;
using Starcounter.Palindrom;
using Starcounter.Palindrom.Transient;

namespace Constructor.ViewModels
{
    public class IndexPage : TransientViewModel
    {
        public string Html => "/Constructor/IndexPage.html";
        public List<ProductModel> Products { get; }
        public NewProductModel NewProduct { get; }
        public void CreateDefaultBicycleProduct() { }
        public void CreateDefaultComputerProduct() { }

        public IndexPage(IPalindromContext context) : base(context) { }

        public void Init()
        {
            RefreshProducts();
        }

        public void RefreshProducts()
        {
            var newItems = DbLinq
                .Objects<Product>()
                .OrderBy(x => x.Name)
                .ThenBy(x => Db.GetOid(x))
                .Select(p => new ProductModel(Context));
            Products.Clear();
            Products.AddRange(newItems);
        }
        
        public class ProductModel : TransientViewModel
        {
            public string ObjectNoStr { get; }
            public string Name { get; }
            public string ImageUrl { get; }
            public RepositoryModel Repository { get; }
            public void Delete() { }

            public ProductModel(IPalindromContext context) : base(context) { }

            public class RepositoryModel : TransientViewModel
            {
                public string BranchCountStr { get; }
                public string CommitCountStr { get; }

                public RepositoryModel(IPalindromContext context) : base(context) { }
            }
        }

        public class NewProductModel : TransientViewModel
        {
            public bool IsVisible { get; set; }
            public string Name { get; set; }
            public void Submit() { }
            public void Cancel() { }

            public NewProductModel(IPalindromContext context) : base(context) { }
        }
    }
}