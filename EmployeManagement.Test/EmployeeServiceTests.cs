using EmployeeManagement.Business;
using EmployeeManagement.Business.EventArguments;
using EmployeeManagement.Business.Exceptions;
using EmployeeManagement.DataAccess.Entities;
using EmployeeManagement.Services.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeManagement.Test
{
    public class EmployeeServiceTests
    {
        [Fact]
        public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendedPredefinedObligatoryClass()
        {
            // Arrange
            var employeeManagementTestDataRepository = new EmployeeManagementTestDataRepository();

            var employeeService = new EmployeeService(employeeManagementTestDataRepository,
                new EmployeeFactory());
            var obligatoryCourse = employeeManagementTestDataRepository.
                GetCourse(Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"));

            // Act
            var internalEmployee = employeeService.CreateInternalEmployee("Huy", "Vo");

            // Assert
            Assert.Contains(obligatoryCourse, internalEmployee.AttendedCourses);
        }

        [Fact]
        public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendedPredicateObligatoryClass()
        {
            // Arrange
            var employeeManagementTestDataRepository = new EmployeeManagementTestDataRepository();

            var employeeService = new EmployeeService(employeeManagementTestDataRepository,
                new EmployeeFactory());

            // Act
            var internalEmployee = employeeService.CreateInternalEmployee("Huy", "Vo");

            // Assert
            Assert.Contains(internalEmployee.AttendedCourses,
                course => course.Id == Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"));
        }

        [Fact]
        public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendedMustMatchObligatoryClasses()
        {
            // Arrange
            var employeeManagementTestDataRepository = new EmployeeManagementTestDataRepository();

            var employeeService = new EmployeeService(employeeManagementTestDataRepository,
                new EmployeeFactory());
            var obligatoryClasses = employeeManagementTestDataRepository.GetCourses(
                Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"),
                Guid.Parse("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e"));

            // Act
            var internalEmployee = employeeService.CreateInternalEmployee("Huy", "Vo");

            // Assert
            Assert.Equal(obligatoryClasses, internalEmployee.AttendedCourses);
        }

        [Fact]
        public void CreateInternalEmployee_InternalEmployeeCreated_AttendedCoursesMustNotBeNew()
        {
            // Arrange
            var employeeManagementTestDataRepository = new EmployeeManagementTestDataRepository();

            var employeeService = new EmployeeService(employeeManagementTestDataRepository,
                new EmployeeFactory());


            // Act
            var internalEmployee = employeeService.CreateInternalEmployee("Huy", "Vo");

            // Assert
            Assert.All(internalEmployee.AttendedCourses, course => Assert.False(course.IsNew));
        }

        [Fact]
        public async Task GiveRaise_RaisebelowMinimumGiven_EmployeeInvalidRaiseExceptionMustBeThrown()
        {
            // Arrange
            var employeeService = new EmployeeService(
                new EmployeeManagementTestDataRepository(),
                new EmployeeFactory());
            var internalEmployee = new InternalEmployee("Huy", "Vo", 5, 2500, false, 1);

            // Act & Assert
            await Assert.ThrowsAsync<EmployeeInvalidRaiseException>(
                async () => await employeeService.GiveRaiseAsync(internalEmployee, 50));
        }

        [Fact]
        public void NotifyOfAbsence_EmployeeIsAbsent_OnEmployeeIsAbsentEventMustBeTriggered()
        {
            // Arrange
            var employeeService = new EmployeeService(
                new EmployeeManagementTestDataRepository(),
                new EmployeeFactory());
            var internalEmployee = new InternalEmployee("Huy", "Vo", 5, 3000, false, 1);

            // Act & Assert
            Assert.Raises<EmployeeIsAbsentEventArgs>(
                handler => employeeService.EmployeeIsAbsent += handler,
                handler => employeeService.EmployeeIsAbsent -= handler,
                () => employeeService.NotifyOfAbsence(internalEmployee));
        }
    }
}
