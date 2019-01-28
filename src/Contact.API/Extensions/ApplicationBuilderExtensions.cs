using Contact.API.IntegrationEvents.EventHandling;
using Contact.API.IntegrationEvents.Events;
using Finbook.BuildingBlocks.EventBus.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.API.Extensions
{
    public static class ApplicationBuilderExtensions
    {

        ///// <summary>
        ///// 注册订阅EventBus相关处理类
        ///// </summary>
        ///// <param name="app"></param>
        ///// <returns></returns>
        //public static IApplicationBuilder UseEventBus(this IApplicationBuilder app)
        //{
        //    var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
        //    eventBus.Subscribe<UserInfoChangedIntegrationEvent, UserInfoChangedIntegrationEventHandler>();

        //    return app;
        //}
    }
}
