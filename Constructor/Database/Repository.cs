using System;
using System.Linq;
using Starcounter.Linq;
using Starcounter.Nova;

namespace Constructor.Database
{
    [Database]
    public class Repository
    {
        public Branch CurrentBranch { get; set; }
        public Commit CurrentCommit { get; set; }
        public string Name { get; set; }

        public Repository(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Branch branch = new Branch(this);

            Name = name;
            CurrentBranch = branch;
            CurrentCommit = branch.GetLastOwnCommit();
        }

        public IQueryable<Product> Products => DbLinq.Objects<Product>().Where(x => x.Repository == this);

        public IQueryable<Branch> Branches => DbLinq.Objects<Branch>().Where(x => x.Repository == this);

        public IQueryable<Commit> Commits => DbLinq.Objects<Commit>().Where(x => x.Branch.Repository == this);

        public void OnDelete()
        {
            foreach (Product product in Products.ToList())
            {
                Db.Delete(product);
            }

            foreach (Branch branch in Branches.Where(x => x.Parent == null).ToList())
            {
                Db.Delete(branch);
            }
        }
    }
}
