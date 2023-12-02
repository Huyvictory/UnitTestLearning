using EmployeeManagement.Business.EventArguments;
using EmployeeManagement.Business.Exceptions;
using EmployeeManagement.DataAccess.Entities;
using EmployeManagement.Test.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeManagement.Test
{
    [Collection("EmployeeServiceCollection")]
    public class DataDrivenEmployeeServiceTests //: IClassFixture<EmployeeServiceFixture>
    {
        private readonly EmployeeServiceFixture _employeeServiceFixture;

        public DataDrivenEmployeeServiceTests(EmployeeServiceFixture employeeServiceFixture)
        {
            _employeeServiceFixture = employeeServiceFixture;
        }


        [Fact]
        public async Task GiveRaise_RaisebelowMinimumGiven_EmployeeInvalidRaiseExceptionMustBeThrown()
        {
            // Arrange
            var internalEmployee = new InternalEmployee("Huy", "Vo", 5, 3000, false, 1);

            // Act
            await _employeeServiceFixture.EmployeeService.GiveRaiseAsync(internalEmployee, 100);

            // Assert
            Assert.True(internalEmployee.MinimumRaiseGiven);
        }

        [Fact]
        public async Task GiveRaise_RaiseMoreThanMinimumRaiseGiven_EmployeeInvalidRaiseExceptionMustBeThrown()
        {
            // Arrange
            var internalEmployee = new InternalEmployee("Huy", "Vo", 5, 3000, false, 1);

            // Act
            await _employeeServiceFixture.EmployeeService.GiveRaiseAsync(internalEmployee, 200);

            // Assert
            Assert.False(internalEmployee.MinimumRaiseGiven);
        }
    }
}
