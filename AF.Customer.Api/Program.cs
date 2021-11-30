using Microsoft.Azure.Functions.Worker.Configuration;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Blog.Service.BlogApi.Infrastructure.Configurations;
using System.Configuration;
using Blog.Service.BlogApi.Infrastructure.Contexts;
using AutoMapper;
using FluentValidation;
using System.Reflection;
using Autofac;
using Blog.Service.BlogApi.Infrastructure.Domain;
using Autofac.Extensions.DependencyInjection;
using Blog.Service.BlogApi.Api.Services;

namespace AF.Customer.Api
{
    public class Program
    {
        readonly static string _storageConnectionString = Environment.GetEnvironmentVariable("MongoDB:ConnectionString") ?? string.Empty;

        public static void Main()
        {
            var host = new HostBuilder()                
                //.UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureOpenApi()
                //.ConfigureLogging(builder =>
                //{
                //    builder.ClearProviders();
                //    builder.AddLog4Net();
                //})
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(builder => {
                    builder.RegisterModule(new DomainModule());
                    builder.RegisterModule(new MediatorModule());
                })
                .ConfigureServices(services =>
                {
                    // Add Logging
                    services.AddLogging();

                    // Add HttpClient
                    services.AddHttpClient();

                    services.AddAutofac();

                    services.Configure<BlogConfiguration>(
                        options =>
                        {
                            options.ConnectionString = "mongodb://marketplacemongo:BagN4f2qvfrLElCbWOt0jf3Bn1D5DHTxQTLA4jwiBc7EoNx64MjdKciJSc2Bq2X9Guej5JVuKcebVxd5OD0dJQ==@marketplacemongo.mongo.cosmos.azure.com:10255/Monitor?ssl=true&replicaSet=globaldb&retrywrites=false&maxIdleTimeMS=120000&appName=@marketplacemongo@";
                            options.Database = "Monitor";
                        }
                    );
                    services.AddSingleton<IBlogContext, BlogContext>();

                    services.AddAutoMapper(cfg =>
                    {
                        cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
                    });
                    

                    services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

                    // Add Custom Services
                    //services.AddSingleton<BlobStorageService>();
                    //services.AddSingleton<CloudBlobClient>(CloudStorageAccount.Parse(_storageConnectionString).CreateCloudBlobClient());
                })
                .ConfigureFunctionsWorkerDefaults()
                .Build();

            host.Run();
        }
    }
}