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

        internal static PropertyCrudManager PropertyCrud { get; set; }

        public static T CreateItem<T>(Repository repository) where T : Item
        {
            var instance = Db.Insert<T>();
            instance.Repository = repository ?? throw new ArgumentNullException(nameof(repository));
            instance.IsDeleted = false;
            return instance;
        }

        public string Name
        {
            get => PropertyCrud.GetStringValue(this);
            set => PropertyCrud.SetStringValue(this, value);
        }

        public string Description
        {
            get => PropertyCrud.GetStringValue(this);
            set => PropertyCrud.SetStringValue(this, value);
        }

        public string ImageUrl
        {
            get => PropertyCrud.GetStringValue(this);
            set => PropertyCrud.SetStringValue(this, value);
        }

        public bool IsDeleted
        {
            get => PropertyCrud.GetBoolValue(this) ?? true;
            set => PropertyCrud.SetBoolValue(this, value);
        }

        public int SortIndex
        {
            get => PropertyCrud.GetIntValue(this) ?? 0;
            set => PropertyCrud.SetIntValue(this, value);
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