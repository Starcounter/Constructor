using Constructor.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Starcounter.Nova.Scopes;
using Starcounter.XSON.Palindrom;

namespace Constructor.Controllers
{
    public class ConstructorController : Controller
    {
        [XSON, Route("/constructor")]
        public XSONResult IndexPage() => Scope.CreateScope().Run(() =>
        {
            var page = new IndexPage();
            page.Init();
            return page;
        });

        [XSON, Route("/constructor/product/{id}")]
        public XSONResult ProductPage(ulong id) => Scope.CreateScope().Run(() =>
        {
            var productPage = new ProductPage();
            productPage.Init(id);
            return productPage;
        });
    }
}