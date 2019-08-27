using Constructor.Database;
using Starcounter.Nova;
using Starcounter.Palindrom;
using Starcounter.Palindrom.Database;
using Starcounter.Palindrom.Transient;

namespace Constructor.ViewModels
{
    public class IndexProductModel : TransientViewModel
    {
        public string ObjectNoStr => Db.GetOid(Product).ToString();
        public string Name => Product.Name;
        public string ImageUrl => Product.ImageUrl;
        public RepositoryModel Repository { get; }

        private Product Product { get; }
        private IndexPage ParentPage { get; }
        private ITransactionFactory TransactionFactory { get; }

        public void Delete()
        {
            var index = ParentPage.Products.IndexOf(this);
            ParentPage.Products.RemoveAt(index);
            ParentPage.RemovedFromCollection(p => p.Products, index);
            TransactionFactory.Transact(() =>
            {
                Product.Repository.PreDelete();
                Db.Delete(Product.Repository);
            });
        }

        public IndexProductModel(Product product, IndexPage parent, IPalindromContext context, ITransactionFactory transactionFactory) : base(context)
        {
            Product = product;
            ParentPage = parent;
            Repository = new RepositoryModel(product.Repository, Context);
            TransactionFactory = transactionFactory;
        }
    }
}