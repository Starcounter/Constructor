using System;
using System.Linq;
using System.Runtime.CompilerServices;
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

        public static string GetStringValue(Commit commit, Item item, [CallerMemberName] string propertyName = "")
        {
            Property property = GetReadProperty(commit, item, propertyName);
            return property?.StringValue;
        }

        public static int? GetIntValue(Commit commit, Item item, [CallerMemberName] string propertyName = "")
        {
            Property property = GetReadProperty(commit, item, propertyName);
            return property?.IntValue;
        }

        public static bool? GetBoolValue(Commit commit, Item item, [CallerMemberName] string propertyName = "")
        {
            Property property = GetReadProperty(commit, item, propertyName);
            return property?.BoolValue;
        }

        public static void SetStringValue(Commit commit, Item item, string value, [CallerMemberName] string propertyName = "")
        {
            Db.Transact(() =>
            {
                Property property = GetOrCreateWriteProperty(commit, item, propertyName);
                property.StringValue = value;
            });
        }

        public static void SetIntValue(Commit commit, Item item, int? value, [CallerMemberName] string propertyName = "")
        {
            Db.Transact(() =>
            {
                Property property = GetOrCreateWriteProperty(commit, item, propertyName);
                property.IntValue = value;
            });
        }

        public static void SetBoolValue(Commit commit, Item item, bool? value, [CallerMemberName] string propertyName = "")
        {
            Db.Transact(() =>
            {
                Property property = GetOrCreateWriteProperty(commit, item, propertyName);
                property.BoolValue = value;
            });
        }

        public static Property GetReadProperty(Commit commit, Item item, string propertyName)
        {
            Property property = Db.SQL<Property>
            (
                @"SELECT p FROM Constructor.Database.Property p 
                    WHERE ? STARTS WITH p.Commit.Key AND p.Commit.CreatedAtUtc <= ? AND p.Item = ? AND p.Name = ?
                    ORDER BY p.Commit.CreatedAtUtc DESC",
                commit.Key,
                commit.CreatedAtUtc,
                item,
                propertyName
            ).FirstOrDefault();

            return property;
        }

        public static Property GetOrCreateWriteProperty(Commit commit, Item item, string propertyName)
        {
            if (commit == null)
            {
                throw new ArgumentNullException(nameof(commit));
            }

            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            Property property = Db.SQL<Property>
            (
                @"SELECT p FROM Constructor.Database.Property p 
                    WHERE p.Commit = ? AND p.Item = ? AND p.Name = ?
                    ORDER BY p.Commit.CreatedAtUtc DESC",
                commit,
                item,
                propertyName
            ).FirstOrDefault();

            if (property == null)
            {
                property = Create();
                property.Commit = commit;
                property.Item = item;
                property.Name = propertyName;
            }

            return property;
        }
    }
}