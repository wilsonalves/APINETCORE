using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Context
{
    public class ContextFactory : IDesignTimeDbContextFactory<MyContext>
    {
        public MyContext CreateDbContext(string[] args)
        {
            //var connectionString = "Server=localhost;Port=3306;Database=dbapi;Uid=wilsonalves;Pwd=Wilson123@";
            var connectionString = "Server=.\\BARTENDER;Initial Catalog=dbapi; MultipleActiveResultSets=true; Integrated Security=True";
            var optionBuilder = new DbContextOptionsBuilder<MyContext>();
            //optionBuilder.UseMySql(connectionString);
            optionBuilder.UseSqlServer(connectionString);
            return new MyContext(optionBuilder.Options);
        }
    }
}