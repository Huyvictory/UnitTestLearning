using EmployeeManagement.Business;
using EmployeeManagement.DataAccess.DbContexts;
using EmployeeManagement.DataAccess.Entities;
using EmployeeManagement.DataAccess.Services;
using EmployeeManagement.Services.Test;
using EmployeManagement.Test.HttpMessageHandlers;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace EmployeManagement.Test
{
    public class TestIsolationApproachesTests
    {
        [Fact]
        public async Task AttendCourseAsync_CourseAttended_SuggestedBonusMustCorrectlyReCalculated()
        {
            // Arrange
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            var optionsBuilder = new DbContextOptionsBuilder<EmployeeDbContext>().UseSqlite(connection);

            var dbContextSqlLite = new EmployeeDbContext(optionsBuilder.Options);
            dbContextSqlLite.Database.Migrate();

            var employeeManagementDataRepository = new EmployeeManagementRepository(dbContextSqlLite);

            var employeeService = new EmployeeService(employeeManagementDataRepository, new EmployeeFactory());

            // Get course from databse - "Dealing with Customers 101"
            var courseToAttend = await employeeManagementDataRepository
                .GetCourseAsync(Guid.Parse("844e14ce-c055-49e9-9610-855669c9859b"));

            // Get exisiting employee - "Megan Jones"
            var internalEmployee = await employeeManagementDataRepository
                .GetInternalEmployeeAsync(Guid.Parse("72f2f5fe-e50c-4966-8420-d50258aefdcb"));

            if (courseToAttend == null || internalEmployee == null)
            {
                throw new XunitException("Arrange the test failed!");
            }

            // expected suggest bonus after internal employee has attended a new course
            var expectedSuggestBonus = internalEmployee.YearsInService *
                (internalEmployee.AttendedCourses.Count + 1) * 100;
            // Act
            await employeeService.AttendCourseAsync(internalEmployee, courseToAttend);

            // Assert
            Assert.Equal(expectedSuggestBonus, internalEmployee.SuggestedBonus);
        }

        [Fact]
        public async Task PromoteInternalEmployeeAsync_IsEligible_JobLevelMustBeIncreased()
        {
            // Arrange
            var httpClient = new HttpClient(
                new TestablePromotionEligibilityHandler(true));
            var internalEmployee = new InternalEmployee("Huy", "Vo", 5, 3000, false, 1);

            var promotionService = new PromotionService(httpClient,
                new EmployeeManagementTestDataRepository());

            // Act
            await promotionService.PromoteInternalEmployeeAsync(internalEmployee);

            // Assert
            Assert.Equal(2, internalEmployee.JobLevel);
        }
    }
}
