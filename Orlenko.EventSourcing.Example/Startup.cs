using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Orlenko.EventSourcing.Example.Authentication;
using Orlenko.EventSourcing.Example.Authorization;
using Orlenko.EventSourcing.Example.Contracts.Abstractions;
using Orlenko.EventSourcing.Example.Contracts.Events;
using Orlenko.EventSourcing.Example.Contracts.Models;
using Orlenko.EventSourcing.Example.Core.Aggregates;
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
            services.AddMvc();
            services.AddAuthenticationCore(opt => opt.AddScheme(CustomAuthenticationHandler.Scheme, builder => builder.HandlerType = typeof(CustomAuthenticationHandler)));
            services.AddAuthorization(cfg => cfg.AddPolicy("AllUsers", policy => {
                policy.AddAuthenticationSchemes(CustomAuthenticationHandler.Scheme);
                policy.AddRequirements(new MyRequirement());
            }));
            services.AddSingleton<IAuthorizationHandler, CustomAuthorizationHandler>();

            services.AddCors();
            //services.AddSingleton<IEventsStore<BaseItemEvent>, InMemoryEventStore>();
            services.AddSingleton<IEventsStore<BaseItemEvent>, InFileEventStore>();
            
            //services.AddSingleton<IAggregateRepository<ItemAggregate>, InMemoryAggregateRepository>();
            services.AddSingleton<IAggregateRepository<ItemAggregate>, InlineRestoreAggregateRepository>();
            services.AddTransient<ICommandHandler, DefaultCommandHandler>();
            services.AddTransient<IEventsPublisher, MockEventPublisher>();
            services.AddSingleton<AggregateRoot>();
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
