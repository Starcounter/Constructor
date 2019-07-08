using System;
using System.Linq;
using System.Collections.Generic;
using Starcounter;
using Starcounter.Linq;

namespace Constructor.Database
{
    [Database]
    public class Repository : IEntity
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

            this.Name = name;
            this.CurrentBranch = branch;
            this.CurrentCommit = branch.GetLastOwnCommit();
        }

        public IQueryable<Product> Products => DbLinq.Objects<Product>().Where(x => x.Repository == this);

        public IQueryable<Branch> Branches => DbLinq.Objects<Branch>().Where(x => x.Repository == this);

        public IQueryable<Commit> Commits => DbLinq.Objects<Commit>().Where(x => x.Branch.Repository == this);

        public void OnDelete()
        {
            foreach (Product product in this.Products.ToList())
            {
                product.Delete();
            }

            foreach (Branch branch in this.Branches.Where(x => x.Parent == null).ToList())
            {
                branch.Delete();
            }
        }
    }
}
