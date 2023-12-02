using EmployeManagement.Test.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeManagement.Test
{
    public class EmployeeServiceTestsWithAspNetCoreDI : IClassFixture<EmployeeServiceWithAspNetCoreDIFixture>
    {
        private readonly EmployeeServiceWithAspNetCoreDIFixture _employeeServiceWithAspNetCoreDIFixture;

        public EmployeeServiceTestsWithAspNetCoreDI(EmployeeServiceWithAspNetCoreDIFixture employeeServiceFixture)
        {
            _employeeServiceWithAspNetCoreDIFixture = employeeServiceFixture;
        }

        [Fact]
        public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendedPredicateObligatoryClass()
        {
            // Arrange
            // Act
            var internalEmployee = _employeeServiceWithAspNetCoreDIFixture.EmployeeService.CreateInternalEmployee("Huy", "Vo");

            // Assert
            Assert.Contains(internalEmployee.AttendedCourses,
                course => course.Id == Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"));
        }
    }
}
