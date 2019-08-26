using System;
using System.Linq;
using System.Collections.Generic;
using Starcounter;
using Starcounter.Linq;
using Starcounter.Nova;

namespace Constructor.Database
{
    [Database]
    public abstract class Item
    {
        public Repository Repository { get; set; }

        public Item(Repository repository)
        {
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
            IsDeleted = false;
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

        public virtual void OnDelete()
        {
            foreach (Property property in DbLinq.Objects<Property>().Where(x => x.Item == this).ToList())
            {
                Db.Delete(property);
            }
        }
    }
}
