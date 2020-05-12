using DotNetCore.Demo.GRPC.Server.Proto;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.Demo.GRPC.Server.Service
{
    public class OrderService: OrderGrpc.OrderGrpcBase
    {
        public override Task<CreateOrderResponse> CreateOrder(CreateOrderRequest request, ServerCallContext context)
        {
            //添加创建订单的内部逻辑，录入将订单信息存储到数据库
            return Task.FromResult(new CreateOrderResponse { OrderId = 100 });
        }
    }
}
