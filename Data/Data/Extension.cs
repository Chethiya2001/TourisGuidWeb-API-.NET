using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Models;
using Data.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Data.Data
{
    public static class Extension
    {
        public static IServiceCollection AddMongo(this IServiceCollection services)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

            services.AddSingleton(serviceProvider =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
                var mongoDbSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();

                Console.WriteLine($"Mongo ConnectionString: {mongoDbSettings?.ConnectionString}");
                Console.WriteLine($"Database Name: {serviceSettings?.ServiceName}");

                if (string.IsNullOrEmpty(serviceSettings?.ServiceName))
                {
                    throw new ArgumentNullException(nameof(serviceSettings.ServiceName), "Database name is missing in configuration");
                }

                return new MongoClient(mongoDbSettings!.ConnectionString).GetDatabase(serviceSettings.ServiceName);
            });

            return services; 
        }

        public static IServiceCollection AddMongoRepository<P, EID>(
        this IServiceCollection services,
        string collectionName)
        where P : IEntity<EID>
        {
            services.AddSingleton<IRepository<P, EID>>(serviceProvider =>
            {
                var database = serviceProvider.GetRequiredService<IMongoDatabase>();
                return new Repository<P, EID>(database, collectionName);
            });

            return services;
        }

    }
}