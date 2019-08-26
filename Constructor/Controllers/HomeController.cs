using Constructor.ViewModels;
using Microsoft.AspNetCore.Mvc;
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
            var page = new IndexPage(PalindromContext);
            page.Init();
            return page;
        }

        [Palindrom, Route("/product/{id}")]
        public PalindromResult Product(ulong id)
        {
            var page = new ProductPage(PalindromContext);
            page.Init(id);
            return page;
        }
    }
}