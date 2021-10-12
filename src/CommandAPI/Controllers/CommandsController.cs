using System.Collections.Generic;
using AutoMapper;
using CommandAPI.Data;
using CommandAPI.Dtos;
using CommandAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CommandAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandApiRepository _repo;
        private readonly IMapper _mapper;

        public CommandsController(ICommandApiRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommands()
        {
            var commands = _repo.GetAllCommands();

            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
        }

        [HttpGet("{commandId:int}", Name = "GetCommandById")]
        public ActionResult<CommandReadDto> GetCommandById(int commandId)
        {
            var command = _repo.GetCommandById(commandId);

            return command == null ? NotFound() : Ok(_mapper.Map<CommandReadDto>(command));
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand([FromBody]CommandCreateDto commandCreateDto)
        {
            var commandModel = _mapper.Map<Command>(commandCreateDto);

            _repo.CreateCommand(commandModel);
            _repo.SaveChanges();

            var commandReadDto = _mapper.Map<CommandReadDto>(commandModel);

            return CreatedAtRoute(nameof(GetCommandById),
                new {commandId = commandReadDto.Id},
                commandReadDto);
        }
        [HttpPut("{commandId:int}")]
        public IActionResult UpdateCommand([FromBody] CommandUpdateDto commandUpdateDto,
            [FromRoute]int commandId)
        {
            var commandFromRepo = _repo.GetCommandById(commandId);
            if (commandFromRepo is null) return NotFound();

            _mapper.Map(commandUpdateDto, commandFromRepo);

            _repo.UpdateCommand(commandFromRepo);

            _repo.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{commandId:int}")]
        public ActionResult PartialCommandUpdate(int commandId,
            JsonPatchDocument<CommandUpdateDto> patchDocument)
        {
            var commandFromRepo = _repo.GetCommandById(commandId);

            if (commandFromRepo is null) return NotFound();

            var cmdToPatch = _mapper.Map<CommandUpdateDto>(commandFromRepo);
            patchDocument.ApplyTo(cmdToPatch, ModelState);

            if (!TryValidateModel(cmdToPatch)) return ValidationProblem(ModelState);

            _mapper.Map(cmdToPatch, commandFromRepo);

            _repo.UpdateCommand(commandFromRepo);
            _repo.SaveChanges();

            return NoContent();
        }
        [HttpDelete("{commandId:int}")]
        public IActionResult DeleteCommand([FromRoute]int commandId)
        {
            var commandFromRepo = _repo.GetCommandById(commandId);
            if (commandFromRepo is null) return NotFound();

            _repo.DeleteCommand(commandFromRepo);
            _repo.SaveChanges();

            return NoContent();
        }
    }
}