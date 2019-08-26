using System;
using System.Linq;
using System.Collections.Generic;
using Starcounter;
using Starcounter.Linq;
using Starcounter.Nova;

namespace Constructor.Database
{
    [Database]
    public class Branch
    {
        #region Constants

        public const string MasterBranchName = "master";
        public const string InitialCommitName = "Initial commit";
        public const string ForkCommitName = "Fork commit";

        #endregion

        public Repository Repository { get; set; }
        public Branch Parent { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }

        public Branch(Repository repository)
        {
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
            Key = Db.GetOid(this).ToString();
            Name = MasterBranchName;

            new Commit(this)
            {
                Name = InitialCommitName,
                IsClosed = true
            };
        }

        public Branch(string name, Branch branch)
        {
            Parent = branch ?? throw new ArgumentNullException(nameof(branch));
            Repository = branch?.Repository ?? throw new ArgumentNullException(nameof(branch.Repository));
            Key = $"{branch.Key}-{Db.GetOid(this)}";
            Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentNullException(nameof(name));

            new Commit(this, branch.GetLastOwnCommit())
            {
                Name = ForkCommitName,
                IsClosed = true
            };
        }

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
                return Db.SQL<Commit>(
                    "SELECT c FROM Constructor.Database.Commit c WHERE ? STARTS WITH c.Key ORDER BY c.CreatedAtUtc DESC, ObjectNo DESC", last.Key);
            }
        }

        public Commit GetLastOwnCommit()
        {
            Commit last = Db.SQL<Commit>("SELECT c FROM Constructor.Database.Commit c WHERE c.Branch = ? ORDER BY c.CreatedAtUtc DESC, ObjectNo DESC",
                this).First();
            return last;
        }

        public Commit StartEdit()
        {
            Commit last = GetLastOwnCommit();

            if (!last.IsClosed)
            {
                return last;
            }

            Commit commit = new Commit(this, last)
            {
                Name = $"Edit - {DateTime.Now}"
            };

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

        public void OnDelete()
        {
            foreach (Branch branch in DbLinq.Objects<Branch>().Where(x => x.Parent == this).ToList())
            {
                Db.Delete(branch);
            }

            foreach (Commit commit in OwnCommits.ToList())
            {
                Db.Delete(commit);
            }
        }
    }
}