using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetricsAgent.Controllers;
using MetricsAgent.DAL;
using MetricsAgent.Models;
using MetricsAgent;
using Moq;
using Xunit;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace MetricsAgentTests
{
    public class CpuMetricsControllerUnitTests
    {
        private CpuMetricsController controller;
        private Mock<ICpuMetricsRepository> mock_repository;
        private Mock<IMapper> mock_mapper;
        private Mock<ILogger<CpuMetricsController>> mock_logger;

        public CpuMetricsControllerUnitTests()
        {
            mock_repository = new Mock<ICpuMetricsRepository>();
            mock_mapper = new Mock<IMapper>();
            mock_logger = new Mock<ILogger<CpuMetricsController>>();

            controller = new CpuMetricsController(mock_repository.Object, mock_mapper.Object, mock_logger.Object);
        }

        [Fact]
        public void Create_ShouldCall_Create_From_Repository()
        {
            // устанавливаем параметр заглушки
            // в заглушке прописываем что в репозиторий прилетит CpuMetric объект
            mock_repository.Setup(repository => repository.Create(It.IsAny<CpuMetric>())).Verifiable();
            mock_mapper.Setup(mapper => mapper.Map<CpuMetricDto>(It.IsAny<object>())).Verifiable();

            // выполняем действие на контроллере
            var result = controller.Create(new MetricsAgent.Requests.CpuMetricCreateRequest { Time = TimeSpan.FromSeconds(1), Value = 50 });

            // проверяем заглушку на то, что пока работал контроллер
            // действительно вызвался метод Create репозитория с нужным типом объекта в параметре
            mock_repository.Verify(repository => repository.Create(It.IsAny<CpuMetric>()), Times.AtMostOnce());
        }

        [Fact]
        public void GetFromCluster_ShouldCall_Create_From_Repository()
        {
            //создание заглушек
            mock_repository.Setup(repository => repository.GetCluster(It.IsAny<long>(), It.IsAny<long>())).Returns(new List<CpuMetric>()).Verifiable();
            mock_mapper.Setup(mapper => mapper.Map<CpuMetricDto>(It.IsAny<object>())).Verifiable();

            //действие
            var result = controller.GetFromCluster(It.IsAny<long>(), It.IsAny<long>());// (0, 10);

            //
            mock_repository.Verify(repository => repository.GetCluster(It.IsAny<long>(), It.IsAny<long>()));
        }

    }
}
