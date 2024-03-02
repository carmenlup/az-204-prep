﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CosmosDb
{
    public record Order(
        Guid id,
        string orderId,
        string category,
        int quantity
    );
}
