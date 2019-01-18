 
using Finbook.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Finbook.BuildingBlocks.EventBus.Abstractions
{

    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
        where TIntegrationEvent : IntegrationEvent
    {
        /// <summary>
        /// 处理EventBus消息
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        Task Handle(TIntegrationEvent @event);
    }

    /// <summary>
    /// 暂时未明白定义一个空接口意图
    /// </summary>
    public interface IIntegrationEventHandler
    {
    }
}
