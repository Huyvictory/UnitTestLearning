﻿using EmployeeManagement.ActionFilters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeManagement.Test
{
    public class CheckShowStatisticsHeaderTests
    {
        [Fact]
        public void OnActionExecuting_InvokeWithoutShowStatisticsHeader_ReturnsBadRequest()
        {
            // Arrange
            var checkShowStatisticsHeaderActionFilter = new CheckShowStatisticsHeader();

            var httpContext = new DefaultHttpContext();

            var actionContext = new ActionContext(httpContext, new(), new(), new());

            var actionExecutingContext = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>(),
                controller: null);

            // Act
            checkShowStatisticsHeaderActionFilter.OnActionExecuting(actionExecutingContext);

            // Assert
            Assert.IsType<BadRequestResult>(actionExecutingContext.Result);
        }
    }
}
