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
    }
}
