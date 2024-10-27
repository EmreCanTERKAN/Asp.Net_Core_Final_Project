using Project.Business.Operations.Order.Dto;
using Project.Business.Operations.Product;
using Project.Business.Types;
using Project.Data.Entities;
using Project.Data.Repositories;
using Project.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Business.Operations.Order
{
    public class OrderManager : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductService _productService;
        private readonly IRepository<OrderEntity> _orderRepository;
        private readonly IRepository<OrderProductEntity> _orderProductRepository;

        public OrderManager(IUnitOfWork unitOfWork, IProductService productService, IRepository<OrderEntity> orderRepository, IRepository<OrderProductEntity> orderProductRepository)
        {
            _unitOfWork = unitOfWork;
            _productService = productService;
            _orderRepository = orderRepository;
            _orderProductRepository = orderProductRepository;
        }

        public async Task<ServiceMessage> AddOrder(AddOrderDto orderDto)
        {
            try
            {
                var order = new OrderEntity
                {
                    UserId = orderDto.UserId,
                    OrderDate = DateTime.Now
                };

                decimal totalAmount = 0;

                foreach (var opDto in orderDto.OrderProducts)
                {
                    var product = await _productService.GetProduct(opDto.ProductId);
                    if (product is null)
                    {
                        return new ServiceMessage
                        {
                            IsSucceed = false,
                            Message = "Ürün bulunamadı"
                        };
                    }
                    var orderProduct = new OrderProductEntity
                    {
                        Order = order,
                        ProductId = opDto.ProductId,
                        Quantity = opDto.Quantity
                    };

                    totalAmount += product.Price * opDto.Quantity;

                    await _orderProductRepository.Add(orderProduct);
                }

                order.TotalAmount = totalAmount;
                await _orderRepository.Add(order);
                await _unitOfWork.SaveChangesAsync();

                return new ServiceMessage
                {
                    IsSucceed = true,
                    Message = "Sipariş eklendi"
                };

            }
            catch (Exception)
            {

                throw new Exception("Kayıt sırasında bir hata oluştu.");
                
            }
        }
    }
}
