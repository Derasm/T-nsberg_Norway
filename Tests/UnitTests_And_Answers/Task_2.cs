using Moq;
using Shared.Common.Models;


namespace UnitTests_And_Answers
{
    public class Task_2
    {
        private Mock<IFileWriter> _mockWriter;
        private Mock<IDateTimeProvider> _mockDateTimeProvider;
        private Logger _logger;
        /// <summary>
        /// Singular setup for all tests - as the logger is stateless, we can reuse it for all tests
        /// Also there are no other dependencies than the mocked ones, so we don't need to reinitialize them
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _mockWriter = new Mock<IFileWriter>();
            _mockDateTimeProvider = new Mock<IDateTimeProvider>();
            _mockDateTimeProvider.Setup(p => p.Now).Returns(new DateTime(2023, 12, 7, 12, 0, 0));
            _logger = new Logger(_mockWriter.Object, _mockDateTimeProvider.Object);
        }
        [Test]
        public void Log_ShouldPrefixInputWithDateTime()
        {
            // Arrange
            string input = "Test log message";
            string expectedLogEntry = "[07.12.23 12:00:00] Test log message";

            // Act
            _logger.Log(input);

            // Assert
            _mockWriter.Verify(w => w.WriteLine(expectedLogEntry), Times.Once);
        }

    }
}
