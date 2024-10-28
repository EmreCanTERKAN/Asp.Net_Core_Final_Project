using Microsoft.EntityFrameworkCore;
using Project.Business.Operations.Order.Dto;
using Project.Business.Operations.Product;
using Project.Business.Types;
using Project.Data.Context;
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
        private readonly IRepository<ProductEntity> _productRepository;
        private readonly FinalProjectDbContext _context;

        public OrderManager(IUnitOfWork unitOfWork, IProductService productService, IRepository<OrderEntity> orderRepository, IRepository<OrderProductEntity> orderProductRepository, IRepository<ProductEntity> productRepository, FinalProjectDbContext context )
        {
            _unitOfWork = unitOfWork;
            _productService = productService;
            _orderRepository = orderRepository;
            _orderProductRepository = orderProductRepository;
            _productRepository = productRepository;
            _context = context;
        }

        public async Task<ServiceMessage> AddOrder(AddOrderDto orderDto)
        {
            try
            {
                // Yeni bir sipariş oluştur
                var order = new OrderEntity
                {
                    UserId = orderDto.UserId,
                    OrderDate = DateTime.Now
                };

                decimal totalAmount = 0;

                // Her sipariş ürünü için
                foreach (var opDto in orderDto.OrderProducts)
                {
                    // Ürünü veritabanından al
                    var product = _productRepository.GetById(opDto.ProductId);
                    if (product == null)
                    {
                        return new ServiceMessage
                        {
                            IsSucceed = false,
                            Message = $"Ürün bulunamadı: {opDto.ProductId}"
                        };
                    }

                    // Stok kontrolü
                    if (product.StockQuantity < opDto.Quantity)
                    {
                        return new ServiceMessage
                        {
                            IsSucceed = false,
                            Message = $"Yetersiz stok: {product.ProductName} - Stok: {product.StockQuantity}"
                        };
                    }

                    // OrderProduct nesnesini oluştur ve ilişkilendir
                    var orderProduct = new OrderProductEntity
                    {
                        Order = order,
                        ProductId = opDto.ProductId,
                        Quantity = opDto.Quantity
                    };

                    // Toplam tutarı hesapla
                    totalAmount += product.Price * opDto.Quantity;

                    // Stok güncellemesi
                    product.StockQuantity -= opDto.Quantity;
                    _productRepository.Update(product); // Stok güncellemesi için Product nesnesini kaydet

                    // OrderProduct ilişkisini ekle
                    order.OrderProducts.Add(orderProduct);
                }

                // Siparişin toplam tutarını belirle
                order.TotalAmount = totalAmount;

                // Siparişi kaydet
                await _orderRepository.Add(order);

                // Değişiklikleri kaydet
                await _unitOfWork.SaveChangesAsync();

                return new ServiceMessage
                {
                    IsSucceed = true,
                    Message = "Sipariş başarıyla eklendi"
                };
            }
            catch (Exception ex)
            {
                // Loglama yapılabilir
                throw new Exception("Kayıt sırasında bir hata oluştu.", ex);
            }
        }

        public async Task<List<OrderListDto>> GetAllOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .Include(o => o.User)
                .ToListAsync();

            var orderListDtos = new List<OrderListDto>();

            foreach (var order in orders)
            {
                var orderProducts = order.OrderProducts.Select(op => new OrderProductDto
                {
                    ProductId = op.ProductId,
                    Quantity = op.Quantity,
                    Price = op.Product.Price
                }).ToList();

                var orderListDto = new OrderListDto
                {
                    OrderId = order.Id,
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount,
                    OrderProducts = orderProducts,
                    UserId = order.UserId,
                    UserFullName = $"{order.User.FirstName} {order.User.LastName}"                   
                };

                orderListDtos.Add(orderListDto);
            }

            return orderListDtos;
        }

        public async Task<OrderListDto> GetOrderById(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            var orderProducts = order.OrderProducts.Select(op => new OrderProductDto
            {
                ProductId = op.ProductId,
                Quantity = op.Quantity,
                Price = op.Product.Price
            }).ToList();

            var orderListDto = new OrderListDto
            {
                OrderId = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                OrderProducts = orderProducts,
                UserId = order.UserId,
                UserFullName = $"{order.User.FirstName} {order.User.LastName}"
            };

            return orderListDto;
        }
    }
}
