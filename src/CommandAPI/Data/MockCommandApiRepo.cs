using System.Collections.Generic;
using System.Linq;
using CommandAPI.Models;

namespace CommandAPI.Data
{
    public class MockCommandApiRepo : ICommandApiRepository {

        public IEnumerable<Command> Commands { get; private set; }

        public MockCommandApiRepo()
        {
            Commands = new List<Command>()
            {
                new Command
                {
                    Id = 0, HowTo = "How to generate a migration",
                    CommandLine = "dotnet ef migrations add <Name of Migration>",
                    Platform = ".Net Core EF"
                },
                new Command
                {
                    Id = 1, HowTo = "Run Migrations",
                    CommandLine = "dotnet ef database update",
                    Platform = ".Net Core EF"
                },
                new Command
                {
                    Id = 2, HowTo = "List active migrations",
                    CommandLine = "dotnet ef migrations list",
                    Platform = ".Net Core EF"
                }
            };
        }
        public bool SaveChanges()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Command> GetAllCommands() => Commands;
        
        public Command GetCommandById(int commandId) => Commands
            .FirstOrDefault(p => p.Id.Equals(commandId));

        public void CreateCommand(Command command)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateCommand(Command command)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteCommand(Command command)
        {
            throw new System.NotImplementedException();
        }
    }
} 