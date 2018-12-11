using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using User.API.Controllers;
using User.API.Data;
using Xunit;

namespace User.API.UnitTests
{
    public class UserControllUnitTests
    {
        private AppUserContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<AppUserContext>()
                            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                            .Options;
            var userContext = new AppUserContext(options);

            userContext.AppUsers.Add(new Models.AppUser { Id = 1, Name = "test" });
            userContext.SaveChanges();

            return userContext;
        }

        [Fact]
        public async Task Get_ReturnRightUser_With_ExpectedParameters()
        {
            var context = CreateDbContext();
            var loggerMoq = new Mock<ILogger<UsersController>>();
            var logger = loggerMoq.Object;

            var controller = new UsersController(context, logger);

            var response = await controller.Get();

            Assert.IsType<JsonResult>(response);


        }
    }
}
