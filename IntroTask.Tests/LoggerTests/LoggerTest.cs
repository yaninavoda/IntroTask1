using Contracts;
using LoggerService;

namespace IntroTask.Tests.LoggerTests
{
    public class LoggerTest
    {
        private ILoggerManager _loggerManager;
        private int _callCount;

        [SetUp]
        public void Setup()
        {
            _loggerManager = new LoggerManager();
            _callCount = 0;
        }

        [TestCase("")]
        [TestCase("message")]
        public void LogDebug_IsCalledOnce(string message)
        {

            // Act
            _loggerManager.LogDebug(message);
            _callCount++;

            // Assert
            Assert.That(_callCount, Is.EqualTo(1));
        }

        [TestCase("")]
        [TestCase("message")]
        public void LogWarn_IsCalledOnce(string message)
        {

            // Act
            _loggerManager.LogWarn(message);
            _callCount++;

            // Assert
            Assert.That(_callCount, Is.EqualTo(1));
        }

        [TestCase("")]
        [TestCase("message")]
        public void TesLogError_IsCalledOncet3(string message)
        {

            // Act
            _loggerManager.LogError(message);
            _callCount++;

            // Assert
            Assert.That(_callCount, Is.EqualTo(1));
        }

        [TestCase("")]
        [TestCase("message")]
        public void LogInfo_IsCalledOnce(string message)
        {

            // Act
            _loggerManager.LogInfo(message);
            _callCount++;

            // Assert
            Assert.That(_callCount, Is.EqualTo(1));
        }
    }
}