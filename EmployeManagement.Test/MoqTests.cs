using EmployeeManagement.Business;
using EmployeeManagement.DataAccess.Entities;
using EmployeeManagement.DataAccess.Services;
using EmployeeManagement.Services.Test;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeManagement.Test
{
    public class MoqTests
    {
        [Fact]
        public void FetchInternalEmployee_EmployeeFetched_SuggestedBonusMustBeCalculatedCorrectly_MoqInterface()
        {
            // Arrange
            //var employeeManagementTestDataRepository = new EmployeeManagementTestDataRepository();
            var employeeManagementTestDataRepositoryMock = new Mock<IEmployeeManagementRepository>();

            employeeManagementTestDataRepositoryMock
                .Setup(m => m.GetInternalEmployee(It.IsAny<Guid>()))
                .Returns(new InternalEmployee("Tony", "Hall", 2, 2500, false, 2)
                {
                    AttendedCourses = new List<Course>
                    {
                        new Course("A course"),
                        new Course("Another course")
                    }
                });

            //var employeeFactory = new EmployeeFactory();
            var employeeFactoryMock = new Mock<EmployeeFactory>();
            var employeeService = new EmployeeService(
                employeeManagementTestDataRepositoryMock.Object, employeeFactoryMock.Object);

            // Act
            var employee = employeeService.FetchInternalEmployee(
                Guid.Empty);

            // Assert
            Assert.Equal(400, employee.SuggestedBonus);
        }

        [Fact]
        public void CreateInternalEmployee_InternalEmployeeCreated_SuggestedBonusMustBeCalculatedCorrectly_MoqFactory()
        {
            // Arrange
            var employeeManagementTestDataRepository = new EmployeeManagementTestDataRepository();
            var employeeFactoryMock = new Mock<EmployeeFactory>();
            employeeFactoryMock.Setup(
                m => m.CreateEmployee(It.IsAny<string>(), It.IsAny<string>(), null, false))
                .Returns(new InternalEmployee("Huy", "Vo", 5, 3000, false, 1));
            var employeeService = new EmployeeService(
                employeeManagementTestDataRepository, employeeFactoryMock.Object);

            decimal suggestedBonus = 1000;

            // Act
            var employee = employeeService.CreateInternalEmployee("Huy", "Vo");

            // Assert
            Assert.Equal(suggestedBonus, employee.SuggestedBonus);
        }

        [Fact]
        public async Task FetchInternalEmployee_EmployeeFetched_SuggestedBonusMustBeCalculatedCorrectly_MoqAsync()
        {
            // Arrange
            //var employeeManagementTestDataRepository = new EmployeeManagementTestDataRepository();
            var employeeManagementTestDataRepositoryMock = new Mock<IEmployeeManagementRepository>();

            employeeManagementTestDataRepositoryMock
                .Setup(m => m.GetInternalEmployeeAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new InternalEmployee("Tony", "Hall", 2, 2500, false, 2)
                {
                    AttendedCourses = new List<Course>
                    {
                        new Course("A course"),
                        new Course("Another course")
                    }
                });

            //var employeeFactory = new EmployeeFactory();
            var employeeFactoryMock = new Mock<EmployeeFactory>();
            var employeeService = new EmployeeService(
                employeeManagementTestDataRepositoryMock.Object, employeeFactoryMock.Object);

            // Act
            var employee = await employeeService.FetchInternalEmployeeAsync(
                Guid.Empty);

            // Assert
            Assert.Equal(400, employee.SuggestedBonus);
        }
    }
}
