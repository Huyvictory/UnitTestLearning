using AutoMapper;
using EmployeeManagement.Business;
using EmployeeManagement.Controllers;
using EmployeeManagement.DataAccess.Entities;
using EmployeeManagement.MapperProfiles;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeManagement.Test
{
    public class EmployeeOverviewTests
    {
        private readonly EmployeeOverviewController _employeeOverviewController;
        public EmployeeOverviewTests()
        {
            var employeeServiceMock = new Mock<IEmployeeService>();
            employeeServiceMock
                .Setup(m => m.FetchInternalEmployeesAsync())
                .ReturnsAsync(new List<InternalEmployee>() {
                    new InternalEmployee("Megan1", "Jones1", 2, 3000, false, 2),
                    new InternalEmployee("Megan2", "Jones2", 1, 2500, false, 2),
                    new InternalEmployee("Megan3", "Jones3", 1, 3000, false, 2),
                    new InternalEmployee("Megan4", "Jones4", 2, 2000, true, 3)
                });

            //var mapperMock = new Mock<IMapper>();
            //mapperMock.Setup(m => m.Map<InternalEmployee, InternalEmployeeForOverviewViewModel>
            //(It.IsAny<InternalEmployee>()))
            //.Returns(new InternalEmployeeForOverviewViewModel());

            var mapperConfiguration = new MapperConfiguration(
                cfg => cfg.AddProfile<EmployeeProfile>());
            var mapper = new Mapper(mapperConfiguration);

            _employeeOverviewController = new EmployeeOverviewController(
                employeeServiceMock.Object, mapper);
        }

        [Fact]
        public async Task Index_GetAction_MustReturnViewResult()
        {
            // Arrange
            // Act
            var result = await _employeeOverviewController.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Index_GetAction_MustReturnEmployeeOverviewViewModelAsViewModelType()
        {
            // Arrange

            // Act
            var result = await _employeeOverviewController.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<EmployeeOverviewViewModel>(viewResult.Model);
        }

        [Fact]
        public async Task Index_GetAction_MustReturnExactNumberOfInternalEmployeesFetched()
        {
            // Arrange

            // Act
            var result = await _employeeOverviewController.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            // Assert
            Assert.Equal(4, ((EmployeeOverviewViewModel)((ViewResult)viewResult).Model).InternalEmployees.Count);
        }

        [Fact]
        public async Task Index_GetAction_ReturnsViewResultWithInternalEmployees()
        {
            // Arrange

            // Act
            var result = await _employeeOverviewController.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var modelPassedView = Assert.IsType<EmployeeOverviewViewModel>(viewResult.Model);
            Assert.Equal(4, modelPassedView.InternalEmployees.Count);
        }
    }
}
