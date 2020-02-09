using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orlenko.EventSourcing.Example.Authentication;
using Orlenko.EventSourcing.Example.Authorization;
using Orlenko.EventSourcing.Example.Contracts.Abstractions;
using Orlenko.EventSourcing.Example.Core.CommandHandlers;
using Orlenko.EventSourcing.Example.Domain;
using Orlenko.EventSourcing.Example.Domain.Events;
using Orlenko.EventSourcing.Example.EventPublishers;
using Orlenko.EventSourcing.Example.Repository.MongoDb;
using Orlenko.EventSourcing.Example.Repository.MongoDb.Configuration;

namespace Orlenko.EventSourcing.Example
{
    public class Startup
    {
        private readonly IConfiguration config;

        public Startup(IConfiguration config)
        {
            this.config = config;
        }

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
            //services.AddSingleton<IEventsStore<BaseEvent<Item>>, InMemoryEventStore>();
            //services.AddTransient<IEventsStore<BaseEvent<Item>>, InFileEventStore>();
            var mongoConfigSection = this.config.GetSection("mongoDB");
            var mongoEventsConfig = new MongoEventsConfig(mongoConfigSection);
            var mongoSnapshotsConfig = new MongoSnapshotsConfig(mongoConfigSection);
            services.AddSingleton(mongoEventsConfig);
            services.AddSingleton(mongoSnapshotsConfig);
            services.AddTransient<IEventsStore<BaseEvent<Item>>, MongoEventsStore>();

            //services.AddSingleton<IAggregateRepository<ItemAggregate>, InMemoryAggregateRepository>();
            //services.AddTransient<IAggregateRepository<ItemAggregate>, InlineRestoreAggregateRepository>();

            services.AddTransient<ICommandHandler, ItemsCommandHandler>();
            services.AddTransient<IEventsPublisher<BaseEvent<Item>>, MockEventPublisher>();
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
