using AutoMapper;
using EmployeeManagement.Business;
using EmployeeManagement.Controllers;
using EmployeeManagement.DataAccess.Entities;
using EmployeeManagement.MapperProfiles;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeManagement.Test
{
    public class InternalEmployeeControllerTests
    {
        private Mapper mapper;
        private readonly InternalEmployeeController _internalEmployeeController;
        private Mock<IEmployeeService> _employeeServiceMock;

        public InternalEmployeeControllerTests()
        {
            _employeeServiceMock = new Mock<IEmployeeService>();

            var mapperConfiguration = new MapperConfiguration(
                cfg => cfg.AddProfile<EmployeeProfile>());
            mapper = new Mapper(mapperConfiguration);

            _internalEmployeeController = new InternalEmployeeController(
                _employeeServiceMock.Object, mapper);
        }
        [Fact]
        public async Task AddInternalEmployee_InvalidInput_MustReturnBadRequest()
        {
            // Arrange
            var createInternalEmployeeViewModel = new CreateInternalEmployeeViewModel();
            _internalEmployeeController.ModelState.AddModelError("FirstName", "Required");

            // Act
            var result = await _internalEmployeeController.
                AddInternalEmployee(createInternalEmployeeViewModel);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task InternalEmployeeDetails_InputFromTempData_MustReturnCorrectData()
        {
            // Arrange
            var expectedEmployeeId = Guid.Parse("7183748a-ebeb-4355-8084-f190f8a5a68f");

            _employeeServiceMock.Setup(m => m.FetchInternalEmployeeAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new InternalEmployee("Jaimy", "Johnson", 3, 3400, true, 1)
                {
                    Id = expectedEmployeeId,
                    SuggestedBonus = 500,
                }
               );

            var mapperConfiguration = new MapperConfiguration(
                cfg => cfg.AddProfile<EmployeeProfile>());
            var mapper = new Mapper(mapperConfiguration);

            var internalEmployeeController = new InternalEmployeeController(
                _employeeServiceMock.Object, mapper);

            var tempDataDictionary = new TempDataDictionary(
                new DefaultHttpContext(),
                new Mock<ITempDataProvider>().Object);
            tempDataDictionary["EmployeeId"] = expectedEmployeeId;

            internalEmployeeController.TempData = tempDataDictionary;

            // Act
            var result = await internalEmployeeController.InternalEmployeeDetails(null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsType<InternalEmployeeDetailViewModel>(viewResult.Model);
            Assert.Equal(expectedEmployeeId, viewModel.Id);
        }
    }
}
