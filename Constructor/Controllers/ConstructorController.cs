using Constructor.Database;
using Constructor.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Starcounter.Nova;
using Starcounter.Palindrom;
using Starcounter.Palindrom.AspNetCore;
using Starcounter.Palindrom.Database;

namespace Constructor.Controllers
{
    public class ConstructorController : Controller
    {
        private IPalindromContext PalindromContext { get; }
        private ITransactionFactory TransactionFactory { get; }

        public ConstructorController(IPalindromContext palindromContext, ITransactionFactory transactionFactory)
        {
            PalindromContext = palindromContext;
            TransactionFactory = transactionFactory;
        }

        [Palindrom, Route("/constructor")]
        public PalindromResult IndexPage()
        {
            return new IndexPage(PalindromContext, TransactionFactory);
        }

        [Palindrom, Route("/constructor/product/{id}")]
        public PalindromResult ProductPage(ulong id)
        {
            var product = TransactionFactory.Read(() => Db.Get<Product>(id));
            return new ProductPage(product, PalindromContext, TransactionFactory);
        }
    }
}