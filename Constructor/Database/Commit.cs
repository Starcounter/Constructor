using System;
using System.Linq;
using System.Collections.Generic;
using Starcounter;
using Starcounter.Linq;

namespace Constructor.Database
{
    [Database]
    public class Commit
    {
        public string Key { get; set; }
        public Branch Branch { get; set; }
        public Commit Previous { get; set; }
        public bool IsClosed { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAtUtc { get; set; }

        public Commit(Branch branch)
        {
            this.Branch = branch ?? throw new ArgumentNullException(nameof(branch));
            this.CreatedAtUtc = DateTime.UtcNow;
            this.Key = $"{this.GetObjectNo()}";
        }

        public Commit(Branch branch, Commit previousCommit) : this(branch)
        {
            this.Previous = previousCommit ?? throw new ArgumentNullException(nameof(previousCommit));
            this.Key = $"{previousCommit.Key}-{this.GetObjectNo()}";
        }

        public IQueryable<Property> Properties => DbLinq.Objects<Property>().Where(x => x.Commit == this);
    }
}
