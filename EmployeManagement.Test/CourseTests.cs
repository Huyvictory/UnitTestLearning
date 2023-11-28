using EmployeeManagement.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeManagement.Test
{
    public class CourseTests
    {
        [Fact]
        public void CourseContructor_ConstructCourse_IsNewMustBeTrue()
        {
            // Arrange

            // Act
            var course = new Course("Database Management 101");

            // Assert
            Assert.True(course.IsNew);
        }
    }
}
