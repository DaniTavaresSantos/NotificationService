using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using NotificationService.Infra.Cache;
using Xunit;

namespace NotificationService.Test.Infra;

public class RateLimitRepositoryTests
{
        private readonly Mock<IDistributedCache> _mockDistributedCache;
        private readonly RateLimitRepository _rateLimitRepository;

        public RateLimitRepositoryTests()
        {
            _mockDistributedCache = new Mock<IDistributedCache>();
            _rateLimitRepository = new RateLimitRepository(_mockDistributedCache.Object);
        }

        [Fact]
        public void GetData_ShouldReturnDeserializedValue_WhenCacheHasData()
        {
            // Arrange
            var key = "testKey";
            var testValue = 1;
            var jsonValue = JsonSerializer.Serialize(testValue);
            
            _mockDistributedCache.Setup(c => c.Get(key)).Returns(Encoding.ASCII.GetBytes(jsonValue));

            // Act
            var result = _rateLimitRepository.GetData<int>(key);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(testValue, result);
        }

        [Fact]
        public void GetData_ShouldReturnDefault_WhenCacheIsEmpty()
        {
            // Arrange
            var key = "testKey";
            
            _mockDistributedCache.Setup(c => c.Get(key)).Returns([]);

            // Act
            var result = _rateLimitRepository.GetData<dynamic>(key);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void SetData_ShouldCallSetStringOnCacheService()
        {
            // Arrange
            var key = "testKey";
            var value = 1;
            var expirationTime = TimeSpan.FromMinutes(5);

            // Act
            _rateLimitRepository.SetData(key, value, expirationTime);

            // Assert
            _mockDistributedCache.Verify(c => c.Set(It.IsAny<string>(), 
                It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>()), Times.Once);
        }
}