using System;
using System.Collections.Generic;
using Matrix;

namespace AliceBot.Commands
{
    internal class WhoCommand : BaseCommand, IMessageHandler
    {
        public WhoCommand(Alice alice) : base(alice, "!who", 1)
        {
        }

        public void HandleMessage(Jid room, string user, string server, string text, DateTime time, bool delay)
        {
            if (delay)
                return;

            var result = ParseCommandArgs(text, out List<string> args);
            if (result == ParseCommandResult.InvalidCommand)
                return;

            if (result == ParseCommandResult.TooFewParams)
                alice.SendGroupMessage(room, $"I am AliceBot, you can look at my guts here: https://github.com/AliceBot (at commit X)");
        }
    }
}