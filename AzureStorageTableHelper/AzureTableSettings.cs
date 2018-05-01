using System;

namespace AzureStorageTableHelper
{
    public class AzureTableSettings
    {
        public AzureTableSettings(string storageAccount,
                                       string storageKey,
                                       string tableName,
                                       string storageConnectionString)
        {
            if (string.IsNullOrEmpty(storageAccount))
                throw new ArgumentNullException("StorageAccount");

            if (string.IsNullOrEmpty(storageKey))
                throw new ArgumentNullException("StorageKey");

            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException("TableName");

            if (string.IsNullOrEmpty(storageConnectionString))
                throw new ArgumentNullException("StorageConnectionString");

            this.StorageAccount = storageAccount;
            this.StorageKey = storageKey;
            this.TableName = tableName;
            this.StorageConnectionString = storageConnectionString;

        }
        public string StorageConnectionString { get; set; }
        public string StorageAccount { get; }
        public string StorageKey { get; }
        public string TableName { get; }
    }
}
