﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Business.Operations.Order.Dto
{
    public class UpdateOrderDto
    {
        public List<OrderProductDto> Products { get; set; }
    }
}
