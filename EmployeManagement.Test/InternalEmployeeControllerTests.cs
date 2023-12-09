using AutoMapper;
using EmployeeManagement.Business;
using EmployeeManagement.Controllers;
using EmployeeManagement.DataAccess.DbContexts;
using EmployeeManagement.DataAccess.Entities;
using EmployeeManagement.DataAccess.Services;
using EmployeeManagement.MapperProfiles;
using EmployeeManagement.Services.Test;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmployeManagement.Test
{
    public class InternalEmployeeControllerTests
    {
        private Mapper mapper;
        private readonly InternalEmployeeController _internalEmployeeController;
        private Mock<IEmployeeService> _employeeServiceMock;
        private readonly HttpClient _httpClient;
        private readonly IEmployeeManagementRepository _employeeManagementRepository;
        private readonly PromotionService _promotionService;

        //public InternalEmployeeControllerTests()
        //{
        //    _employeeServiceMock = new Mock<IEmployeeService>();

        //    var mapperConfiguration = new MapperConfiguration(
        //        cfg => cfg.AddProfile<EmployeeProfile>());
        //    mapper = new Mapper(mapperConfiguration);

        //    _httpClient = new HttpClient();
        //    _employeeManagementRepository = new EmployeeManagementRepository();
        //    _promotionService = new PromotionService(_httpClient, _employeeManagementRepository);

        //    _internalEmployeeController = new InternalEmployeeController(
        //        _employeeServiceMock.Object, mapper, _promotionService);
        //}
        //[Fact]
        //public async Task AddInternalEmployee_InvalidInput_MustReturnBadRequest()
        //{
        //    // Arrange
        //    var createInternalEmployeeViewModel = new CreateInternalEmployeeViewModel();
        //    _internalEmployeeController.ModelState.AddModelError("FirstName", "Required");

        //    // Act
        //    var result = await _internalEmployeeController.
        //        AddInternalEmployee(createInternalEmployeeViewModel);

        //    // Assert
        //    Assert.IsType<BadRequestObjectResult>(result);
        //}

        //[Fact]
        //public async Task InternalEmployeeDetails_InputFromTempData_MustReturnCorrectData()
        //{
        //    // Arrange
        //    var expectedEmployeeId = Guid.Parse("7183748a-ebeb-4355-8084-f190f8a5a68f");

        //    _employeeServiceMock.Setup(m => m.FetchInternalEmployeeAsync(It.IsAny<Guid>()))
        //        .ReturnsAsync(new InternalEmployee("Jaimy", "Johnson", 3, 3400, true, 1)
        //        {
        //            Id = expectedEmployeeId,
        //            SuggestedBonus = 500,
        //        }
        //       );

        //    var mapperConfiguration = new MapperConfiguration(
        //        cfg => cfg.AddProfile<EmployeeProfile>());
        //    var mapper = new Mapper(mapperConfiguration);

        //    var internalEmployeeController = new InternalEmployeeController(
        //        _employeeServiceMock.Object, mapper);

        //    var tempDataDictionary = new TempDataDictionary(
        //        new DefaultHttpContext(),
        //        new Mock<ITempDataProvider>().Object);
        //    tempDataDictionary["EmployeeId"] = expectedEmployeeId;

        //    internalEmployeeController.TempData = tempDataDictionary;

        //    // Act
        //    var result = await internalEmployeeController.InternalEmployeeDetails(null);

        //    // Assert
        //    var viewResult = Assert.IsType<ViewResult>(result);
        //    var viewModel = Assert.IsType<InternalEmployeeDetailViewModel>(viewResult.Model);
        //    Assert.Equal(expectedEmployeeId, viewModel.Id);
        //}

        //[Fact]
        //public async Task InternalEmployeeDetails_InputFromSession_MustReturnCorrectData()
        //{
        //    // Arrange
        //    var expectedEmployeeId = Guid.Parse("7183748a-ebeb-4355-8084-f190f8a5a68f");

        //    _employeeServiceMock.Setup(m => m.FetchInternalEmployeeAsync(It.IsAny<Guid>()))
        //        .ReturnsAsync(new InternalEmployee("Jaimy", "Johnson", 3, 3400, true, 1)
        //        {
        //            Id = expectedEmployeeId,
        //            SuggestedBonus = 500,
        //        }
        //       );

        //    var defaultHttpContext = new DefaultHttpContext();

        //    var mapperConfiguration = new MapperConfiguration(
        //        cfg => cfg.AddProfile<EmployeeProfile>());
        //    var mapper = new Mapper(mapperConfiguration);

        //    var internalEmployeeController = new InternalEmployeeController(
        //        _employeeServiceMock.Object, mapper);

        //    var sessionMock = new Mock<ISession>();
        //    //sessionMock.Setup(s => s.GetString("EmployeeId"))
        //    //    .Returns(expectedEmployeeId.ToString());
        //    var guidAsBytes = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());
        //    sessionMock.Setup(s => s.TryGetValue(It.IsAny<string>(), out guidAsBytes))
        //        .Returns(true);

        //    defaultHttpContext.Session = sessionMock.Object;

        //    internalEmployeeController.ControllerContext = new ControllerContext()
        //    {
        //        HttpContext = defaultHttpContext
        //    };

        //    // Act
        //    var result = await internalEmployeeController.InternalEmployeeDetails(null);

        //    // Assert
        //    var viewResult = Assert.IsType<ViewResult>(result);
        //    var viewModel = Assert.IsType<InternalEmployeeDetailViewModel>(viewResult.Model);
        //    Assert.Equal(expectedEmployeeId, viewModel.Id);
        //}

        [Fact]
        public async Task ExecutePromotionRequest_RequestPromotion_MustPromoteEmployee()
        {
            // Arrange
            var expectedEmployeeId = Guid.NewGuid();
            var currentJobLevel = 1;

            var employeeServiceMock = new Mock<IEmployeeService>();
            employeeServiceMock.Setup(m => m.FetchInternalEmployeeAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new InternalEmployee("Jaimy", "Johnson", 3, 3400, true, currentJobLevel)
                {
                    Id = expectedEmployeeId,
                    SuggestedBonus = 500
                });

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<EmployeeProfile>());
            var mapper = new Mapper(mapperConfiguration);

            var eligiblePromotionHandlerMock = new Mock<HttpMessageHandler>();
            eligiblePromotionHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>
                ("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                {
                    Content = new StringContent(
                        JsonSerializer.Serialize(new PromotionEligibility() { EligibleForPromotion = true },
                        new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        }), Encoding.ASCII, "application/json")
                });
            var httpClient = new HttpClient(eligiblePromotionHandlerMock.Object);

            var promotionService = new PromotionService(httpClient,
                new EmployeeManagementTestDataRepository());

            var internalEmployeeController = new InternalEmployeeController(employeeServiceMock.Object,
                mapper, promotionService);

            // Act
            var result = await internalEmployeeController.ExecutePromotionRequest(expectedEmployeeId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsType<InternalEmployeeDetailViewModel>(viewResult.Model);
            Assert.Equal(expectedEmployeeId, viewModel.Id);
            Assert.Equal(++currentJobLevel, viewModel.JobLevel);
        }
    }
}
