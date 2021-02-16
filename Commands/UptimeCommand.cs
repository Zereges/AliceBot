using System;
using Matrix;

namespace AliceBot.Commands
{
    internal class UptimeCommand : BaseCommand
    {
        public UptimeCommand(Alice alice) : base(alice, "!uptime", 0)
        {
        }

        protected override void HandleCommand(Jid room, string user, DateTime time, string[] args)
        {
            var joinTime = alice.GetRoomJoinTime(room);
            alice.SendGroupMessage(room, $"Present since {joinTime} ({Utils.ToHumanReadable(DateTime.Now - joinTime)} ago)");
        }
    }
}
