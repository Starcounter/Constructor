using System;
using System.Linq;
using Starcounter.Linq;
using Starcounter.Nova;

namespace Constructor.Database
{
    [Database]
    public abstract class Item
    {
        public abstract Repository Repository { get; set; }

        public static T CreateItem<T>(Repository repository) where T : Item
        {
            var instance = Db.Insert<T>();
            instance.Repository = repository ?? throw new ArgumentNullException(nameof(repository));
            instance.IsDeleted = false;
            return instance;
        }

        public string Name
        {
            get => Property.GetStringValue(Repository.CurrentCommit, this);
            set => Property.SetStringValue(Repository.CurrentCommit, this, value);
        }

        public string Description
        {
            get => Property.GetStringValue(Repository.CurrentCommit, this);
            set => Property.SetStringValue(Repository.CurrentCommit, this, value);
        }

        public string ImageUrl
        {
            get => Property.GetStringValue(Repository.CurrentCommit, this);
            set => Property.SetStringValue(Repository.CurrentCommit, this, value);
        }

        public bool IsDeleted
        {
            get => Property.GetBoolValue(Repository.CurrentCommit, this) ?? true;
            set => Property.SetBoolValue(Repository.CurrentCommit, this, value);
        }

        public int SortIndex
        {
            get => Property.GetIntValue(Repository.CurrentCommit, this) ?? 0;
            set => Property.SetIntValue(Repository.CurrentCommit, this, value);
        }

        public virtual void PreDelete()
        {
            foreach (var property in DbLinq.Objects<Property>().Where(x => x.Item == this))
            {
                Db.Delete(property);
            }
        }
    }
}