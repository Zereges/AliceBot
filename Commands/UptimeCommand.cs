using System;
using System.Collections.Generic;
using Matrix;

namespace AliceBot.Commands
{
    internal class UptimeCommand : BaseCommand, IMessageHandler
    {
        public UptimeCommand(Alice alice) : base(alice, "!uptime", 0)
        {
        }

        public void HandleMessage(Jid room, string user, string server, string text, DateTime time, bool delay)
        {
            if (delay)
                return;

            var result = ParseCommandArgs(text, out List<string> args);
            if (result == ParseCommandResult.InvalidCommand)
                return;

            var joinTime = alice.GetRoomJoinTime(room);
            alice.SendGroupMessage(room, $"Present since {joinTime} ({Utils.ToHumanReadable(DateTime.Now - joinTime)} ago)");
        }
    }
}
