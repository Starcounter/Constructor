using System;
using System.Linq;
using Starcounter.Linq;
using Starcounter.Nova;

namespace Constructor.Database
{
    [Database]
    public abstract class Commit
    {
        public abstract string Key { get; set; }
        public abstract Branch Branch { get; set; }
        public abstract Commit Previous { get; set; }
        public abstract bool IsClosed { get; set; }
        public abstract string Name { get; set; }
        public abstract DateTime CreatedAtUtc { get; set; }

        public IQueryable<Property> Properties => DbLinq.Objects<Property>().Where(x => x.Commit == this);

        public static Commit Create(Branch branch)
        {
            var instance = Db.Insert<Commit>();
            instance.Branch = branch ?? throw new ArgumentNullException(nameof(branch));
            instance.CreatedAtUtc = DateTime.UtcNow;
            instance.Key = $"{Db.GetOid(instance)}";
            return instance;
        }

        public static Commit Create(Branch branch, Commit previousCommit)
        {
            var instance = Create(branch);
            instance.Previous = previousCommit ?? throw new ArgumentNullException(nameof(previousCommit));
            instance.Key = $"{previousCommit.Key}-{Db.GetOid(instance)}";
            return instance;
        }
    }
}