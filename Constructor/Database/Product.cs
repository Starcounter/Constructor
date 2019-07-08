using System;
using System.Linq;
using System.Collections.Generic;
using Starcounter;
using Starcounter.Linq;

namespace Constructor.Database
{
    [Database]
    public class Product : Item, IEntity
    {
        public Product(Repository repository) : base(repository)
        {
        }

        public IQueryable<Module> Modules => DbLinq.Objects<Module>()
            .Where(x => x.Product == this && !x.IsDeleted)
            .OrderBy(x => x.SortIndex);

        public override void OnDelete()
        {
            foreach (Module module in DbLinq.Objects<Module>().Where(x => x.Product == this).ToList())
            {
                module.Delete();
            }

            base.OnDelete();
        }
    }
}
