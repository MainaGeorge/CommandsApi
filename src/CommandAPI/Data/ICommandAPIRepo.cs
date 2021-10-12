using System.Collections.Generic;
using CommandAPI.Models;

namespace CommandAPI.Data
{
    public interface ICommandApiRepository {
        bool SaveChanges();
        IEnumerable<Command> GetAllCommands();
        Command GetCommandById(int commandId);
        void CreateCommand(Command command);
        void UpdateCommand(Command command);
        void DeleteCommand(Command command);
    }
}