using Starcounter.Nova;

namespace Constructor.Database
{
    [Database]
    public abstract class Property
    {
        public abstract Commit Commit { get; set; }
        public abstract Item Item { get; set; }
        public abstract string Name { get; set; }
        public abstract string StringValue { get; set; }
        public abstract int? IntValue { get; set; }
        public abstract bool? BoolValue { get; set; }

        public static Property Create()
        {
            return Db.Insert<Property>();
        }
    }
}