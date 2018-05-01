using System;

using AzureStorageTableHelper;

namespace Logger.Domain.Entities
{
    public class Order : AzureTableEntity
    {
        public string OrderId { get; set; }
        public Guid BusinessId { get; set; }
        public DateTime Date { get; set; }
        public string SerializedData { get; set; }

        public static string GeneratePartitionKey(Guid businessId, string orderId)
        {
            return $"{businessId}-{orderId}";
        }

        public static string GenerateRowKey(DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
