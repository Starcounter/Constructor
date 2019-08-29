using Constructor.Database;
using Starcounter.Nova;
using Starcounter.Palindrom;
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

        public void Delete()
        {
            var index = ParentPage.Products.IndexOf(this);
            ParentPage.Products.RemoveAt(index);
            ParentPage.RemovedFromCollection(p => p.Products, index);
            var repository = Product.Repository;
            repository.PreDelete();
            Db.Delete(repository);
        }

        public IndexProductModel(Product product, IndexPage parent, IPalindromContext context) : base(context)
        {
            Product = product;
            ParentPage = parent;
            Repository = new RepositoryModel(product.Repository, Context);
        }
    }
}