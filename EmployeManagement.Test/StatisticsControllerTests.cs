﻿using AutoMapper;
using EmployeeManagement.Controllers;
using EmployeeManagement.MapperProfiles;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeManagement.Test
{
    public class StatisticsControllerTests
    {
        [Fact]
        public void Index_InputFromHttpConectionFeature_MustReturnInputtedIps()
        {
            // Arrange
            var localIpAddress = System.Net.IPAddress.Parse("111.111.111.111");
            var localPort = 5000;
            var remoteIpAddress = System.Net.IPAddress.Parse("222.222.222.222");
            var remotePort = 8080;

            var featureCollectionMock = new Mock<IFeatureCollection>();
            featureCollectionMock.Setup(e => e.Get<IHttpConnectionFeature>())
                .Returns(new HttpConnectionFeature
                {
                    LocalIpAddress = localIpAddress,
                    LocalPort = localPort,
                    RemoteIpAddress = remoteIpAddress,
                    RemotePort = remotePort
                });

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(e => e.Features)
                .Returns(featureCollectionMock.Object);

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<StatisticsProfile>());
            var mapper = new Mapper(mapperConfiguration);

            var statisticsController = new StatisticsController(mapper);
            statisticsController.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext()
            {
                HttpContext = httpContextMock.Object,
            };

            // Act
            var result = statisticsController.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsType<StatisticsViewModel>(viewResult.Model);
            Assert.Equal(localIpAddress.ToString(), viewModel.LocalIpAddress);
            Assert.Equal(localPort, viewModel.LocalPort);
            Assert.Equal(remoteIpAddress.ToString(), viewModel.RemoteIpAddress);
            Assert.Equal(remotePort, viewModel.RemotePort);
        }
    }
}
