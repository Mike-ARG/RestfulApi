using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestfulApi.Controllers;
using RestfulApi.Data;
using RestfulApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace RestfulApi.Tests
{
    public class CustomerControllerTests
    {
        /* Note to Leo: I tried to implement a variety of tests, but of course, more could be added to handle more cases. */

        #region GetAll Tests
        [Fact]
        public void GetAll_ReturnsAllCustomers()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "TestDbForGetAll")
                .Options;

            // Insert test data into the in-memory database
            using (var context = new ApiContext(options))
            {
                context.Customers.Add(new Customer
                {
                    Id = 1,
                    Name = "John Doe",
                    Address = "Address 1",
                    DNI = "37333444",
                    Email = "testEmail@gmail.com",
                    Phone = "4586895522",
                    Mobile = "4526897146",
                    State = "California",
                    City = "Los Angeles"
                });
                context.Customers.Add(new Customer
                {
                    Id = 2,
                    Name = "Jane Doe",
                    Address = "Address 2",
                    DNI = "37333445",
                    Email = "testEmail2@gmail.com",
                    Phone = "4586895523",
                    Mobile = "4526897140",
                    State = "California",
                    City = "Los Angeles"
                });
                context.SaveChanges();
            }

            using (var context = new ApiContext(options))
            {
                var controller = new CustomerController(context);

                // Act
                var result = controller.GetAll();

                // Assert
                var jsonResult = Assert.IsType<JsonResult>(result);
                var okObjectResult = jsonResult.Value as OkObjectResult;
                Assert.NotNull(okObjectResult);

                var customers = Assert.IsType<List<Customer>>(okObjectResult.Value);
                Assert.Equal(2, customers.Count); // Since 2 customers where added, there should be 2 customers in the list
            }
        }

        [Fact]
        public void GetAll_ReturnsEmptyList_WhenDatabaseIsEmpty()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "TestDbForGetAllEmpty")
                .Options;

            using (var context = new ApiContext(options))
            {
                var controller = new CustomerController(context);

                // Act
                var result = controller.GetAll();

                // Assert
                var jsonResult = Assert.IsType<JsonResult>(result);
                var okObjectResult = jsonResult.Value as OkObjectResult;
                Assert.NotNull(okObjectResult);

                var customers = Assert.IsType<List<Customer>>(okObjectResult.Value);
                Assert.Empty(customers); // An empty list should be returned when the database is empty
            }
        }

        #endregion
        #region GetByID Tests
        [Fact]
        public void GetByID_ReturnsCustomer_WhenCustomerExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "TestDbForGetByID")
                .Options;

            int testCustomerId = 1;

            using (var context = new ApiContext(options))
            {
                context.Customers.Add(new Customer
                {
                    Id = testCustomerId,
                    Name = "John Doe",
                    Address = "Address 1",
                    DNI = "37333444",
                    Email = "john.doe@example.com",
                    Phone = "4586895522",
                    Mobile = "4526897146",
                    State = "California",
                    City = "Los Angeles"
                });
                context.SaveChanges();
            }

            using (var context = new ApiContext(options))
            {
                var controller = new CustomerController(context);

                // Act
                var result = controller.GetByID(testCustomerId);

                // Assert
                var jsonResult = Assert.IsType<JsonResult>(result);
                var okObjectResult = jsonResult.Value as OkObjectResult;
                Assert.NotNull(okObjectResult);

                var customer = Assert.IsType<Customer>(okObjectResult.Value);
                Assert.Equal(testCustomerId, customer.Id);
            }
        }

        [Fact]
        public void GetByID_ReturnsNotFound_WhenCustomerDoesNotExist_NotEmptyDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "TestDbForGetByIDNotFound")
                .Options;

            int nonExistentCustomerId = 99;

            using (var context = new ApiContext(options))
            {
                context.Customers.Add(new Customer
                {
                    Id = 1,
                    Name = "John Doe",
                    Address = "Address 1",
                    DNI = "37333444",
                    Email = "john.doe@example.com",
                    Phone = "4586895522",
                    Mobile = "4526897146",
                    State = "California",
                    City = "Los Angeles"
                });
                context.SaveChanges();
            }

            using (var context = new ApiContext(options))
            {
                var controller = new CustomerController(context);

                // Act
                var result = controller.GetByID(nonExistentCustomerId);

                // Assert
                var jsonResult = Assert.IsType<JsonResult>(result);
                Assert.IsType<NotFoundResult>(jsonResult.Value);
            }
        }

        [Fact]
        public void GetByID_ReturnsNotFound_WhenCustomerDoesNotExist_EmptyDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "TestDbForGetByIDNotFound")
                .Options;

            int nonExistentCustomerId = 99;

            using (var context = new ApiContext(options))
            {
                // No customers added to keep the database empty
                context.SaveChanges();
            }

            using (var context = new ApiContext(options))
            {
                var controller = new CustomerController(context);

                // Act
                var result = controller.GetByID(nonExistentCustomerId);

                // Assert
                var jsonResult = Assert.IsType<JsonResult>(result);
                Assert.IsType<NotFoundResult>(jsonResult.Value);
            }
        }
        #endregion
        #region GetByDNI Tests
        [Fact]
        public void GetByDNI_ReturnsCustomer_WhenCustomerExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "TestDbForGetByDNIExists")
                .Options;

            string testCustomerDNI = "37333444";

            using (var context = new ApiContext(options))
            {
                context.Customers.Add(new Customer
                {
                    Id = 1,
                    Name = "John Doe",
                    Address = "Address 1",
                    DNI = testCustomerDNI,
                    Email = "john.doe@example.com",
                    Phone = "4586895522",
                    Mobile = "4526897146",
                    State = "California",
                    City = "Los Angeles"
                });
                context.SaveChanges();
            }

            using (var context = new ApiContext(options))
            {
                var controller = new CustomerController(context);

                // Act
                var result = controller.GetByDNI(testCustomerDNI);

                // Assert
                var jsonResult = Assert.IsType<JsonResult>(result);
                var okObjectResult = Assert.IsType<OkObjectResult>(jsonResult.Value); // I want to ensure the inner value is OkObjectResult
                var customers = Assert.IsAssignableFrom<IEnumerable<Customer>>(okObjectResult.Value);
                Assert.Contains(customers, customer => customer.DNI == testCustomerDNI);
            }
        }

        [Fact]
        public void GetByDNI_ReturnsNotFound_WhenCustomerDoesNotExist_AndDBIsEmpty()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "TestDbForGetByDNIEmptyDB")
                .Options;

            string nonExistentCustomerDNI = "99999999";

            using (var context = new ApiContext(options))
            {
                // No customers added, so the database remains empty
            }

            using (var context = new ApiContext(options))
            {
                var controller = new CustomerController(context);

                // Act
                var result = controller.GetByDNI(nonExistentCustomerDNI);

                // Assert
                var jsonResult = Assert.IsType<JsonResult>(result);
                Assert.IsType<NotFoundResult>(jsonResult.Value);
            }
        }

        [Fact]
        public void GetByDNI_ReturnsNotFound_WhenCustomerDoesNotExist_AndDBIsNotEmpty()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "TestDbForGetByDNIWithOtherCustomers")
                .Options;

            string nonExistentCustomerDNI = "99999999";

            using (var context = new ApiContext(options))
            {
                context.Customers.Add(new Customer
                {
                    Id = 1,
                    Name = "Jane Doe",
                    Address = "Address 2",
                    DNI = "37333445",
                    Email = "jane.doe@example.com",
                    Phone = "4586895523",
                    Mobile = "4526897147",
                    State = "New York",
                    City = "New York City"
                });
                context.SaveChanges();
            }

            using (var context = new ApiContext(options))
            {
                var controller = new CustomerController(context);

                // Act
                var result = controller.GetByDNI(nonExistentCustomerDNI);

                // Assert
                var jsonResult = Assert.IsType<JsonResult>(result);
                Assert.IsType<NotFoundResult>(jsonResult.Value);
            }
        }
        #endregion
        #region GetByName Tests

        [Fact]
        public void GetByName_ReturnsCustomers_WhenCustomersExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "TestDbForGetByNameExists")
                .Options;

            string testName = "John Doe";

            using (var context = new ApiContext(options))
            {
                context.Customers.Add(new Customer
                {
                    Id = 1,
                    Name = testName,
                    Address = "Address 1",
                    DNI = "12345678",
                    Email = "john.doe1@example.com",
                    Phone = "1111111111",
                    Mobile = "2222222222",
                    State = "California",
                    City = "Los Angeles"
                });
                context.Customers.Add(new Customer
                {
                    Id = 2,
                    Name = testName,
                    Address = "Address 2",
                    DNI = "87654321",
                    Email = "john.doe2@example.com",
                    Phone = "3333333333",
                    Mobile = "4444444444",
                    State = "California",
                    City = "San Francisco"
                });
                context.SaveChanges();
            }

            using (var context = new ApiContext(options))
            {
                var controller = new CustomerController(context);

                // Act
                var result = controller.GetByName(testName);

                // Assert
                var jsonResult = Assert.IsType<JsonResult>(result);
                var okObjectResult = Assert.IsType<OkObjectResult>(jsonResult.Value);
                var customers = Assert.IsAssignableFrom<IEnumerable<Customer>>(okObjectResult.Value);
                Assert.NotEmpty(customers);
                Assert.All(customers, customer => Assert.Equal(testName, customer.Name));
            }
        }

        [Fact]
        public void GetByName_ReturnsNotFound_WhenCustomersDoNotExist_AndDBIsEmpty()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "TestDbForGetByNameEmptyDB")
                .Options;

            string nonExistentName = "Nonexistent Name";

            using (var context = new ApiContext(options))
            {
                // No customers added, so the database remains empty
            }

            using (var context = new ApiContext(options))
            {
                var controller = new CustomerController(context);

                // Act
                var result = controller.GetByName(nonExistentName);

                // Assert
                var jsonResult = Assert.IsType<JsonResult>(result);
                Assert.IsType<NotFoundResult>(jsonResult.Value);
            }
        }

        [Fact]
        public void GetByName_ReturnsNotFound_WhenCustomersDoNotExist_AndDBIsNotEmpty()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "TestDbForGetByNameWithOtherCustomers")
                .Options;

            string nonExistentName = "Nonexistent Name";

            using (var context = new ApiContext(options))
            {
                context.Customers.Add(new Customer
                {
                    Id = 1,
                    Name = "Jane Doe",
                    Address = "Address 1",
                    DNI = "87654321",
                    Email = "jane.doe@example.com",
                    Phone = "5555555555",
                    Mobile = "6666666666",
                    State = "New York",
                    City = "New York City"
                });
                context.SaveChanges();
            }

            using (var context = new ApiContext(options))
            {
                var controller = new CustomerController(context);

                // Act
                var result = controller.GetByName(nonExistentName);

                // Assert
                var jsonResult = Assert.IsType<JsonResult>(result);
                Assert.IsType<NotFoundResult>(jsonResult.Value);
            }
        }

        #endregion
        #region GetByState Tests

        [Fact]
        public void GetByState_ReturnsCustomers_WhenCustomersExistInState()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "TestDbForGetByStateExists")
                .Options;

            string testState = "California";

            using (var context = new ApiContext(options))
            {
                context.Customers.Add(new Customer
                {
                    Id = 1,
                    Name = "John Doe",
                    Address = "Address 1",
                    DNI = "12345678",
                    Email = "john.doe@example.com",
                    Phone = "1111111111",
                    Mobile = "2222222222",
                    State = testState,
                    City = "Los Angeles"
                });
                context.Customers.Add(new Customer
                {
                    Id = 2,
                    Name = "Jane Doe",
                    Address = "Address 2",
                    DNI = "87654321",
                    Email = "jane.doe@example.com",
                    Phone = "3333333333",
                    Mobile = "4444444444",
                    State = testState,
                    City = "San Francisco"
                });
                context.SaveChanges();
            }

            using (var context = new ApiContext(options))
            {
                var controller = new CustomerController(context);

                // Act
                var result = controller.GetByState(testState);

                // Assert
                var jsonResult = Assert.IsType<JsonResult>(result);
                var okObjectResult = Assert.IsType<OkObjectResult>(jsonResult.Value);
                var customers = Assert.IsAssignableFrom<IEnumerable<Customer>>(okObjectResult.Value);
                Assert.NotEmpty(customers);
                Assert.All(customers, customer => Assert.Equal(testState, customer.State));
            }
        }

        [Fact]
        public void GetByState_ReturnsNotFound_WhenNoCustomersInState_AndDBIsEmpty()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "TestDbForGetByStateEmptyDB")
                .Options;

            string nonExistentState = "Nonexistent State";

            using (var context = new ApiContext(options))
            {
                // No customers added, so the database remains empty
            }

            using (var context = new ApiContext(options))
            {
                var controller = new CustomerController(context);

                // Act
                var result = controller.GetByState(nonExistentState);

                // Assert
                var jsonResult = Assert.IsType<JsonResult>(result);
                Assert.IsType<NotFoundResult>(jsonResult.Value);
            }
        }

        [Fact]
        public void GetByState_ReturnsNotFound_WhenNoCustomersInState_AndDBIsNotEmpty()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "TestDbForGetByStateWithOtherStates")
                .Options;

            string nonExistentState = "Nonexistent State";

            using (var context = new ApiContext(options))
            {
                context.Customers.Add(new Customer
                {
                    Id = 1,
                    Name = "Jane Doe",
                    Address = "Address 1",
                    DNI = "87654321",
                    Email = "jane.doe@example.com",
                    Phone = "5555555555",
                    Mobile = "6666666666",
                    State = "New York",
                    City = "New York City"
                });
                context.SaveChanges();
            }

            using (var context = new ApiContext(options))
            {
                var controller = new CustomerController(context);

                // Act
                var result = controller.GetByState(nonExistentState);

                // Assert
                var jsonResult = Assert.IsType<JsonResult>(result);
                Assert.IsType<NotFoundResult>(jsonResult.Value);
            }
        }

        #endregion
        #region GetByCity Tests

        [Fact]
        public void GetByCity_ReturnsCustomers_WhenCustomersExistInCity()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "TestDbForGetByCityExists")
                .Options;

            string testCity = "Los Angeles";

            using (var context = new ApiContext(options))
            {
                context.Customers.Add(new Customer
                {
                    Id = 1,
                    Name = "John Doe",
                    Address = "Address 1",
                    DNI = "12345678",
                    Email = "john.doe@example.com",
                    Phone = "1111111111",
                    Mobile = "2222222222",
                    State = "California",
                    City = testCity
                });
                context.Customers.Add(new Customer
                {
                    Id = 2,
                    Name = "Jane Doe",
                    Address = "Address 2",
                    DNI = "87654321",
                    Email = "jane.doe@example.com",
                    Phone = "3333333333",
                    Mobile = "4444444444",
                    State = "California",
                    City = testCity
                });
                context.SaveChanges();
            }

            using (var context = new ApiContext(options))
            {
                var controller = new CustomerController(context);

                // Act
                var result = controller.GetByCity(testCity);

                // Assert
                var jsonResult = Assert.IsType<JsonResult>(result);
                var okObjectResult = Assert.IsType<OkObjectResult>(jsonResult.Value);
                var customers = Assert.IsAssignableFrom<IEnumerable<Customer>>(okObjectResult.Value);
                Assert.NotEmpty(customers);
                Assert.All(customers, customer => Assert.Equal(testCity, customer.City));
            }
        }

        [Fact]
        public void GetByCity_ReturnsNotFound_WhenNoCustomersInCity_AndDBIsEmpty()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "TestDbForGetByCityEmptyDB")
                .Options;

            string nonExistentCity = "Nonexistent City";

            using (var context = new ApiContext(options))
            {
                // No customers added, so the database remains empty
            }

            using (var context = new ApiContext(options))
            {
                var controller = new CustomerController(context);

                // Act
                var result = controller.GetByCity(nonExistentCity);

                // Assert
                var jsonResult = Assert.IsType<JsonResult>(result);
                Assert.IsType<NotFoundResult>(jsonResult.Value);
            }
        }

        [Fact]
        public void GetByCity_ReturnsNotFound_WhenNoCustomersInCity_AndDBIsNotEmpty()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "TestDbForGetByCityWithOtherCities")
                .Options;

            string nonExistentCity = "Nonexistent City";

            using (var context = new ApiContext(options))
            {
                context.Customers.Add(new Customer
                {
                    Id = 1,
                    Name = "Jane Doe",
                    Address = "Address 1",
                    DNI = "87654321",
                    Email = "jane.doe@example.com",
                    Phone = "5555555555",
                    Mobile = "6666666666",
                    State = "New York",
                    City = "New York City"
                });
                context.SaveChanges();
            }

            using (var context = new ApiContext(options))
            {
                var controller = new CustomerController(context);

                // Act
                var result = controller.GetByCity(nonExistentCity);

                // Assert
                var jsonResult = Assert.IsType<JsonResult>(result);
                Assert.IsType<NotFoundResult>(jsonResult.Value);
            }
        }

        #endregion
        #region GetByEmail Tests

        [Fact]
        public void GetByEmail_ReturnsCustomer_WhenCustomerExistsWithEmail()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "TestDbForGetByEmailExists")
                .Options;

            string testEmail = "john.doe@example.com";

            using (var context = new ApiContext(options))
            {
                context.Customers.Add(new Customer
                {
                    Id = 1,
                    Name = "John Doe",
                    Address = "Address 1",
                    DNI = "12345678",
                    Email = testEmail,
                    Phone = "1111111111",
                    Mobile = "2222222222",
                    State = "California",
                    City = "Los Angeles"
                });
                context.SaveChanges();
            }

            using (var context = new ApiContext(options))
            {
                var controller = new CustomerController(context);

                // Act
                var result = controller.GetByEmail(testEmail);

                // Assert
                var jsonResult = Assert.IsType<JsonResult>(result);
                var okObjectResult = Assert.IsType<OkObjectResult>(jsonResult.Value);
                var customers = Assert.IsAssignableFrom<IEnumerable<Customer>>(okObjectResult.Value);
                Assert.Single(customers); // Note to Leo: Since I'm assuming the email should be unique, I'm expecting a single customer
                var customer = customers.First();
                Assert.Equal(testEmail, customer.Email);
            }
        }

        [Fact]
        public void GetByEmail_ReturnsNotFound_WhenNoCustomerWithEmail_AndDBIsEmpty()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "TestDbForGetByEmailEmptyDB")
                .Options;

            string nonExistentEmail = "nonexistent.email@example.com";

            using (var context = new ApiContext(options))
            {
                // No customers added, so the database remains empty
            }

            using (var context = new ApiContext(options))
            {
                var controller = new CustomerController(context);

                // Act
                var result = controller.GetByEmail(nonExistentEmail);

                // Assert
                var jsonResult = Assert.IsType<JsonResult>(result);
                Assert.IsType<NotFoundResult>(jsonResult.Value);
            }
        }

        [Fact]
        public void GetByEmail_ReturnsNotFound_WhenNoCustomerWithEmail_AndDBIsNotEmpty()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "TestDbForGetByEmailWithOtherEmails")
                .Options;

            string nonExistentEmail = "nonexistent.email@example.com";

            using (var context = new ApiContext(options))
            {
                context.Customers.Add(new Customer
                {
                    Id = 1,
                    Name = "Jane Doe",
                    Address = "Address 1",
                    DNI = "87654321",
                    Email = "jane.doe@example.com",
                    Phone = "5555555555",
                    Mobile = "6666666666",
                    State = "New York",
                    City = "New York City"
                });
                context.SaveChanges();
            }

            using (var context = new ApiContext(options))
            {
                var controller = new CustomerController(context);

                // Act
                var result = controller.GetByEmail(nonExistentEmail);

                // Assert
                var jsonResult = Assert.IsType<JsonResult>(result);
                Assert.IsType<NotFoundResult>(jsonResult.Value);
            }
        }

        #endregion
        #region GetByPhoneNumber Tests

        [Fact]
        public void GetByPhoneNumber_ReturnsCustomers_WhenCustomersExistWithPhoneNumber()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "TestDbForGetByPhoneNumberExists")
                .Options;

            string testPhoneNumber = "1111111111";

            using (var context = new ApiContext(options))
            {
                // Adding a customer with the test phone number as both their phone and mobile numbers
                context.Customers.Add(new Customer
                {
                    Id = 1,
                    Name = "John Doe",
                    Address = "Address 1",
                    DNI = "12345678",
                    Email = "john.doe@example.com",
                    Phone = testPhoneNumber,
                    Mobile = testPhoneNumber,
                    State = "California",
                    City = "Los Angeles"
                });
                context.SaveChanges();
            }

            using (var context = new ApiContext(options))
            {
                var controller = new CustomerController(context);

                // Act
                var result = controller.GetByPhoneNumber(testPhoneNumber);

                // Assert
                var jsonResult = Assert.IsType<JsonResult>(result);
                var okObjectResult = Assert.IsType<OkObjectResult>(jsonResult.Value);
                var customers = Assert.IsAssignableFrom<IEnumerable<Customer>>(okObjectResult.Value);
                Assert.NotEmpty(customers);
                Assert.Contains(customers, customer => customer.Phone == testPhoneNumber || customer.Mobile == testPhoneNumber);
            }
        }

        [Fact]
        public void GetByPhoneNumber_ReturnsNotFound_WhenNoCustomersWithPhoneNumber_AndDBIsEmpty()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "TestDbForGetByPhoneNumberEmptyDB")
                .Options;

            string nonExistentPhoneNumber = "9999999999";

            using (var context = new ApiContext(options))
            {
                // No customers added, so the database remains empty
            }

            using (var context = new ApiContext(options))
            {
                var controller = new CustomerController(context);

                // Act
                var result = controller.GetByPhoneNumber(nonExistentPhoneNumber);

                // Assert
                var jsonResult = Assert.IsType<JsonResult>(result);
                Assert.IsType<NotFoundResult>(jsonResult.Value);
            }
        }

        [Fact]
        public void GetByPhoneNumber_ReturnsNotFound_WhenNoCustomersWithPhoneNumber_AndDBIsNotEmpty()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "TestDbForGetByPhoneNumberWithOtherNumbers")
                .Options;

            string nonExistentPhoneNumber = "9999999999";

            using (var context = new ApiContext(options))
            {
                context.Customers.Add(new Customer
                {
                    Id = 1,
                    Name = "Jane Doe",
                    Address = "Address 1",
                    DNI = "87654321",
                    Email = "jane.doe@example.com",
                    Phone = "2222222222",
                    Mobile = "3333333333",
                    State = "New York",
                    City = "New York City"
                });
                context.SaveChanges();
            }

            using (var context = new ApiContext(options))
            {
                var controller = new CustomerController(context);

                // Act
                var result = controller.GetByPhoneNumber(nonExistentPhoneNumber);

                // Assert
                var jsonResult = Assert.IsType<JsonResult>(result);
                Assert.IsType<NotFoundResult>(jsonResult.Value);
            }
        }

        [Fact]
        public void GetByPhoneNumber_ReturnsCustomers_WhenCustomersExistWithDistinctPhoneAndMobileNumbers()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "TestDbForGetByPhoneNumberDistinct")
                .Options;

            string testPhone = "1111111111";
            string testMobile = "2222222222";

            using (var context = new ApiContext(options))
            {
                context.Customers.Add(new Customer
                {
                    Id = 1,
                    Name = "John Doe",
                    Address = "Address 1",
                    DNI = "12345678",
                    Email = "john.doe@example.com",
                    Phone = testPhone,
                    Mobile = testMobile, // Distinct mobile number
                    State = "California",
                    City = "Los Angeles"
                });
                context.SaveChanges();
            }

            using (var context = new ApiContext(options))
            {
                var controller = new CustomerController(context);

                // Act (Phone)
                var resultPhone = controller.GetByPhoneNumber(testPhone);

                // Assert (Phone)
                var jsonResultPhone = Assert.IsType<JsonResult>(resultPhone);
                var okObjectResultPhone = Assert.IsType<OkObjectResult>(jsonResultPhone.Value);
                var customersPhone = Assert.IsAssignableFrom<IEnumerable<Customer>>(okObjectResultPhone.Value);
                Assert.Single(customersPhone);
                Assert.Contains(customersPhone, customer => customer.Phone == testPhone);

                // Act (Mobile)
                var resultMobile = controller.GetByPhoneNumber(testMobile);

                // Assert (Mobile)
                var jsonResultMobile = Assert.IsType<JsonResult>(resultMobile);
                var okObjectResultMobile = Assert.IsType<OkObjectResult>(jsonResultMobile.Value);
                var customersMobile = Assert.IsAssignableFrom<IEnumerable<Customer>>(okObjectResultMobile.Value);
                Assert.Single(customersMobile);
                Assert.Contains(customersMobile, customer => customer.Mobile == testMobile);
            }
        }

        [Fact]
        public void GetByPhoneNumber_ReturnsNotFound_WhenNoCustomersWithDistinctPhoneAndMobile_AndDBIsEmpty()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "TestDbForDistinctPhoneAndMobileEmptyDB")
                .Options;

            string nonExistentPhone = "9999999999";
            string nonExistentMobile = "8888888888";

            using (var context = new ApiContext(options))
            {
                // No customers added, database remains empty
            }

            using (var context = new ApiContext(options))
            {
                var controller = new CustomerController(context);

                // Act (Phone)
                var resultPhone = controller.GetByPhoneNumber(nonExistentPhone);

                // Assert (Phone)
                var jsonResultPhone = Assert.IsType<JsonResult>(resultPhone);
                Assert.IsType<NotFoundResult>(jsonResultPhone.Value);

                // Act (Mobile)
                var resultMobile = controller.GetByPhoneNumber(nonExistentMobile);

                // Assert (Mobile)
                var jsonResultMobile = Assert.IsType<JsonResult>(resultMobile);
                Assert.IsType<NotFoundResult>(jsonResultMobile.Value);
            }
        }

        [Fact]
        public void GetByPhoneNumber_ReturnsNotFound_WhenNoCustomersWithDistinctPhoneAndMobile_AndDBIsNotEmpty()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "TestDbForDistinctPhoneAndMobileWithOtherNumbers")
                .Options;

            string nonExistentPhone = "9999999999";
            string nonExistentMobile = "8888888888";

            using (var context = new ApiContext(options))
            {
                context.Customers.Add(new Customer
                {
                    Id = 1,
                    Name = "Jane Doe",
                    Address = "Address 1",
                    DNI = "87654321",
                    Email = "jane.doe@example.com",
                    Phone = "7777777777", // Different existing phone number
                    Mobile = "6666666666", // Different existing mobile number
                    State = "New York",
                    City = "New York City"
                });
                context.SaveChanges();
            }

            using (var context = new ApiContext(options))
            {
                var controller = new CustomerController(context);

                // Act (Phone)
                var resultPhone = controller.GetByPhoneNumber(nonExistentPhone);

                // Assert (Phone)
                var jsonResultPhone = Assert.IsType<JsonResult>(resultPhone);
                Assert.IsType<NotFoundResult>(jsonResultPhone.Value);

                // Act (Mobile)
                var resultMobile = controller.GetByPhoneNumber(nonExistentMobile);

                // Assert (Mobile)
                var jsonResultMobile = Assert.IsType<JsonResult>(resultMobile);
                Assert.IsType<NotFoundResult>(jsonResultMobile.Value);
            }
        }
        #endregion
        #region GetByAddress Tests

        [Fact]
        public void GetByAddress_ReturnsCustomers_WhenCustomersExistWithAddress()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "TestDbForGetByAddressExists")
                .Options;

            string testAddress = "123 Main St";

            using (var context = new ApiContext(options))
            {
                // Adding customers with the same address
                context.Customers.Add(new Customer
                {
                    Id = 1,
                    Name = "John Doe",
                    Address = testAddress,
                    DNI = "12345678",
                    Email = "john.doe@example.com",
                    Phone = "1111111111",
                    Mobile = "2222222222",
                    State = "California",
                    City = "Los Angeles"
                });
                context.Customers.Add(new Customer
                {
                    Id = 2,
                    Name = "Jane Doe",
                    Address = testAddress,
                    DNI = "87654321",
                    Email = "jane.doe@example.com",
                    Phone = "3333333333",
                    Mobile = "4444444444",
                    State = "California",
                    City = "San Francisco"
                });
                context.SaveChanges();
            }

            using (var context = new ApiContext(options))
            {
                var controller = new CustomerController(context);

                // Act
                var result = controller.GetByAddress(testAddress);

                // Assert
                var jsonResult = Assert.IsType<JsonResult>(result);
                var okObjectResult = Assert.IsType<OkObjectResult>(jsonResult.Value);
                var customers = Assert.IsAssignableFrom<IEnumerable<Customer>>(okObjectResult.Value);
                Assert.NotEmpty(customers);
                Assert.All(customers, customer => Assert.Equal(testAddress, customer.Address));
            }
        }

        [Fact]
        public void GetByAddress_ReturnsNotFound_WhenNoCustomersWithAddress_AndDBIsEmpty()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "TestDbForGetByAddressEmptyDB")
                .Options;

            string nonExistentAddress = "9999 Nonexistent St";

            using (var context = new ApiContext(options))
            {
                // No customers added, so the database remains empty
            }

            using (var context = new ApiContext(options))
            {
                var controller = new CustomerController(context);

                // Act
                var result = controller.GetByAddress(nonExistentAddress);

                // Assert
                var jsonResult = Assert.IsType<JsonResult>(result);
                Assert.IsType<NotFoundResult>(jsonResult.Value);
            }
        }

        [Fact]
        public void GetByAddress_ReturnsNotFound_WhenNoCustomersWithAddress_AndDBIsNotEmpty()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "TestDbForGetByAddressWithOtherAddresses")
                .Options;

            string nonExistentAddress = "9999 Nonexistent St";

            using (var context = new ApiContext(options))
            {
                context.Customers.Add(new Customer
                {
                    Id = 1,
                    Name = "Eve Doe",
                    Address = "456 Another St",
                    DNI = "11223344",
                    Email = "eve.doe@example.com",
                    Phone = "5555555555",
                    Mobile = "6666666666",
                    State = "New York",
                    City = "New York City"
                });
                context.SaveChanges();
            }

            using (var context = new ApiContext(options))
            {
                var controller = new CustomerController(context);

                // Act
                var result = controller.GetByAddress(nonExistentAddress);

                // Assert
                var jsonResult = Assert.IsType<JsonResult>(result);
                Assert.IsType<NotFoundResult>(jsonResult.Value);
            }
        }

        #endregion
        #region CreateEdit Tests

        [Fact]
        public void CreateEdit_CreatesNewCustomer_WhenIdIsZero()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "TestDbForCreateNewCustomer")
                .Options;

            var newCustomer = new Customer
            {
                Name = "New Customer",
                Address = "New Address",
                DNI = "NewDNI123",
                Email = "new.customer@example.com",
                Phone = "1234567890",
                Mobile = "0987654321",
                State = "NewState",
                City = "NewCity"
            };

            using (var context = new ApiContext(options))
            {
                var controller = new CustomerController(context);

                // Act
                var result = controller.CreateEdit(newCustomer);

                // Assert
                var jsonResult = Assert.IsType<JsonResult>(result);
                var okObjectResult = Assert.IsType<OkObjectResult>(jsonResult.Value);
                var createdCustomer = Assert.IsType<Customer>(okObjectResult.Value);

                // Asserting that the customer's ID is set (it should be auto-incremented by EF)
                Assert.True(createdCustomer.Id > 0);
                // Asserting each property to ensure that they're correct
                Assert.Equal(newCustomer.Name, createdCustomer.Name);
                Assert.Equal(newCustomer.Address, createdCustomer.Address);
                Assert.Equal(newCustomer.DNI, createdCustomer.DNI);
                Assert.Equal(newCustomer.Email, createdCustomer.Email);
                Assert.Equal(newCustomer.Phone, createdCustomer.Phone);
                Assert.Equal(newCustomer.Mobile, createdCustomer.Mobile);
                Assert.Equal(newCustomer.State, createdCustomer.State);
                Assert.Equal(newCustomer.City, createdCustomer.City);

                // I verify that the customer was added to the database
                var dbCustomer = context.Customers.Find(createdCustomer.Id);
                Assert.NotNull(dbCustomer);
                Assert.Equal(newCustomer.Name, dbCustomer.Name);
                Assert.Equal(newCustomer.Address, dbCustomer.Address);
                Assert.Equal(newCustomer.DNI, dbCustomer.DNI);
                Assert.Equal(newCustomer.Email, dbCustomer.Email);
                Assert.Equal(newCustomer.Phone, dbCustomer.Phone);
                Assert.Equal(newCustomer.Mobile, dbCustomer.Mobile);
                Assert.Equal(newCustomer.State, dbCustomer.State);
                Assert.Equal(newCustomer.City, dbCustomer.City);
            }
        }

        [Fact]
        public void CreateEdit_UpdatesExistingCustomer_WhenIdIsNonZero()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "TestDbForUpdateCustomer")
                .Options;

            var existingCustomer = new Customer
            {
                Name = "Existing Customer",
                Address = "Existing Address",
                DNI = "ExistingDNI123",
                Email = "existing.customer@example.com",
                Phone = "1234567890",
                Mobile = "0987654321",
                State = "ExistingState",
                City = "ExistingCity"
            };

            using (var context = new ApiContext(options))
            {
                context.Customers.Add(existingCustomer);
                context.SaveChanges();
            }

            // Change properties for the update
            existingCustomer.Name = "Updated Name";
            existingCustomer.Address = "Updated Address";
            existingCustomer.DNI = "UpdatedDNI456";
            existingCustomer.Email = "updated.email@example.com";
            existingCustomer.Phone = "9876543210";
            existingCustomer.Mobile = "0123456789";
            existingCustomer.State = "UpdatedState";
            existingCustomer.City = "UpdatedCity";


            using (var context = new ApiContext(options))
            {
                var controller = new CustomerController(context);

                // Act
                var result = controller.CreateEdit(existingCustomer);

                // Assert
                var jsonResult = Assert.IsType<JsonResult>(result);
                var okObjectResult = Assert.IsType<OkObjectResult>(jsonResult.Value);
                var updatedCustomer = Assert.IsType<Customer>(okObjectResult.Value);

                // I'm asserting that the customer's ID remains the same
                Assert.Equal(existingCustomer.Id, updatedCustomer.Id);
                // Asserting if properties were updated
                Assert.Equal("Updated Name", updatedCustomer.Name);
                Assert.Equal("updated.email@example.com", updatedCustomer.Email);
                Assert.Equal("Updated Address", updatedCustomer.Address);
                Assert.Equal("UpdatedDNI456", updatedCustomer.DNI);
                Assert.Equal("9876543210", updatedCustomer.Phone);
                Assert.Equal("0123456789", updatedCustomer.Mobile);
                Assert.Equal("UpdatedState", updatedCustomer.State);
                Assert.Equal("UpdatedCity", updatedCustomer.City);

                // Here I'm checking if the customer's updated details persist in the database
                var dbCustomer = context.Customers.Find(updatedCustomer.Id);
                Assert.NotNull(dbCustomer);
                Assert.Equal("Updated Name", dbCustomer.Name);
                Assert.Equal("updated.email@example.com", dbCustomer.Email);
                Assert.Equal("Updated Name", dbCustomer.Name);
                Assert.Equal("updated.email@example.com", dbCustomer.Email);
                Assert.Equal("Updated Address", dbCustomer.Address);
                Assert.Equal("UpdatedDNI456", dbCustomer.DNI);
                Assert.Equal("9876543210", dbCustomer.Phone);
                Assert.Equal("0123456789", dbCustomer.Mobile);
                Assert.Equal("UpdatedState", dbCustomer.State);
                Assert.Equal("UpdatedCity", dbCustomer.City);
            }
        }

        [Fact]
        public void CreateEdit_ReturnsNotFound_WhenUpdatingNonExistentCustomer()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "TestDbForUpdateNonExistentCustomer")
                .Options;

            var nonExistentCustomer = new Customer
            {
                Id = 999,
                Name = "Non Existing Customer",
                Address = "Test Address",
                DNI = "TestDNI",
                Email = "test.customer@example.com",
                Phone = "1234567890",
                Mobile = "0987654321",
                State = "TestState",
                City = "TestCity"
            };

            using (var context = new ApiContext(options))
            {
                var controller = new CustomerController(context);

                // Act
                var result = controller.CreateEdit(nonExistentCustomer);

                // Assert
                var jsonResult = Assert.IsType<JsonResult>(result);
                Assert.IsType<NotFoundResult>(jsonResult.Value);
            }
        }

        #endregion
        #region Delete Tests

        [Fact]
        public void Delete_RemovesCustomer_WhenCustomerExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "TestDbForDeleteCustomerWithTwoCustomers")
                .Options;

            // Customer to be deleted
            var customerToDelete = new Customer
            {
                Name = "Customer To Delete",
                Address = "123 Delete St",
                DNI = "DeleteDNI123",
                Email = "delete.customer@example.com",
                Phone = "1111111111",
                Mobile = "2222222222",
                State = "DeleteState",
                City = "DeleteCity"
            };

            // Another customer who should NOT be deleted from the database
            var customerToKeep = new Customer
            {
                Name = "Customer To Keep",
                Address = "456 Keep Ave",
                DNI = "KeepDNI456",
                Email = "keep.customer@example.com",
                Phone = "3333333333",
                Mobile = "4444444444",
                State = "KeepState",
                City = "KeepCity"
            };

            using (var context = new ApiContext(options))
            {
                context.Customers.AddRange(customerToDelete, customerToKeep);
                context.SaveChanges();
            }

            int customerIdToDelete = customerToDelete.Id;

            using (var context = new ApiContext(options))
            {
                var controller = new CustomerController(context);

                // Act
                var result = controller.Delete(customerIdToDelete);

                // Assert
                var jsonResult = Assert.IsType<JsonResult>(result);
                Assert.IsType<NoContentResult>(jsonResult.Value);

                // Ensure that only the specified customer is removed
                using (var validationContext = new ApiContext(options))
                {
                    Assert.Null(validationContext.Customers.Find(customerIdToDelete));
                    Assert.NotNull(validationContext.Customers.Find(customerToKeep.Id)); // The other customer should still exist
                }
            }
        }

        [Fact]
        public void Delete_ReturnsNotFound_WhenCustomerDoesNotExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "TestDbForDeleteNonExistentCustomerWithTwoCustomers")
                .Options;

            // Adding two customers, but will attempt to delete a non-existent one
            var customer1 = new Customer
            {
                Name = "First Customer",
                Address = "789 First St",
                DNI = "FirstDNI789",
                Email = "first.customer@example.com",
                Phone = "5555555555",
                Mobile = "6666666666",
                State = "FirstState",
                City = "FirstCity"
            };
            var customer2 = new Customer
            {
                Name = "Second Customer",
                Address = "101 Second Ave",
                DNI = "SecondDNI101",
                Email = "second.customer@example.com",
                Phone = "7777777777",
                Mobile = "8888888888",
                State = "SecondState",
                City = "SecondCity"
            };

            using (var context = new ApiContext(options))
            {
                context.Customers.AddRange(customer1, customer2);
                context.SaveChanges();
            }

            int nonExistentCustomerId = 999;

            using (var context = new ApiContext(options))
            {
                var controller = new CustomerController(context);

                // Act
                var result = controller.Delete(nonExistentCustomerId);

                // Assert
                var jsonResult = Assert.IsType<JsonResult>(result);
                Assert.IsType<NotFoundResult>(jsonResult.Value);

                // I want to ensure the existing customers are still in the database
                using (var validationContext = new ApiContext(options))
                {
                    Assert.NotNull(validationContext.Customers.Find(customer1.Id));
                    Assert.NotNull(validationContext.Customers.Find(customer2.Id));
                }
            }
        }

        [Fact]
        public void Delete_ReturnsNotFound_WhenDatabaseIsEmpty()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "TestDbForDeleteFromEmptyDB")
                .Options;

            int nonExistentCustomerId = 1;

            using (var context = new ApiContext(options))
            {
                var controller = new CustomerController(context);

                // Act
                var result = controller.Delete(nonExistentCustomerId);

                // Assert
                var jsonResult = Assert.IsType<JsonResult>(result);
                Assert.IsType<NotFoundResult>(jsonResult.Value);
            }
        }
        #endregion
    }
}
