using Constructor.Database;
using Constructor.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Starcounter.Nova;
using Starcounter.Palindrom;
using Starcounter.Palindrom.AspNetCore;

namespace Constructor.Controllers
{
    public class ConstructorController : Controller
    {
        private IPalindromContext PalindromContext { get; }

        public ConstructorController(IPalindromContext palindromContext)
        {
            PalindromContext = palindromContext;
        }

        [Palindrom]
        public PalindromResult Index()
        {
            return new IndexPage(PalindromContext);
        }

        [Palindrom, Route("/product/{id}")]
        public PalindromResult Product(ulong id)
        {
            var product = Db.Get<Product>(id);
            return new ProductPage(product, PalindromContext);
        }
    }
}