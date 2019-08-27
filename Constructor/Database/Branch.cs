using System;
using System.Linq;
using System.Collections.Generic;
using Starcounter.Linq;
using Starcounter.Nova;

namespace Constructor.Database
{
    [Database]
    public abstract class Branch
    {
        #region Constants

        public const string MasterBranchName = "master";
        public const string InitialCommitName = "Initial commit";
        public const string ForkCommitName = "Fork commit";

        #endregion

        public abstract Repository Repository { get; set; }
        public abstract Branch ParentBranch { get; set; }
        public abstract string Key { get; set; }
        public abstract string Name { get; set; }

        public IQueryable<Commit> OwnCommits => DbLinq.Objects<Commit>().Where(x => x.Branch == this);

        /// <summary>
        /// Returns all commits from parent and current branches.
        /// Sorted by <see cref="Commit.CreatedAtUtc"/> descending.
        /// </summary>
        public IEnumerable<Commit> AllCommits
        {
            get
            {
                Commit last = GetLastOwnCommit();
                const string sql = "SELECT c FROM Constructor.Database.Commit c WHERE ? STARTS WITH c.Key ORDER BY c.CreatedAtUtc DESC, ObjectNo DESC";
                return Db.SQL<Commit>(sql, last.Key);
            }
        }

        public static Branch Create(Repository repository)
        {
            var instance = Db.Insert<Branch>();
            instance.Repository = repository ?? throw new ArgumentNullException(nameof(repository));
            instance.Key = Db.GetOid(instance).ToString();
            instance.Name = MasterBranchName;

            var commit = Commit.Create(instance);
            commit.Name = InitialCommitName;
            commit.IsClosed = true;

            return instance;
        }

        public static Branch Create(string name, Branch branch)
        {
            var instance = Db.Insert<Branch>();
            instance.ParentBranch = branch ?? throw new ArgumentNullException(nameof(branch));
            instance.Repository = branch.Repository ?? throw new ArgumentNullException(nameof(branch.Repository));
            instance.Key = $"{branch.Key}-{Db.GetOid(instance)}";
            instance.Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentNullException(nameof(name));

            var commit = Commit.Create(instance, branch.GetLastOwnCommit());
            commit.Name = ForkCommitName;
            commit.IsClosed = true;

            return instance;
        }

        public Commit GetLastOwnCommit()
        {
            const string sql = "SELECT c FROM Constructor.Database.Commit c WHERE c.Branch = ? ORDER BY c.CreatedAtUtc DESC, ObjectNo DESC";
            return Db.SQL<Commit>(sql, this).First();
        }

        public Commit StartEdit()
        {
            Commit last = GetLastOwnCommit();

            if (!last.IsClosed)
            {
                return last;
            }

            Commit commit = Commit.Create(this, last);
            commit.Name = $"Edit - {DateTime.Now}";

            Repository.CurrentCommit = commit;

            return commit;
        }

        public Commit FinishEdit(string name = null)
        {
            Commit last = GetLastOwnCommit();

            if (last.IsClosed)
            {
                return last;
            }

            if (!last.Properties.Any())
            {
                Db.Delete(last);
                last = GetLastOwnCommit();
                Repository.CurrentCommit = last;
                return last;
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                last.Name = name;
            }

            last.IsClosed = true;

            return last;
        }

        public Commit CancelEdit()
        {
            Commit last = GetLastOwnCommit();

            if (last.IsClosed)
            {
                return last;
            }

            Db.Delete(last);
            last = GetLastOwnCommit();

            Repository.CurrentCommit = last;

            return last;
        }

        public void PreDelete()
        {
            foreach (var branch in DbLinq.Objects<Branch>().Where(x => x.ParentBranch == this))
            {
                branch.PreDelete();
                Db.Delete(branch);
            }

            foreach (var commit in OwnCommits)
            {
                Db.Delete(commit);
            }
        }
    }
}