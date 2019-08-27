using System.Collections.Generic;
using System.Linq;
using Constructor.Database;
using Starcounter.Linq;
using Starcounter.Nova;
using Starcounter.Palindrom;
using Starcounter.Palindrom.Transient;

namespace Constructor.ViewModels
{
    public class IndexPage : TransientViewModel
    {
        public List<IndexProductModel> Products { get; }
        public IndexNewProductModel NewProduct { get; }

        public string Html => "/Constructor/IndexPage.html";

        public IndexPage(IPalindromContext context) : base(context)
        {
            Products = new List<IndexProductModel>();
            NewProduct = new IndexNewProductModel(this, Context);
            RefreshProducts();
        }

        public void CreateDefaultBicycleProduct()
        {
            var data = new TestData();
            data.CreateDefaultBicycleProduct();
            RefreshProducts();
        }

        public void CreateDefaultComputerProduct()
        {
            var data = new TestData();
            data.CreateDefaultComputerProduct();
            RefreshProducts();
        }

        internal void RefreshProducts()
        {
            var newItems = DbLinq
                .Objects<Product>()
                .OrderBy(x => x.Name)
                .ThenBy(x => Db.GetOid(x))
                .Select(p => new IndexProductModel(p, this, Context));
            Products.Clear();
            Products.AddRange(newItems);
            this.MemberChanged(i => i.Products);
        }
    }
}