using System.Collections.Generic;
using CommandAPI.Data;
using CommandAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandApiRepository _repo;

        public CommandsController(ICommandApiRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Command>> GetCommands()
        {
            var commands = _repo.GetAllCommands();

            return Ok(commands);
        }

        [HttpGet("{commandId:int}")]
        public ActionResult<Command> GetCommandById(int commandId)
        {
            var command = _repo.GetCommandById(commandId);

            return command == null ? NotFound() : Ok(command);
        }
    }
}