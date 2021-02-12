using System;
using System.Collections.Generic;
using System.Linq;

namespace AliceBot.Commands
{
    public enum ParseCommandResult
    {
        Valid,
        InvalidCommand,
        TooFewParams,
        TooMuchParams,
    }

    public abstract class BaseCommand
    {
        protected readonly Alice alice;
        private readonly int paramCount;
        public string CommandString { get; }

        protected BaseCommand(Alice alice, string commandString, int paramCount)
        {
            this.alice = alice;
            this.paramCount = paramCount;
            CommandString = commandString;
        }

        private static readonly char[] WhiteChars = {' ', '\t'};
        protected ParseCommandResult ParseCommandArgs(string text, out List<string> parsedArgs)
        {
            parsedArgs = new List<string>();
            if (!text.StartsWith(CommandString))
                return ParseCommandResult.InvalidCommand;

            var parts = text.Split(WhiteChars, StringSplitOptions.RemoveEmptyEntries);
            
            parsedArgs.AddRange(parts.Skip(1));

            if (parts[0] != CommandString)
                return ParseCommandResult.InvalidCommand;
            if (parsedArgs.Count < paramCount)
                return ParseCommandResult.TooFewParams;
            if (parsedArgs.Count > paramCount)
                return ParseCommandResult.TooMuchParams;
            return ParseCommandResult.Valid;
        }
    }
}
