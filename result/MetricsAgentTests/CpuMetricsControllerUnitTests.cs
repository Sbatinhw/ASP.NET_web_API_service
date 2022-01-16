using AutoMapper;
using MetricsAgent.Controllers;
using MetricsAgent.DAL.Interfaces;
using MetricsAgent.DAL.Models;
using MetricsAgent.DAL.Repositories;
using MetricsAgent.Infrastructure.Mapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MetricsAgentTests
{
    public class CpuMetricsControllerUnitTests
    {
        private CpuMetricsController controller;
        private Mock<ICpuMetricsRepository> repositoryMock;
        private Mock<ILogger<CpuMetricsController>> loggerMock;
        private Mock<IMapper> mapperMock;
        public CpuMetricsControllerUnitTests()
        {
            repositoryMock = new Mock<ICpuMetricsRepository>();
            mapperMock = new Mock<IMapper>();
            loggerMock = new Mock<ILogger<CpuMetricsController>>();
            controller = new CpuMetricsController(repositoryMock.Object, mapperMock.Object, loggerMock.Object);
        }
        [Fact]
        public void GetMetricsFromCluster_ReturnsOk()
        {
            //arrange
            TimeSpan fromTime = TimeSpan.FromSeconds(0);
            TimeSpan toTime = TimeSpan.FromSeconds(1);
            repositoryMock.Setup(repository => repository.GetByTimePeriod(fromTime, toTime)).Returns(Task.FromResult(new List<CpuMetric>()));

            //act
            IActionResult result = controller.GetMetricsByTimePeriod(fromTime, toTime).Result;

            //assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
