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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Note to Leo: Here's where I load some "default" data to the DB, as requested in the technical assessment.
            // The IDs are set manually. Normally, I would let the DB handle this, but since this is an in-memory DB, I set them manually.
            
            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    Id = 1,
                    Name = "John Doe",
                    Address = "123 Main St",
                    DNI = "12345678",
                    Email = "john.doe@example.com",
                    Phone = "111-222-3333",
                    Mobile = "444-555-6666",
                    State = "State1",
                    City = "City1"
                },
                new Customer
                {
                    Id = 2,
                    Name = "Jane Doe",
                    Address = "456 Main St",
                    DNI = "87654321",
                    Email = "jane.doe@example.com",
                    Phone = "777-888-9999",
                    Mobile = "000-111-2222",
                    State = "State2",
                    City = "City2"
                },
                new Customer
                {
                    Id = 3,
                    Name = "Juan Perez",
                    Address = "495 Main St",
                    DNI = "87656648",
                    Email = "juanperez@gmail.com",
                    Phone = "777-128-4545",
                    Mobile = "000-166-1741",
                    State = "Houston",
                    City = "Texas"
                }
            );
        }
    }
}
