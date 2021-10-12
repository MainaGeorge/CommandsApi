using System;
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

        public bool SaveChanges() => _context.SaveChanges() >= 0;

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
            if (command is null) throw new ArgumentNullException(nameof(command));
            _context.CommandItems.Add(command);
        }

        public void UpdateCommand(Command command)
        {
        }

        public void DeleteCommand(Command command)
        {
            if (command is null) throw new ArgumentNullException(nameof(command));
            _context.CommandItems.Remove(command);
        }
    }
}