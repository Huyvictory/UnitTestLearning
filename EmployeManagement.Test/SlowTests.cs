using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeManagement.Test
{
    public class SlowTests
    {
        [Fact]
        public void SlowTest2()
        {
            Thread.Sleep(5000);
            Assert.True(true);
        }
    }
}
