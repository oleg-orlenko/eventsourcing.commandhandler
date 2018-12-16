using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Orlenko.EventSourcing.Example.Contracts.Abstractions;
using Orlenko.EventSourcing.Example.Contracts.Events;
using Orlenko.EventSourcing.Example.Contracts.Models;
using Orlenko.EventSourcing.Example.Core.CommandHandlers;
using Orlenko.EventSourcing.Example.Core.EventPublishers;
using Orlenko.EventSourcing.Example.Repository;

namespace Orlenko.EventSourcing.Example
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore();
            services.AddCors();
            services.AddSingleton<IEventsStore<BaseItemEvent>, InMemoryEventStore>();
            services.AddSingleton<IAggregateRepository<ItemAggregate>, InMemoryAggregateRepository>();
            services.AddTransient<ICommandHandler, DefaultCommandHandler>();
            services.AddTransient<IEventsPublisher, MockEventPublisher>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseCors();
        }
    }
}
