using System;
using Api.Data.Implementation;
using Api.Data.Repository;
using Api.Domain.Interfaces;
using Api.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Api.CrossCuting.DependecyInjection
{
    public class ConfigureRepository
    {
        public static void ConfigureDependenciesRepository(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            serviceCollection.AddScoped<IUserRepository, UserImplamentation>();

            if (Environment.GetEnvironmentVariable("DATABASE").ToLower() == "SQL".ToLower())
            {
                serviceCollection.AddDbContext<Api.Data.Context.MyContext>(
                              //  op => op.UseMySql("Server=localhost;Port=3306;Database=dbapi;Uid=wilsonalves;Pwd=Wilson123@")
                              op => op.UseSqlServer(Environment.GetEnvironmentVariable("DB_CONNECTION"))
                            );
            }
            else
            {
                serviceCollection.AddDbContext<Api.Data.Context.MyContext>(
                             //  op => op.UseMySql("Server=localhost;Port=3306;Database=dbapi;Uid=wilsonalves;Pwd=Wilson123@")
                             op => op.UseMySql(Environment.GetEnvironmentVariable("DB_CONNECTION"))
                           );
            }


        }
    }
}