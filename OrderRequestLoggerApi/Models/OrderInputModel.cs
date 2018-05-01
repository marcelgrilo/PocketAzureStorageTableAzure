using System;

namespace OrderRequestLoggerApi.Models
{
    public class OrderInputModel
    {
        public string OrderId { get; set; }
        public Guid BusinessId { get; set; }
        public DateTime Date { get; set; }
        public string SerializedData { get; set; }
    }
}
