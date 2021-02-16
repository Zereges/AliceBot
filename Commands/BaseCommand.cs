using System;
using System.Collections.Generic;
using System.Linq;
using Matrix;

namespace AliceBot.Commands
{
    public abstract class BaseCommand : IMessageHandler
    {
        protected readonly Alice alice;
        public string CommandString { get; }
        private readonly int paramCount;

        protected BaseCommand(Alice alice, string commandString, int paramCount)
        {
            this.alice = alice;
            CommandString = commandString;
            this.paramCount = paramCount;
        }

        private static readonly char[] WhiteChars = {' ', '\t'};
        public void HandleMessage(Jid room, string user, string server, string text, DateTime time, bool delay)
        {
            if (!text.StartsWith(CommandString) || delay)
                return;

            var parts = text.Split(WhiteChars, paramCount + 1, StringSplitOptions.RemoveEmptyEntries);

            if (parts[0] != CommandString)
                return;

            HandleCommand(room, user, time, parts);
        }

        protected abstract void HandleCommand(Jid room, string user, DateTime time, string[] args);
    }
}
