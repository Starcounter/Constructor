using System.Linq;
using Constructor.Database;
using Starcounter.Palindrom;
using Starcounter.Palindrom.Transient;

namespace Constructor.ViewModels
{
    public class RepositoryModel : TransientViewModel
    {
        public string BranchCountStr { get; }
        public string CommitCountStr { get; }

        public RepositoryModel(Repository repository, IPalindromContext context) : base(context)
        {
            BranchCountStr = repository?.Branches.Count() switch
            {
                null => "No branches",
                0 => "No branches",
                1 => "One branch",
                var multiple => $"{multiple} branches"
            };
            CommitCountStr = repository?.Commits.Count() switch
            {
                null => "No commits",
                0 => "No commits",
                1 => "One commit",
                var multiple => $"{multiple} commits"
            };
        }
    }
}