using System;
using System.Linq;
using Starcounter.Linq;
using Starcounter.Nova;

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
            Branch = branch ?? throw new ArgumentNullException(nameof(branch));
            CreatedAtUtc = DateTime.UtcNow;
            Key = $"{Db.GetOid(this)}";
        }

        public Commit(Branch branch, Commit previousCommit) : this(branch)
        {
            Previous = previousCommit ?? throw new ArgumentNullException(nameof(previousCommit));
            Key = $"{previousCommit.Key}-{Db.GetOid(this)}";
        }

        public IQueryable<Property> Properties => DbLinq.Objects<Property>().Where(x => x.Commit == this);
    }
}
