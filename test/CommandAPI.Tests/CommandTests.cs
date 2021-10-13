using System;
using CommandAPI.Models;
using Xunit;

namespace CommandAPI.Tests
{
    public class CommandTests :IDisposable
    {
        private Command _testCommand;
        public CommandTests()
        {
            _testCommand = new Command()
            {
                HowTo = "Do something awesome",
                Platform = "NUnit",
                CommandLine = "dotnet test"
            };
        }
        public void Dispose()
        {
            _testCommand = null;
        }

        [Fact]
        public void CanChangeHowTo()
        {
            //Arrange
            const string howToCommand = "Run tests in xUnit";
            //Act
            _testCommand.HowTo = howToCommand;

            //Assert
            Assert.Equal(howToCommand, _testCommand.HowTo);
        }

        [Fact]
        public void CanChangePlatform()
        {
            //Act
            const string platform = "xUnit";
            _testCommand.Platform = platform;

            Assert.Equal(platform, _testCommand.Platform);
        }

        [Fact]
        public void CanChangeCommandLine()
        {
            //Act
            const string commandLine = "dotnet test";
            _testCommand.CommandLine = commandLine;

            Assert.Equal(commandLine, _testCommand.CommandLine);
        }
    }
}