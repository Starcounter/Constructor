using System.Collections.Generic;
using System.Linq;
using Constructor.Database;
using Starcounter.Linq;
using Starcounter.Nova;
using Starcounter.Palindrom;
using Starcounter.Palindrom.Database;
using Starcounter.Palindrom.Transient;

namespace Constructor.ViewModels
{
    public class IndexPage : TransientViewModel
    {
        private ITransactionFactory TransactionFactory { get; }

        public List<IndexProductModel> Products { get; }
        public IndexNewProductModel NewProduct { get; }
        public string Html => "/Constructor/IndexPage.html";

        public IndexPage(IPalindromContext context, ITransactionFactory transactionFactory) : base(context)
        {
            TransactionFactory = transactionFactory;
            Products = new List<IndexProductModel>();
            NewProduct = new IndexNewProductModel(this, Context, TransactionFactory);
            RefreshProducts();
        }

        public void CreateDefaultBicycleProduct()
        {
            var data = new TestData(TransactionFactory);
            data.CreateDefaultBicycleProduct();
            RefreshProducts();
        }

        public void CreateDefaultComputerProduct()
        {
            var data = new TestData(TransactionFactory);
            data.CreateDefaultComputerProduct();
            RefreshProducts();
        }

        internal void RefreshProducts() => TransactionFactory.Read(() =>
        {
            var newItems = DbLinq
                .Objects<Product>()
                .OrderBy(x => x.Name)
                .ThenBy(x => Db.GetOid(x))
                .AsEnumerable()
                .Select(p => new IndexProductModel(p, this, Context, TransactionFactory));
            Products.Clear();
            Products.AddRange(newItems);
            this.MemberChanged(i => i.Products);
        });
    }
}