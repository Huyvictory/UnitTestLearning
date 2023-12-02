using EmployeeManagement.Business;
using EmployeeManagement.DataAccess.Services;
using EmployeeManagement.Services.Test;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeManagement.Test.Fixtures
{
    public class EmployeeServiceWithAspNetCoreDIFixture : IDisposable
    {
        private readonly ServiceProvider _serviceProvider;

        public IEmployeeManagementRepository EmployeeManagementTestDataRepository
        {
            get
            {
                return _serviceProvider.GetService<IEmployeeManagementRepository>();
            }
        }
        public IEmployeeService EmployeeService
        {
            get
            {
                return _serviceProvider.GetService<IEmployeeService>();
            }
        }
        public EmployeeServiceWithAspNetCoreDIFixture()
        {
            var services = new ServiceCollection();
            services.AddScoped<EmployeeFactory>();
            services.AddScoped<IEmployeeManagementRepository, EmployeeManagementTestDataRepository>();
            services.AddScoped<IEmployeeService, EmployeeService>();

            // build service provider
            _serviceProvider = services.BuildServiceProvider();
        }
        public void Dispose()
        {

        }
    }
}
