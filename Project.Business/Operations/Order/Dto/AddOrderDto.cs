using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Business.Operations.Order.Dto
{
    public class AddOrderDto
    {
        public int UserId { get; set; }
        public List<OrderProductDto> OrderProducts { get; set; }
    }
}
