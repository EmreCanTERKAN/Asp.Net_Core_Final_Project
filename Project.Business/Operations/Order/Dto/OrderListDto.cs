using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Business.Operations.Order.Dto
{
    public class OrderListDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderProductDto> OrderProducts { get; set; }

        // Kullanıcı Bilgileri
        public int UserId { get; set; }
        public string UserFullName { get; set; }
    }
}
