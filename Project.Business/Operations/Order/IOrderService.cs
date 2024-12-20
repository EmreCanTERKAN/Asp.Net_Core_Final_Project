﻿using Project.Business.Operations.Order.Dto;
using Project.Business.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Business.Operations.Order
{
    public interface IOrderService
    {
        Task<ServiceMessage> AddOrder(AddOrderDto order);

        Task<List<OrderListDto>> GetAllOrders(int pageNumber, int pageSize);
        Task<ServiceMessage<OrderListDto>> GetOrderById(int orderId);
        Task<ServiceMessage> UpdateOrderProduts(int orderId, UpdateOrderDto updateOrderDto);
        Task<ServiceMessage> SoftDeleteOrder(int orderId);

    }
}
