
using System;
using System.Collections;
using System.Collections.Generic;
using TrackSimulator;
using Xunit;

namespace TrackSimulator_Testing
{
    [Collection("Driver Tests")]
    public class DriverTests : IDisposable
    {
        private readonly Driver _testDriver;

        public DriverTests()
        {
            _testDriver = new Driver("The", "Stig", "T0PGEAR")
            {
                City = "London",
                State = "England",
                Car_Make = "Reliant",
                Car_Model = "Robin",
                Car_Year = 2022
            };
        }

        public void Dispose()
        {

        }

        [Fact]
        public void FormSearchTerms()
        {
            string expectedResult = " FirstName LIKE '%The%' and  LastName LIKE '%Stig%' and  DriverNumber LIKE '%T0PGEAR%'";
            string actualResult = _testDriver.SearchTerms();
            Assert.Equal(expectedResult, actualResult);
        }
    }

    [Collection("Run Tests")]
    public class RunTests : IDisposable
    {
        private readonly Run _testRun;
        public RunTests()
        {
            _testRun = new Run();
        }
        public void Dispose()
        {

        }

        [Theory]
        [MemberData(nameof(RunTimestamps))]
        public void CalculateSensorTime(DateTime start, DateTime sensor, decimal expected)
        {
            decimal actualResult = _testRun.CalculateTimeInSeconds(start, sensor);
            Assert.Equal(expected, actualResult);
        }
        public static IEnumerable<object[]> RunTimestamps =>
        new List<object[]>
        {
            new object[] { new DateTime(2022, 1, 18, 10, 5, 30, 456), new DateTime(2022, 1, 18, 10, 5, 30, 800), 0.344},
            new object[] { new DateTime(2022, 1, 18, 10, 5, 30, 150), new DateTime(2022, 1, 18, 10, 5, 35, 785), 5.635},
            new object[] { new DateTime(2022, 1, 18, 10, 5, 30, 100), new DateTime(2022, 1, 18, 10, 5, 30, 100), 0},
            new object[] { new DateTime(2022, 1, 18, 10, 5, 30, 400), new DateTime(2022, 1, 18, 10, 5, 30, 200), 0}
        };
    }
}
