using EmployeeManagement.Business;
using EmployeeManagement.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeManagement.Test
{
    public class EmployeeFactoryTests
    {
        [Fact]
        public void CreateEmployee_ConstructInternalEmployee_IsInternalEmployee()
        {
            var employeeFactory = new EmployeeFactory();

            var isInternalEmployee = (InternalEmployee)employeeFactory.CreateEmployee("Huy", "Vo");

            Assert.NotNull(isInternalEmployee);
        }

        [Fact]
        public void CreateEmployee_EnsureBaseSalary2500()
        {
            var employeeFactory = new EmployeeFactory();

            var InternalEmployee = (InternalEmployee)employeeFactory.CreateEmployee("Huy", "Vo");

            Assert.Equal(2500, InternalEmployee.Salary);
        }

        [Fact]
        public void CreateEmployee_ConstructInternalEmployee_SalaryMustBeBetween2500And3500()
        {
            // Arrange
            var employeeFactory = new EmployeeFactory();

            // Act
            var InternalEmployee = (InternalEmployee)employeeFactory.CreateEmployee("Huy", "Vo");

            // Assert
            Assert.True(InternalEmployee.Salary >= 2500 && InternalEmployee.Salary <= 3500, "Salary not in predfined range");
        }

        [Fact]
        public void CreateEmployee_ConstructInternalEmployee_SalaryMustBeInRange2500And3500()
        {
            // Arrange
            var employeeFactory = new EmployeeFactory();

            // Act
            var InternalEmployee = (InternalEmployee)employeeFactory.CreateEmployee("Huy", "Vo");

            // Assert
            Assert.InRange(InternalEmployee.Salary, 2500, 3500);
        }

        [Fact]
        public void CreateEmployee_EnsureBaseSalary2500_Precision()
        {
            var employeeFactory = new EmployeeFactory();

            var InternalEmployee = (InternalEmployee)employeeFactory.CreateEmployee("Huy", "Vo");
            InternalEmployee.Salary = 2500.123m;

            Assert.Equal(2500, InternalEmployee.Salary, 0);
        }
    }
}
