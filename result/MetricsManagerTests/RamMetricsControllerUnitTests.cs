using AutoMapper;
using MetricsManager.Controllers;
using MetricsManager.DAL.Interfaces.Metrics;
using MetricsManager.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MetricsManagerTests
{
    public class RamMetricsControllerUnitTests
    {
        private RamMetricsController controller;
        private Mock<IRamMetricsRepository> repositoryMock;
        private Mock<ILogger<RamMetricsController>> loggerMock;
        private Mock<IMapper> mapperMock;
        public RamMetricsControllerUnitTests()
        {
            repositoryMock = new Mock<IRamMetricsRepository>();
            mapperMock = new Mock<IMapper>();
            loggerMock = new Mock<ILogger<RamMetricsController>>();
            controller = new RamMetricsController(repositoryMock.Object, mapperMock.Object, loggerMock.Object);
        }
        [Fact]
        public void GetMetricsFromCluster_ReturnsOk()
        {
            //arrange
            TimeSpan fromTime = TimeSpan.FromSeconds(0);
            TimeSpan toTime = TimeSpan.FromSeconds(1);
            repositoryMock.Setup(repository => repository.GetByTimePeriod(0, fromTime, toTime)).Returns(Task.FromResult(new List<RamMetric>()));

            //act
            IActionResult result = controller.GetMetricsByTimePeriod(0, fromTime, toTime).Result;

            //assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
