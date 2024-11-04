using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Business.Operations.Order.Dto
{
    public class OrderProductUpdateDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
