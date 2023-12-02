using EmployeeManagement.Business;
using EmployeeManagement.Business.EventArguments;
using EmployeeManagement.Business.Exceptions;
using EmployeeManagement.DataAccess.Entities;
using EmployeeManagement.Services.Test;
using EmployeManagement.Test.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace EmployeManagement.Test
{
    [Collection("EmployeeServiceCollection")]
    public class EmployeeServiceTests //: IClassFixture<EmployeeServiceFixture>
    {
        private readonly EmployeeServiceFixture _employeeServiceFixture;
        private readonly ITestOutputHelper _testOutputHelper;

        public EmployeeServiceTests(EmployeeServiceFixture employeeServiceFixture, ITestOutputHelper testOutputHelper)
        {
            _employeeServiceFixture = employeeServiceFixture;
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendedPredefinedObligatoryClass()
        {
            // Arrange
            var obligatoryCourse = _employeeServiceFixture.EmployeeManagementTestDataRepository.
                GetCourse(Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"));

            // Act
            var internalEmployee = _employeeServiceFixture.EmployeeService.CreateInternalEmployee("Huy", "Vo");

            //Log test output to test explorer
            _testOutputHelper.WriteLine($"Employee after creation: {internalEmployee.FirstName} - {internalEmployee.LastName}");

            internalEmployee.AttendedCourses.ForEach((course) => _testOutputHelper.WriteLine($"Attended course: {course.Title}"));

            // Assert
            Assert.Contains(obligatoryCourse, internalEmployee.AttendedCourses);
        }

        [Fact]
        public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendedPredicateObligatoryClass()
        {
            // Arrange
            // Act
            var internalEmployee = _employeeServiceFixture.EmployeeService.CreateInternalEmployee("Huy", "Vo");

            // Assert
            Assert.Contains(internalEmployee.AttendedCourses,
                course => course.Id == Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"));
        }

        [Fact]
        public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendedMustMatchObligatoryClasses()
        {
            // Arrange
            var obligatoryClasses = _employeeServiceFixture.EmployeeManagementTestDataRepository.GetCourses(
                Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"),
                Guid.Parse("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e"));

            // Act
            var internalEmployee = _employeeServiceFixture.EmployeeService.CreateInternalEmployee("Huy", "Vo");

            // Assert
            Assert.Equal(obligatoryClasses, internalEmployee.AttendedCourses);
        }

        [Fact]
        public void CreateInternalEmployee_InternalEmployeeCreated_AttendedCoursesMustNotBeNew()
        {
            // Arrange
            // Act
            var internalEmployee = _employeeServiceFixture.EmployeeService.CreateInternalEmployee("Huy", "Vo");

            // Assert
            Assert.All(internalEmployee.AttendedCourses, course => Assert.False(course.IsNew));
        }

        [Fact]
        public async Task GiveRaise_RaisebelowMinimumGiven_EmployeeInvalidRaiseExceptionMustBeThrown()
        {
            // Arrange
            var internalEmployee = new InternalEmployee("Huy", "Vo", 5, 3000, false, 1);

            // Act & Assert
            await Assert.ThrowsAsync<EmployeeInvalidRaiseException>(
                async () => await _employeeServiceFixture.EmployeeService.GiveRaiseAsync(internalEmployee, 50));
        }

        [Fact]
        public void NotifyOfAbsence_EmployeeIsAbsent_OnEmployeeIsAbsentEventMustBeTriggered()
        {
            // Arrange
            var internalEmployee = new InternalEmployee("Huy", "Vo", 5, 3000, false, 1);

            // Act & Assert
            Assert.Raises<EmployeeIsAbsentEventArgs>(
                handler => _employeeServiceFixture.EmployeeService.EmployeeIsAbsent += handler,
                handler => _employeeServiceFixture.EmployeeService.EmployeeIsAbsent -= handler,
                () => _employeeServiceFixture.EmployeeService.NotifyOfAbsence(internalEmployee));
        }
    }
}
