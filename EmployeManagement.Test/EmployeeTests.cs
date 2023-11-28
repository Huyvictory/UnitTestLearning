using EmployeeManagement.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeManagement.Test
{
    public class EmployeeTests
    {
        [Fact]
        public void EmployeeFullNamePropetyGetter_InputFirstNameAndLastName_FullNameIsConcatenated()
        {
            // Arrange
            var internalEmployee = new InternalEmployee("Huy", "Vo", 1, 2500, false, 1);

            // Act
            internalEmployee.FirstName = "Lucia";
            internalEmployee.LastName = "Shelton";

            // Assert
            Assert.Equal("Lucia Shelton", internalEmployee.FullName);
        }

        [Fact]
        public void EmployeeFullNamePropetyGetter_InputFirstNameAndLastName_FullNameStartsFirstName()
        {
            // Arrange
            var internalEmployee = new InternalEmployee("Huy", "Vo", 1, 2500, false, 1);

            // Act
            internalEmployee.FirstName = "Lucia";
            internalEmployee.LastName = "Shelton";

            // Assert
            Assert.StartsWith(internalEmployee.FirstName, internalEmployee.FullName);
        }

        [Fact]
        public void EmployeeFullNamePropetyGetter_InputFirstNameAndLastName_FullNameEndsWithName()
        {
            // Arrange
            var internalEmployee = new InternalEmployee("Huy", "Vo", 1, 2500, false, 1);

            // Act
            internalEmployee.FirstName = "Lucia";
            internalEmployee.LastName = "Shelton";

            // Assert
            Assert.EndsWith(internalEmployee.LastName, internalEmployee.FullName);
        }

        [Fact]
        public void EmployeeFullNamePropetyGetter_InputFirstNameAndLastName_FullNameContainsPartOfConcatenation()
        {
            // Arrange
            var internalEmployee = new InternalEmployee("Huy", "Vo", 1, 2500, false, 1);

            // Act
            internalEmployee.FirstName = "Lucia";
            internalEmployee.LastName = "Shelton";

            // Assert
            Assert.Contains("on", internalEmployee.FullName);
        }

        [Fact]
        public void EmployeeFullNamePropetyGetter_InputFirstNameAndLastName_FullNameSoundsLikeConcatenation()
        {
            // Arrange
            var internalEmployee = new InternalEmployee("Huy", "Vo", 1, 2500, false, 1);

            // Act
            internalEmployee.FirstName = "Lusia";
            internalEmployee.LastName = "Sheldon";

            // Assert
            Assert.Matches("Lu(c|s|z)ia Shel(t|d)on", internalEmployee.FullName);
        }
    }
}
