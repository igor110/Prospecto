using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace Prospecto.Schedule.Extensions
{
    public static class ScheduleDependencyInjectionExtension
    {
        public static void AddScheduleDependencyInjection(
            this IServiceCollection services)
        {            
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();            
            services.AddSingleton<ScheduledAppointments>();
            services.AddSingleton(
                 new JobSchedule(jobType: typeof(ScheduledAppointments), cronExpression: "0 30 8 ? * * *")
           );
        }
    }
}
