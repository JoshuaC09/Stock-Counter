//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;
//using WebApplication1.DatabaseContext;

//namespace WebApplication2
//{
//    public class MyDesignTimeDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
//    {
//        public MyDbContext CreateDbContext(string[] args)
//        {
//            var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();
//            var connectionString = "Server=luigi;Database=bgc_count;User=root;Password=BLUEGATES;";
//            optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 21)));

//            return new MyDbContext(optionsBuilder.Options);
//        }
//    }
//}
