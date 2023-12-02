using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeManagement.Test.TestData
{
    public class StronglyTypedEmployeServiceTestData_FromFile : TheoryData<int, bool>
    {
        public StronglyTypedEmployeServiceTestData_FromFile()
        {
            var testDataLines = File.ReadAllLines("TestData/EmployeeServiceTestData.csv");
            foreach (var line in testDataLines)
            {
                // Split the string
                var splittedString = line.Split(',');
                // Try parsing
                if (int.TryParse(splittedString[0], out int raise) && bool.TryParse(splittedString[1], out bool minimumRaiseGiven))
                {
                    // add test data
                    Add(raise, minimumRaiseGiven);
                }
            }
        }
    }
}
