﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasteRestaurant.Data;

namespace TasteRestaurant.ViewModel
{
    public class OrderDetailsCart
    {
        public List<ShoppingCart> ListCart { get; set; }

        public OrderHeader OrderHeader { get; set; }
    }
}
