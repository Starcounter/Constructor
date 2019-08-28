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

        public ConstructorController(IPalindromContext palindromContext, IStarcounterInteractionContext interactionContext)
        {
            PalindromContext = palindromContext;

            // This way we tell the Palindrom context to use the Starcounter interaction context.
            // We can also set the default interaction context (see Startup), in which case we 
            // don't need this.
            PalindromContext.InteractionContext = interactionContext;
        }

        [Palindrom, Route("/constructor")]
        public PalindromResult IndexPage()
        {
            return new IndexPage(PalindromContext);
        }

        [Palindrom, Route("/constructor/product/{id}")]
        public PalindromResult ProductPage(ulong id)
        {
            var product = Db.Get<Product>(id);
            return new ProductPage(product, PalindromContext);
        }
    }
}