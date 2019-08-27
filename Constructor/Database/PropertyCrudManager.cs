using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Starcounter.Nova;
using Starcounter.Palindrom.Database;

namespace Constructor.Database
{
    internal class PropertyCrudManager
    {
        private ITransactionFactory TransactionFactory { get; }

        public PropertyCrudManager(ITransactionFactory transactionFactory)
        {
            TransactionFactory = transactionFactory;
        }

        public string GetStringValue(Item item, [CallerMemberName] string propertyName = "")
        {
            return TransactionFactory.Read(() =>
            {
                Property property = GetReadProperty(item.Repository.CurrentCommit, item, propertyName);
                return property?.StringValue;
            });
        }

        public int? GetIntValue(Item item, [CallerMemberName] string propertyName = "")
        {
            return TransactionFactory.Read(() =>
            {
                Property property = GetReadProperty(item.Repository.CurrentCommit, item, propertyName);
                return property?.IntValue;
            });
        }

        public bool? GetBoolValue(Item item, [CallerMemberName] string propertyName = "")
        {
            return TransactionFactory.Read(() =>
            {
                Property property = GetReadProperty(item.Repository.CurrentCommit, item, propertyName);
                return property?.BoolValue;
            });
        }


        public void SetStringValue(Item item, string value, [CallerMemberName] string propertyName = "")
        {
            TransactionFactory.Transact(() =>
            {
                Property property = GetOrCreateWriteProperty(item.Repository.CurrentCommit, item, propertyName);
                property.StringValue = value;
            });
        }

        public void SetIntValue(Item item, int? value, [CallerMemberName] string propertyName = "")
        {
            TransactionFactory.Transact(() =>
            {
                Property property = GetOrCreateWriteProperty(item.Repository.CurrentCommit, item, propertyName);
                property.IntValue = value;
            });
        }

        public void SetBoolValue(Item item, bool? value, [CallerMemberName] string propertyName = "")
        {
            TransactionFactory.Transact(() =>
            {
                Property property = GetOrCreateWriteProperty(item.Repository.CurrentCommit, item, propertyName);
                property.BoolValue = value;
            });
        }

        public Property GetReadProperty(Commit commit, Item item, string propertyName)
        {
            var query = Db.SQL<Property>
            (
                query: @"SELECT p FROM Constructor.Database.Property p 
                         WHERE ? STARTS WITH p.Commit.Key 
                         AND p.Commit.CreatedAtUtc <= ? AND p.Item = ? 
                         AND p.Name = ? ORDER BY p.Commit.CreatedAtUtc DESC",
                arg1: commit.Key,
                arg2: commit.CreatedAtUtc,
                arg3: item,
                propertyName
            );
            return query.FirstOrDefault();
        }

        public Property GetOrCreateWriteProperty(Commit commit, Item item, string propertyName)
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

            var query = Db.SQL<Property>
            (
                query: @"SELECT p FROM Constructor.Database.Property p 
                         WHERE p.Commit = ? AND p.Item = ? AND p.Name = ?
                         ORDER BY p.Commit.CreatedAtUtc DESC",
                arg1: commit,
                arg2: item,
                arg3: propertyName
            );
            var property = query.FirstOrDefault();

            if (property == null)
            {
                property = Property.Create();
                property.Commit = commit;
                property.Item = item;
                property.Name = propertyName;
            }

            return property;
        }
    }
}