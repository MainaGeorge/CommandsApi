using System;
using System.Collections;
using System.Collections.Generic;
using AutoMapper;
using CommandAPI.Controllers;
using CommandAPI.Data;
using CommandAPI.Dtos;
using CommandAPI.Models;
using CommandAPI.Profiles;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CommandAPI.Tests
{
    public class CommandsControllerTests : IDisposable
    {
        private Mock<ICommandApiRepository> _mockRepo;
        private CommandsProfile _realProfile;
        private MapperConfiguration _configuration;
        private IMapper _mapper;
        public CommandsControllerTests()
        {
            _mockRepo = new Mock<ICommandApiRepository>();
            _realProfile = new CommandsProfile();
            _configuration = new MapperConfiguration(conf =>
                conf.AddProfile(_realProfile));
            _mapper = new Mapper(_configuration);
        }

        private static IEnumerable<Command> GetCommands(int number)
        {

            var commands = new List<Command>();
            if (number > 0)
            {
                commands.Add(new Command
                {
                    Id = 0,
                    HowTo = "How to generate a migration",
                    CommandLine = "dotnet ef migrations add <Name of Migration>",
                    Platform = ".Net Core EF"
                });
            }

            return commands;
        }

        [Fact]
        public void GetCommandItems_ReturnsZeroItems_WhenDbIsEmpty()
        {
            //Arrange
            _mockRepo.Setup(m => m.GetAllCommands())
                .Returns(GetCommands(0));

            var controller = new CommandsController(_mockRepo.Object, _mapper);

            //Act 
            var result = controller.GetCommands();

            //Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetCommands_ReturnsASingleItem_WhenDbHasJustOneItem()
        {
            //Arrange
            _mockRepo.Setup(repo => repo.GetAllCommands())
                .Returns(GetCommands(1));

            var controller = new CommandsController(_mockRepo.Object, _mapper);

            //Act
            var result = controller.GetCommands();

            //Assert
            var okResult = result.Result as OkObjectResult;

            if (okResult!.Value is List<Command> commands) Assert.Single((IEnumerable)commands);
        }

        [Fact]
        public void GetCommands_Returns200Ok_WhenDbHasOneEntry()
        {
            //Arrange
            _mockRepo.Setup(r => r.GetAllCommands())
                .Returns(GetCommands(1));
            var controller = new CommandsController(_mockRepo.Object, _mapper);

            //Act
            var result = controller.GetCommands();

            //Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetCommands_ReturnsCorrectObjectType_WhenDbHasOneEntry()
        {
            //arrange
            _mockRepo.Setup(r => r.GetAllCommands())
                .Returns(GetCommands(1));
            var controller = new CommandsController(_mockRepo.Object, _mapper);

            //act
            var result = controller.GetCommands();

            //Assert
            Assert.IsType<ActionResult<IEnumerable<CommandReadDto>>>(result);
        }

        [Fact]
        public void GetCommandById_Returns404NotFound_WhenNonExistentIdProvided()
        {
            //arrange
            _mockRepo.Setup(r => r.GetCommandById(0))
                .Returns(() => null);
            var controller = new CommandsController(_mockRepo.Object, _mapper);

            //act
            var result = controller.GetCommandById(0);

            //Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
        [Fact]
        public void GetCommandById_Returns200Ok_WhenAnExistingIdProvided()
        {
            //arrange
            _mockRepo.Setup(r => r.GetCommandById(1))
                .Returns(new Command
                {
                    Id = 0,
                    HowTo = "How to generate a migration",
                    CommandLine = "dotnet ef migrations add <Name of Migration>",
                    Platform = ".Net Core EF"
                });
            var controller = new CommandsController(_mockRepo.Object, _mapper);

            //Act
            var result = controller.GetCommandById(1);

            //Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetCommandById_ReturnsCorrectType_GivenAnExistingId()
        {
            //arrange
            _mockRepo.Setup(r => r.GetCommandById(1))
                .Returns(new Command
                {
                    Id = 0,
                    HowTo = "How to generate a migration",
                    CommandLine = "dotnet ef migrations add <Name of Migration>",
                    Platform = ".Net Core EF"
                });
            var controller = new CommandsController(_mockRepo.Object, _mapper);

            //Act
            var result = controller.GetCommandById(1);

            //Assert
            Assert.IsType<ActionResult<CommandReadDto>>(result);
        }

        [Fact]
        public void CreateCommand_ReturnsCorrectType_WhenValidObjectIsSubmitted()
        {
            //arrange
            var controller = new CommandsController(_mockRepo.Object, _mapper);

            //act
            var result = controller.CreateCommand(new CommandCreateDto());

            //assert
            Assert.IsType<ActionResult<CommandReadDto>>(result);
        }

        [Fact]
        public void CreateCommand_ReturnsCreatedAtRoute_WhenValidObjectSubmitted()
        {
            //arrange
            var controller = new CommandsController(_mockRepo.Object, _mapper);

            //act
            var result = controller.CreateCommand(new CommandCreateDto());

            //assert
            Assert.IsType<CreatedAtRouteResult>(result.Result);

        }

        [Fact]
        public void UpdateCommand_ReturnsNoContent_WhenAnExistentIdIsProvided()
        {
            _mockRepo.Setup(r => r.GetCommandById(1))
                .Returns(new Command
                {
                    Id = 0,
                    HowTo = "How to generate a migration",
                    CommandLine = "dotnet ef migrations add <Name of Migration>",
                    Platform = ".Net Core EF"
                });
            var controller = new CommandsController(_mockRepo.Object, _mapper);

            var result = controller.UpdateCommand(new CommandUpdateDto() { }, 1);

            Assert.IsType<NoContentResult>(result);
        }
        [Fact]
        public void UpdateCommand_Returns404NotFound_WithInvalidId()
        {
            _mockRepo.Setup(r => r.GetCommandById(0))
                .Returns(() => null);
            var controller = new CommandsController(_mockRepo.Object, _mapper);

            var result = controller.UpdateCommand(new CommandUpdateDto(), 0);

            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public void PartialCommandUpdate_Returns404NotFound_WithAnInvalidId()
        {
            _mockRepo.Setup(r => r.GetCommandById(1)).Returns(() => null);
            var controller = new CommandsController(_mockRepo.Object, _mapper);

            var result = controller.PartialCommandUpdate(1,
                new JsonPatchDocument<CommandUpdateDto>());

            Assert.IsType<NotFoundResult>(result);
        }
        public void Dispose()
        {
            _mockRepo = null;
            _mapper = null;
            _configuration = null;
            _realProfile = null;
        }
    }

}
