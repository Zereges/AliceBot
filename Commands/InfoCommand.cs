using System;
using System.Collections.Generic;
using System.Diagnostics;
using Matrix;

namespace AliceBot.Commands
{
    internal class InfoCommand : BaseCommand
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

        protected override void HandleCommand(Jid room, string user, DateTime time, string[] args)
        {
            alice.SendGroupMessage(room, $"I am {alice.NickName}, you can look at my guts here: https://github.com/Zereges/AliceBot (operating at commit {CommitHash})");
        }
    }
}
