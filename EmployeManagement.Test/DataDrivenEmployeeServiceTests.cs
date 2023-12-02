using EmployeeManagement.Business.EventArguments;
using EmployeeManagement.Business.Exceptions;
using EmployeeManagement.DataAccess.Entities;
using EmployeManagement.Test.Fixtures;
using EmployeManagement.Test.TestData;
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

        public static IEnumerable<object[]> ExampleTestDataForGiveRaise_WithPropery
        {
            get
            {
                return new List<object[]>
                {
                    new object[] { 100, true },
                    new object[] { 200, false }
                };
            }
        }

        public static TheoryData<int, bool> StronglyTypedExampleTestDataForGiveRaise_WithPropery
        {
            get
            {
                return new TheoryData<int, bool>
                {
                     { 100, true },
                     { 200, false }
                };
            }
        }

        public static IEnumerable<object[]> ExampleTestDataForGiveRaise_WithMethod(
            int testDataInstancesToProvide)
        {
            var testData = new List<object[]>
            {
                new object[] { 100, true },
                new object[] { 200, false }
            };

            return testData.Take(testDataInstancesToProvide);
        }

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

        [Theory]
        [InlineData("37e03ca7-c730-4351-834c-b66f280cdb01")]
        [InlineData("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e")]
        public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendedObligatoryClassWithSpecificGuid
            (Guid courseId)
        {
            // Arrange
            // Act
            var internalEmployee = _employeeServiceFixture.EmployeeService.CreateInternalEmployee("Huy", "Vo");

            // Assert
            Assert.Contains(internalEmployee.AttendedCourses,
                course => course.Id == courseId);
        }

        [Theory]
        //[MemberData(
        //    nameof(ExampleTestDataForGiveRaise_WithMethod),
        //    2,
        //    MemberType = typeof(DataDrivenEmployeeServiceTests))]
        //[ClassData(typeof(EmployeeServiceTestData))]
        //[ClassData(typeof(StronglyTypedEmployeeServiceTestData))]
        [ClassData(typeof(StronglyTypedEmployeServiceTestData_FromFile))]
        public async Task GiveRaise_RaiseGiven_EmployeeMinimumRaiseGivenMatchesValue(int raiseGiven, bool expectedValueForMinimumRaiseGiven)
        {
            // Arrange
            var internalEmployee = new InternalEmployee("Huy", "Vo", 5, 3000, false, 1);

            // Act
            await _employeeServiceFixture.EmployeeService.GiveRaiseAsync(internalEmployee, raiseGiven);

            // Assert
            Assert.Equal(expectedValueForMinimumRaiseGiven, internalEmployee.MinimumRaiseGiven);
        }
    }
}
