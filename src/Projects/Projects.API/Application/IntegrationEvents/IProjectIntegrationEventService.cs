using Finbook.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projects.API.Application.IntegrationEvents
{
    public interface IProjectIntegrationEventService
    {
        /// <summary>
        /// 发送集成事件只MQ
        /// </summary>
        /// <param name="evt"></param>
        /// <returns></returns>
        Task PublishThroughEventBusAsync(IntegrationEvent evt);
    }
}
