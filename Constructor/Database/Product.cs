using System.Linq;
using Starcounter.Linq;
using Starcounter.Nova;

namespace Constructor.Database
{
    [Database]
    public abstract class Product : Item
    {
        public static Product Create(Repository repository)
        {
            return CreateItem<Product>(repository);
        }

        public IQueryable<Module> Modules => DbLinq.Objects<Module>()
            .Where(x => x.Product == this && !x.IsDeleted)
            .OrderBy(x => x.SortIndex);

        public override void OnDelete()
        {
            foreach (Module module in DbLinq.Objects<Module>().Where(x => x.Product == this).ToList())
            {
                Db.Delete(module);
            }

            base.OnDelete();
        }
    }
}
