using AutoMapper;
using DataObjects_BE.Entities;
using DTOs_BE.ServiceDTOs;
using Repositories_BE.Interfaces;
using Services_BE.Interfaces;
using Services_BE.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BE.Test.ServiceTest
{
    public class ServiceServiceTest : SetupTest
    {
        private readonly IServiceService _serviceService;
        private readonly Mock<IServiceRepositoy> _serviceRepositoryMock;
        private readonly Mock<ICurrentTime> _currentTimeMock;
        private readonly Mock<IMapper> _mapperMock;

        public ServiceServiceTest()
        {
            _serviceRepositoryMock = new Mock<IServiceRepositoy>();
            _mapperMock = new Mock<IMapper>();
            _currentTimeMock = new Mock<ICurrentTime>();
            _serviceService = new ServiceService
            (
                _serviceRepositoryMock.Object,
                _mapperMock.Object,
                _currentTimeMock.Object
            );
        }
        //ListService Test
        [Fact]
        public async Task ListService_ShouldReturnList_WhenDataExists()
        {
            // Arrange
            var services = new List<Service>
            {
                new Service { ServiceId = 1, ServiceName = "Service A", ServicePrice = 100, IsActive = true },
                new Service { ServiceId = 2, ServiceName = "Service B", ServicePrice = 200, IsActive = false }
            };

            var expectedDtos = new List<ServiceResponseDTO>
            {
                new ServiceResponseDTO { ServiceId = 1, ServiceName = "Service A", ServicePrice = 100 },
                new ServiceResponseDTO { ServiceId = 2, ServiceName = "Service B", ServicePrice = 200 }
            };

            _serviceRepositoryMock
                .Setup(repo => repo.Get(
                    It.IsAny<Expression<Func<Service, bool>>>(),
                    It.IsAny<Func<IQueryable<Service>, IOrderedQueryable<Service>>>(),
                    "ServiceOrders",
                    null,
                    null))
                .Returns(services);

            _mapperMock.Setup(mapper => mapper.Map<List<ServiceResponseDTO>>(services))
                .Returns(expectedDtos);

            // Act
            var result = await _serviceService.ListService();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Service A", result[0].ServiceName);
            Assert.Equal(100, result[0].ServicePrice);
            _serviceRepositoryMock.Verify(repo => repo.Get(It.IsAny<Expression<Func<Service, bool>>>(),
                It.IsAny<Func<IQueryable<Service>, IOrderedQueryable<Service>>>(),
                "ServiceOrders", null, null), Times.Once);
        }

        [Fact]
        public async Task ListService_ShouldThrowException_WhenListIsEmpty()
        {
            // Arrange
            _serviceRepositoryMock
                .Setup(repo => repo.Get(
                    It.IsAny<Expression<Func<Service, bool>>>(),
                    It.IsAny<Func<IQueryable<Service>, IOrderedQueryable<Service>>>(),
                    "ServiceOrders",
                    null,
                    null))
                .Returns(new List<Service>());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _serviceService.ListService());

            Assert.Equal("List is empty", exception.Message);
            _serviceRepositoryMock.Verify(repo => repo.Get(It.IsAny<Expression<Func<Service, bool>>>(),
                It.IsAny<Func<IQueryable<Service>, IOrderedQueryable<Service>>>(),
                "ServiceOrders", null, null), Times.Once);
        }
        //GetServiceById Test
        [Fact]
        public async Task GetServiceById_ShouldReturnService_WhenServiceExists()
        {
            // Arrange
            int serviceId = 1;
            var service = new Service
            {
                ServiceId = serviceId,
                ServiceName = "Test Service",
                ServicePrice = 100
            };

            var expectedDto = new ServiceResponseDTO
            {
                ServiceId = serviceId,
                ServiceName = "Test Service",
                ServicePrice = 100
            };

            _serviceRepositoryMock.Setup(repo => repo.GetByID(serviceId))
                .Returns(service);

            _mapperMock.Setup(mapper => mapper.Map<ServiceResponseDTO>(service))
                .Returns(expectedDto);

            // Act
            var result = await _serviceService.GetServiceById(serviceId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(serviceId, result.ServiceId);
            Assert.Equal("Test Service", result.ServiceName);
            _serviceRepositoryMock.Verify(repo => repo.GetByID(serviceId), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<ServiceResponseDTO>(service), Times.Once);
        }
        [Fact]
        public async Task GetServiceById_ShouldThrowException_WhenServiceDoesNotExist()
        {
            // Arrange
            int serviceId = 999;

            _serviceRepositoryMock.Setup(repo => repo.GetByID(serviceId))
                .Returns((Service)null); // Giả lập service không tồn tại

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _serviceService.GetServiceById(serviceId));

            Assert.Equal("Service is not existing!!!", exception.Message);
            _serviceRepositoryMock.Verify(repo => repo.GetByID(serviceId), Times.Once);
        }
        //CreateService Test
        [Fact]
        public async Task CreateService_ShouldReturnServiceResponseDTO_WhenSuccessful()
        {
            // Arrange
            var currentTime = DateTime.UtcNow;
            _currentTimeMock.Setup(ct => ct.GetCurrentTime()).Returns(currentTime);

            var createModel = new CreateServiceModel
            {
                ServiceName = "Test Service",
                ServicePrice = 100,
                ServiceDescription = "Test Description",
                ServiceDuration = 2.5f,
                IsActive = true
            };

            var serviceEntity = new Service
            {
                ServiceName = createModel.ServiceName,
                ServicePrice = createModel.ServicePrice,
                ServiceDescription = createModel.ServiceDescription,
                ServiceDuration = createModel.ServiceDuration,
                ServiceCreateDate = currentTime.Date,
                IsActive = createModel.IsActive
            };

            var expectedDto = new ServiceResponseDTO
            {
                ServiceId = 1,
                ServiceName = createModel.ServiceName,
                ServicePrice = createModel.ServicePrice,
                ServiceDescription = createModel.ServiceDescription,
                ServiceDuration = createModel.ServiceDuration,
                IsActive = createModel.IsActive
            };

            _mapperMock.Setup(mapper => mapper.Map<ServiceResponseDTO>(It.IsAny<Service>()))
                .Returns(expectedDto);

            _serviceRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Service>()))
                .Returns(Task.CompletedTask);

            _serviceRepositoryMock.Setup(repo => repo.Save());

            // Act
            var result = await _serviceService.CreateService(createModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createModel.ServiceName, result.ServiceName);
            Assert.Equal(createModel.ServicePrice, result.ServicePrice);
            Assert.Equal(createModel.ServiceDescription, result.ServiceDescription);
            Assert.Equal(createModel.ServiceDuration, result.ServiceDuration);
            Assert.Equal(createModel.IsActive, result.IsActive);
            _serviceRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Service>()), Times.Once);
            _serviceRepositoryMock.Verify(repo => repo.Save(), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<ServiceResponseDTO>(It.IsAny<Service>()), Times.Once);
        }

        [Fact]
        public async Task CreateService_ShouldThrowException_WhenErrorOccurs()
        {
            // Arrange
            var createModel = new CreateServiceModel
            {
                ServiceName = "Test Service",
                ServicePrice = 100,
                ServiceDescription = "Test Description",
                ServiceDuration = 2.5f,
                IsActive = true
            };

            _serviceRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Service>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _serviceService.CreateService(createModel));

            Assert.Equal("Database error", exception.Message);
            _serviceRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Service>()), Times.Once);
            _serviceRepositoryMock.Verify(repo => repo.Save(), Times.Never);
        }
        //UpdateService Test
        [Fact]
        public async Task UpdateService_ShouldReturnUpdatedServiceResponseDTO_WhenSuccessful()
        {
            // Arrange
            int serviceId = 1;

            var existingService = new Service
            {
                ServiceId = serviceId,
                ServiceName = "Old Name",
                ServicePrice = 200,
                ServiceDescription = "Old Description",
                ServiceDuration = 1.5f,
                IsActive = true
            };

            var updateModel = new UpdateServiceModel
            {
                ServiceName = "Updated Service",
                ServicePrice = 300,
                ServiceDescription = "Updated Description",
                ServiceDuration = 2.0f,
                IsActive = false
            };

            var expectedDto = new ServiceResponseDTO
            {
                ServiceId = serviceId,
                ServiceName = updateModel.ServiceName,
                ServicePrice = updateModel.ServicePrice,
                ServiceDescription = updateModel.ServiceDescription,
                ServiceDuration = updateModel.ServiceDuration,
                IsActive = updateModel.IsActive
            };

            _serviceRepositoryMock.Setup(repo => repo.GetByID(serviceId)).Returns(existingService);
            _mapperMock.Setup(mapper => mapper.Map<ServiceResponseDTO>(It.IsAny<Service>())).Returns(expectedDto);
            _serviceRepositoryMock.Setup(repo => repo.Update(It.IsAny<Service>()));
            _serviceRepositoryMock.Setup(repo => repo.Save());

            // Act
            var result = await _serviceService.UpdateService(updateModel, serviceId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updateModel.ServiceName, result.ServiceName);
            Assert.Equal(updateModel.ServicePrice, result.ServicePrice);
            Assert.Equal(updateModel.ServiceDescription, result.ServiceDescription);
            Assert.Equal(updateModel.ServiceDuration, result.ServiceDuration);
            Assert.Equal(updateModel.IsActive, result.IsActive);

            _serviceRepositoryMock.Verify(repo => repo.GetByID(serviceId), Times.Once);
            _serviceRepositoryMock.Verify(repo => repo.Update(It.IsAny<Service>()), Times.Once);
            _serviceRepositoryMock.Verify(repo => repo.Save(), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<ServiceResponseDTO>(It.IsAny<Service>()), Times.Once);
        }

        [Fact]
        public async Task UpdateService_ShouldThrowException_WhenServiceNotFound()
        {
            // Arrange
            int serviceId = 1;

            var updateModel = new UpdateServiceModel
            {
                ServiceName = "Updated Service",
                ServicePrice = 300,
                ServiceDescription = "Updated Description",
                ServiceDuration = 2.0f,
                IsActive = false
            };

            _serviceRepositoryMock.Setup(repo => repo.GetByID(serviceId)).Returns((Service)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _serviceService.UpdateService(updateModel, serviceId));

            Assert.Equal("Service is not existing!!!", exception.Message);
            _serviceRepositoryMock.Verify(repo => repo.GetByID(serviceId), Times.Once);
            _serviceRepositoryMock.Verify(repo => repo.Update(It.IsAny<Service>()), Times.Never);
            _serviceRepositoryMock.Verify(repo => repo.Save(), Times.Never);
        }
        //SoftRemoveService Test
        [Fact]
        public async Task SoftRemoveService_ShouldSetIsActiveToFalse_WhenServiceExists()
        {
            // Arrange
            int serviceId = 1;

            var existingService = new Service
            {
                ServiceId = serviceId,
                ServiceName = "Test Service",
                ServicePrice = 100,
                ServiceDescription = "Test Description",
                ServiceDuration = 1.5f,
                ServiceCreateDate = DateTime.UtcNow,
                IsActive = true
            };

            var expectedDto = new ServiceResponseDTO
            {
                ServiceId = serviceId,
                ServiceName = existingService.ServiceName,
                ServicePrice = existingService.ServicePrice,
                ServiceDescription = existingService.ServiceDescription,
                ServiceDuration = existingService.ServiceDuration,
                IsActive = false
            };

            _serviceRepositoryMock.Setup(repo => repo.GetByID(serviceId)).Returns(existingService);
            _mapperMock.Setup(mapper => mapper.Map<ServiceResponseDTO>(It.IsAny<Service>())).Returns(expectedDto);
            _serviceRepositoryMock.Setup(repo => repo.Update(It.IsAny<Service>()));
            _serviceRepositoryMock.Setup(repo => repo.Save());

            // Act
            var result = await _serviceService.SoftRemoveService(serviceId);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsActive);
            _serviceRepositoryMock.Verify(repo => repo.GetByID(serviceId), Times.Once);
            _serviceRepositoryMock.Verify(repo => repo.Update(It.IsAny<Service>()), Times.Once);
            _serviceRepositoryMock.Verify(repo => repo.Save(), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<ServiceResponseDTO>(It.IsAny<Service>()), Times.Once);
        }

        [Fact]
        public async Task SoftRemoveService_ShouldThrowException_WhenServiceNotFound()
        {
            // Arrange
            int serviceId = 1;

            _serviceRepositoryMock.Setup(repo => repo.GetByID(serviceId)).Returns((Service)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _serviceService.SoftRemoveService(serviceId));

            Assert.Equal("Service is not existing!!!", exception.Message);
            _serviceRepositoryMock.Verify(repo => repo.GetByID(serviceId), Times.Once);
            _serviceRepositoryMock.Verify(repo => repo.Update(It.IsAny<Service>()), Times.Never);
            _serviceRepositoryMock.Verify(repo => repo.Save(), Times.Never);
        }
    }
}
