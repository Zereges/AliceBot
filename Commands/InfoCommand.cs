using System;
using System.Collections.Generic;
using System.Diagnostics;
using Matrix;

namespace AliceBot.Commands
{
    internal class InfoCommand : BaseCommand, IMessageHandler
    {
        private static readonly string CommitHash;

        static InfoCommand()
        {
            var parts = FileVersionInfo.GetVersionInfo(typeof(InfoCommand).Assembly.Location).ProductVersion.Split('-');
            CommitHash = parts[1];
        }

        public InfoCommand(Alice alice) : base(alice, "!info", 0)
        {
        }

        public void HandleMessage(Jid room, string user, string server, string text, DateTime time, bool delay)
        {
            if (delay)
                return;

            var result = ParseCommandArgs(text, out List<string> args);
            if (result == ParseCommandResult.InvalidCommand)
                return;

            alice.SendGroupMessage(room, $"I am {alice.NickName}, you can look at my guts here: https://github.com/Zereges/AliceBot (operating at commit {CommitHash})");
        }
    }
}
