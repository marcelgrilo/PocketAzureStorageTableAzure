using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Logger.Domain.Entities;
using Logger.Domain.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

using OrderRequestLoggerApi.Helpers;
using OrderRequestLoggerApi.Models;

namespace OrderRequestLoggerApi.Controllers
{
    [Route("orderlogger")]
    public class OrderLoggerController : BaseController
    {
        private readonly IOrderService orderService;
        public OrderLoggerController(IOrderService orderService)
        {
            this.orderService = orderService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var model = await orderService.GetOrders();

            var outputModel = ToOutputModel(model);
            return Ok(outputModel);
        }

        [HttpGet("{businessId}/{orderId}/{date}", Name = "GetOrder")]
        public async Task<IActionResult> Get(Guid businessId, string orderId, DateTime date)
        {
            var model = await orderService.GetOrder(date, businessId, orderId);
            if (model == null)
                return NotFound();

            var outputModel = ToOutputModel(model);
            return Ok(outputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]OrderInputModel inputModel)
        {
            if (inputModel == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return Unprocessable(ModelState);

            var model = ToDomainModel(inputModel);
            await orderService.AddOrder(model);

            var outputModel = ToOutputModel(model);
            return CreatedAtRoute("GetOrder",
                new { businessId = outputModel.BusinessId, orderId = outputModel.OrderId, date = outputModel.Date },
                outputModel);
        }


        [HttpPut("{businessId}/{orderId}/{date}")]
        public async Task<IActionResult> Update(Guid businessId, string orderId, DateTime date,
            [FromBody]OrderInputModel inputModel)
        {
            if (inputModel == null
                || businessId != inputModel.BusinessId
                || orderId != inputModel.OrderId
                || date != inputModel.Date)
                return BadRequest();

            if (!await orderService.OrderExists(date, businessId, orderId))
                return NotFound();

            if (!ModelState.IsValid)
                return Unprocessable(ModelState);

            var model = ToDomainModel(inputModel);
            await orderService.UpdateOrder(model);

            return NoContent();
        }

        [HttpDelete("{businessId}/{orderId}/{date}")]
        public async Task<IActionResult> Delete(Guid businessId, string orderId, DateTime date)
        {
            if (!await orderService.OrderExists(date, businessId, orderId))
                return NotFound();

            await orderService.DeleteOrder(date, businessId, orderId);

            return NoContent();
        }

        private OrderOutputModel ToOutputModel(Order model)
        {
            return new OrderOutputModel
            {
                BusinessId = model.BusinessId,
                OrderId = model.OrderId,
                Date = model.Date,
                SerializedData = model.SerializedData,
                LastReadAt = DateTime.Now
            };
        }

        private List<OrderOutputModel> ToOutputModel(List<Order> model)
        {
            return model.Select(item => ToOutputModel(item))
                        .ToList();
        }

        private Order ToDomainModel(OrderInputModel inputModel)
        {
            return new Order
            {
                PartitionKey = Order.GeneratePartitionKey(inputModel.BusinessId, inputModel.OrderId),
                RowKey = Order.GenerateRowKey(inputModel.Date),

                BusinessId = inputModel.BusinessId,
                OrderId = inputModel.OrderId,
                Date = inputModel.Date,
                SerializedData = inputModel.SerializedData
            };
        }

        private OrderInputModel ToInputModel(Order model)
        {
            return new OrderInputModel
            {
                BusinessId = model.BusinessId,
                OrderId = model.OrderId,
                Date = model.Date,
                SerializedData = model.SerializedData
            };
        }
    }
}