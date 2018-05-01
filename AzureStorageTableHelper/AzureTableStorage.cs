using System.Collections.Generic;
using System.Threading.Tasks;

using AzureStorageTableHelper.Interfaces;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureStorageTableHelper
{
    public class AzureTableStorage<T> : IAzureTableStorage<T>
       where T : AzureTableEntity, new()
    {
        #region Public Methods

        public AzureTableStorage(AzureTableSettings settings)
        {
            this.settings = settings;
        }

        public async Task<List<T>> GetList()
        {
            //Table
            CloudTable table = await GetTableAsync();

            //Query
            TableQuery<T> query = new TableQuery<T>();

            List<T> results = new List<T>();
            TableContinuationToken continuationToken = null;
            do
            {
                TableQuerySegment<T> queryResults =
                    await table.ExecuteQuerySegmentedAsync(
                    query: query,
                    token: continuationToken);

                continuationToken = queryResults.ContinuationToken;
                results.AddRange(queryResults.Results);

            } while (continuationToken != null);

            return results;
        }

        public async Task<List<T>> GetList(string partitionKey)
        {
            //Table
            CloudTable table = await GetTableAsync();

            //Query
            TableQuery<T> query = new TableQuery<T>()
                .Where(
                    TableQuery.GenerateFilterCondition(
                        propertyName: "PartitionKey",
                        operation: QueryComparisons.Equal,
                        givenValue: partitionKey));

            List<T> results = new List<T>();
            TableContinuationToken continuationToken = null;
            do
            {
                TableQuerySegment<T> queryResults =
                    await table.ExecuteQuerySegmentedAsync(
                        query: query,
                        token: continuationToken);

                continuationToken = queryResults.ContinuationToken;

                results.AddRange(queryResults.Results);

            } while (continuationToken != null);

            return results;
        }

        public async Task<T> GetItem(string partitionKey, string rowKey)
        {
            //Table 
            CloudTable table = await GetTableAsync();

            //Operation
            TableOperation operation = TableOperation.Retrieve<T>(partitionKey, rowKey);

            //Execute
            TableResult result = await table.ExecuteAsync(operation);

            return (T)(dynamic)result.Result;
        }

        public async Task Insert(T item)
        {
            //Table
            CloudTable table = await GetTableAsync();

            //Operation
            TableOperation operation = TableOperation.Insert(item);

            //Execute
            await table.ExecuteAsync(operation);
        }

        public async Task Update(T item)
        {
            //Table
            CloudTable table = await GetTableAsync();

            //Operation
            TableOperation operation = TableOperation.InsertOrReplace(item);

            //Execute
            await table.ExecuteAsync(operation);
        }

        public async Task Delete(string partitionKey, string rowKey)
        {
            //Item
            T item = await GetItem(partitionKey, rowKey);

            //Table
            CloudTable table = await GetTableAsync();

            //Operation
            TableOperation operation = TableOperation.Delete(item);

            //Execute
            await table.ExecuteAsync(operation);
        }

        #endregion

        #region Private Methods

        private readonly AzureTableSettings settings;

        private async Task<CloudTable> GetTableAsync()
        {
            CloudStorageAccount storageAccount = null;
            //Account
            if (string.IsNullOrEmpty(this.settings.StorageConnectionString))
            {
                storageAccount = new CloudStorageAccount(
                        new StorageCredentials(this.settings.StorageAccount, this.settings.StorageKey), false);
            }
            else
            {
                storageAccount = CloudStorageAccount.Parse(this.settings.StorageConnectionString);
            }
            //Client
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            //Table
            CloudTable table = tableClient.GetTableReference(this.settings.TableName);
            await table.CreateIfNotExistsAsync();

            return table;
        }

        #endregion
    }
}
