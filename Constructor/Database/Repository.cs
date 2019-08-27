using System;
using System.Linq;
using Starcounter.Linq;
using Starcounter.Nova;

namespace Constructor.Database
{
    [Database]
    public abstract class Repository
    {
        public abstract Branch CurrentBranch { get; set; }
        public abstract Commit CurrentCommit { get; set; }
        public abstract string Name { get; set; }

        public static Repository Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var instance = Db.Insert<Repository>();
            var branch = Branch.Create(instance);

            instance.Name = name;
            instance.CurrentBranch = branch;
            instance.CurrentCommit = branch.GetLastOwnCommit();

            return instance;
        }

        public IQueryable<Product> Products => DbLinq.Objects<Product>().Where(x => x.Repository == this);
        public IQueryable<Branch> Branches => DbLinq.Objects<Branch>().Where(x => x.Repository == this);
        public IQueryable<Commit> Commits => DbLinq.Objects<Commit>().Where(x => x.Branch.Repository == this);

        public void PreDelete()
        {
            foreach (var product in Products)
            {
                product.PreDelete();
                Db.Delete(product);
            }

            foreach (var branch in Branches.Where(x => x.ParentBranch == null))
            {
                branch.PreDelete();
                Db.Delete(branch);
            }
        }
    }
}