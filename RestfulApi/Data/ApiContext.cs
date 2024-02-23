using Microsoft.EntityFrameworkCore;
using RestfulApi.Models;

namespace RestfulApi.Data
{
    public class ApiContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public ApiContext(DbContextOptions<ApiContext> options) : base(options) 
        {

        }
    }
}
