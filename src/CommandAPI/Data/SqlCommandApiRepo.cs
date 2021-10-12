using System.Collections.Generic;
using System.Linq;
using CommandAPI.Models;

namespace CommandAPI.Data
{
    public class SqlCommandApiRepo : ICommandApiRepository
    {
        private readonly CommandContext _context;

        public SqlCommandApiRepo(CommandContext context)
        {
            _context = context;
        }
        public bool SaveChanges()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Command> GetAllCommands()
        {
            return _context.CommandItems.ToList();
        }

        public Command GetCommandById(int commandId)
        {
            return _context.CommandItems.Find(commandId);
        }

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