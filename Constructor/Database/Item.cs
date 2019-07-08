using System;
using System.Linq;
using System.Collections.Generic;
using Starcounter;
using Starcounter.Linq;

namespace Constructor.Database
{
    [Database]
    public abstract class Item : IEntity
    {
        public Repository Repository { get; set; }

        public Item(Repository repository)
        {
            this.Repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.IsDeleted = false;
        }

        public string Name
        {
            get
            {
                return Property.GetStringValue(this.Repository.CurrentCommit, this);
            }
            set
            {
                Property.SetStringValue(this.Repository.CurrentCommit, this, value);
            }
        }

        public string Description
        {
            get
            {
                return Property.GetStringValue(this.Repository.CurrentCommit, this);
            }
            set
            {
                Property.SetStringValue(this.Repository.CurrentCommit, this, value);
            }
        }

        public string ImageUrl
        {
            get
            {
                return Property.GetStringValue(this.Repository.CurrentCommit, this);
            }
            set
            {
                Property.SetStringValue(this.Repository.CurrentCommit, this, value);
            }
        }

        public bool IsDeleted
        {
            get
            {
                return Property.GetBoolValue(this.Repository.CurrentCommit, this) ?? true;
            }
            set
            {
                Property.SetBoolValue(this.Repository.CurrentCommit, this, value);
            }
        }

        public int SortIndex
        {
            get
            {
                return Property.GetIntValue(this.Repository.CurrentCommit, this) ?? 0;
            }
            set
            {
                Property.SetIntValue(this.Repository.CurrentCommit, this, value);
            }
        }

        public virtual void OnDelete()
        {
            foreach (Property property in DbLinq.Objects<Property>().Where(x => x.Item == this).ToList())
            {
                property.Delete();
            }
        }
    }
}
