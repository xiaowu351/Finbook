using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.API.Controllers;
using User.API.Data;
using User.API.Models;
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

        private (AppUserContext, UsersController) CreateUsersController()
        {
            var context = CreateDbContext();
            var loggerMoq = new Mock<ILogger<UsersController>>();
            var logger = loggerMoq.Object;

            return (context, new UsersController(context, logger));
        }

        private async Task UserPropertiesData(AppUserContext context)
        {
            var user = await context.AppUsers.SingleOrDefaultAsync(u => u.Id == 1);
            user.Properties.Add(new UserProperty { Key = "fin_industry", Value = "互联网", Text = "互联网" });
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task Get_ReturnRightUser_With_ExpectedParameters()
        {
            (var context, var controller) = CreateUsersController();

            var response = await controller.Get();

            var result = response.Should().BeOfType<JsonResult>().Subject;
            var appUser = result.Value.Should().BeAssignableTo<AppUser>().Subject;

            appUser.Id.Should().Be(1);
            appUser.Name.Should().Be("test");
        }


        [Fact]
        public async Task Patch_ReturnNewName_WithExpectedNameParamter()
        {
            (var context, var controller) = CreateUsersController();
            var document = new JsonPatchDocument<AppUser>();
            document.Replace(u => u.Name, "zhangsan");
            var response = await controller.Patch(document);

            var result = response.Should().BeOfType<JsonResult>().Subject;

            //1.验证返回结果
            var appUser = result.Value.Should().BeAssignableTo<AppUser>().Subject;
            appUser.Name.Should().Be("zhangsan");

            //2.验证持久化结果
            var dbAppUser = await context.AppUsers.SingleOrDefaultAsync(u => u.Id == appUser.Id);
            dbAppUser.Should().NotBeNull();
            dbAppUser.Name.Should().Be("zhangsan");
        }

        [Fact]
        public async Task Patch_ReturnAddProperties_WithExpectedAddPropertyParamter()
        {
            (var context, var controller) = CreateUsersController();
            var document = new JsonPatchDocument<AppUser>();
            document.Replace(u => u.Properties, new List<UserProperty>() {
                new UserProperty{ Key="fin_industry",Value="互联网",Text="互联网" }
            });
            var response = await controller.Patch(document);

            var result = response.Should().BeOfType<JsonResult>().Subject;

            //1.验证返回结果
            var appUser = result.Value.Should().BeAssignableTo<AppUser>().Subject;
            appUser.Properties.Count.Should().Be(1);
            appUser.Properties.FirstOrDefault().Value.Should().Be("互联网");
            appUser.Properties.FirstOrDefault().Key.Should().Be("fin_industry");

            //2.验证持久化结果
            var dbAppUser = await context.AppUsers.SingleOrDefaultAsync(u => u.Id == appUser.Id);
            dbAppUser.Properties.Count.Should().Be(1);
            dbAppUser.Properties.FirstOrDefault().Value.Should().Be("互联网");
            dbAppUser.Properties.FirstOrDefault().Key.Should().Be("fin_industry");
        }

        [Fact]
        public async Task Patch_ReturnRemoveProperties_WithExpectedRemovePropertyParamter()
        {
            (var context, var controller) = CreateUsersController();
            await UserPropertiesData(context);
            var document = new JsonPatchDocument<AppUser>();
            document.Replace(u => u.Properties, new List<UserProperty>() { 
            });
            var response = await controller.Patch(document);

            var result = response.Should().BeOfType<JsonResult>().Subject;

            //1.验证返回结果
            var appUser = result.Value.Should().BeAssignableTo<AppUser>().Subject;
            appUser.Properties.Should().BeEmpty();
             

            //2.验证持久化结果
            var dbAppUser = await context.AppUsers.SingleOrDefaultAsync(u => u.Id == appUser.Id);
            dbAppUser.Properties.Should().BeEmpty(); 
        }

        [Fact]
        public async Task Patch_ReturnUpdateProperties_WithExpectedUpdatePropertyParamter()
        {
            (var context, var controller) = CreateUsersController();

            await UserPropertiesData(context);

            var document = new JsonPatchDocument<AppUser>();
            document.Replace(u => u.Properties, new List<UserProperty>()
            {
                new UserProperty{ Key="fin_industry",Value="互联网+",Text="互联网+" }
            });
            var response = await controller.Patch(document);

            var result = response.Should().BeOfType<JsonResult>().Subject;

            //1.验证返回结果
            var appUser = result.Value.Should().BeAssignableTo<AppUser>().Subject;
            appUser.Properties.Count.Should().Be(1);
            appUser.Properties.FirstOrDefault().Value.Should().Be("互联网+");
            appUser.Properties.FirstOrDefault().Key.Should().Be("fin_industry");


            //2.验证持久化结果
            var dbAppUser = await context.AppUsers.SingleOrDefaultAsync(u => u.Id == appUser.Id);
            dbAppUser.Properties.Count.Should().Be(1);
            dbAppUser.Properties.FirstOrDefault().Value.Should().Be("互联网+");
            dbAppUser.Properties.FirstOrDefault().Key.Should().Be("fin_industry");
        }
    }
}
