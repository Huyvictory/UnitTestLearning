using EmployeeManagement.Business;
using EmployeeManagement.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeManagement.Test
{
    public class EmployeeFactoryTests : IDisposable
    {
        public EmployeeFactory _employeeFactory;
        public EmployeeFactoryTests()
        {
            _employeeFactory = new EmployeeFactory();
        }

        public void Dispose()
        {

        }

        [Fact]
        [Trait("Category", "Employee_CreateEmployee_Salary")]
        public void CreateEmployee_ConstructInternalEmployee_IsInternalEmployee()
        {
            var isInternalEmployee = (InternalEmployee)_employeeFactory.CreateEmployee("Huy", "Vo");

            Assert.NotNull(isInternalEmployee);
        }

        [Fact]
        [Trait("Category", "Employee_CreateEmployee_Salary")]
        public void CreateEmployee_EnsureBaseSalary2500()
        {
            var InternalEmployee = (InternalEmployee)_employeeFactory.CreateEmployee("Huy", "Vo");

            Assert.Equal(2500, InternalEmployee.Salary);
        }

        [Fact]
        [Trait("Category", "Employee_CreateEmployee_Salary")]
        public void CreateEmployee_ConstructInternalEmployee_SalaryMustBeBetween2500And3500()
        {
            // Arrange
            // Act
            var InternalEmployee = (InternalEmployee)_employeeFactory.CreateEmployee("Huy", "Vo");

            // Assert
            Assert.True(InternalEmployee.Salary >= 2500 && InternalEmployee.Salary <= 3500, "Salary not in predfined range");
        }

        [Fact]
        [Trait("Category", "Employee_CreateEmployee_Salary")]
        public void CreateEmployee_ConstructInternalEmployee_SalaryMustBeInRange2500And3500()
        {
            // Arrange
            // Act
            var InternalEmployee = (InternalEmployee)_employeeFactory.CreateEmployee("Huy", "Vo");

            // Assert
            Assert.InRange(InternalEmployee.Salary, 2500, 3500);
        }

        [Fact]
        [Trait("Category", "Employee_CreateEmployee_Salary")]
        public void CreateEmployee_EnsureBaseSalary2500_Precision()
        {
            var InternalEmployee = (InternalEmployee)_employeeFactory.CreateEmployee("Huy", "Vo");
            InternalEmployee.Salary = 2500.123m;

            Assert.Equal(2500, InternalEmployee.Salary, 0);
        }

        [Fact]
        [Trait("Category", "Employee_CreateEmployee_ReturnType")]
        public void CreateEmployee_IsExternalIsTrue_ReturnTypeMustBeExternalEmployee()
        {
            // Arrange
            // Act
            var employee = _employeeFactory.CreateEmployee("Huy", "Vo", "Company", true);

            // Assert
            Assert.IsType<ExternalEmployee>(employee);
            //Assert.IsAssignableFrom<Employee>(employee);
        }
    }
}
