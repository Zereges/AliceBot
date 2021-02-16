using System;
using System.Collections.Generic;
using AliceBot.Commands;
using Matrix;
using Matrix.Xmpp;

namespace AliceBot
{
    internal class SeenCommand : BaseCommand, IPresenceHandler
    {
        private readonly Dictionary<string, DateTime?> times = new Dictionary<string, DateTime?>();

        public SeenCommand(Alice alice) : base(alice, "!seen", 1)
        {
        }

        public void HandlePresence(Jid room, string user, string server, PresenceType type, DateTime time)
        {
            user = user.ToLower();
            if (type == PresenceType.Available)
                times[user] = null;
            else
                times[user] = time;
        }

        protected override void HandleCommand(Jid room, string user, DateTime time, string[] args)
        {
            if (args.Length < 2)
            {
                alice.SendGroupMessage(room, "Seen whom? Try !seen <user>");
                return;
            }

            string requestedUser = args[1];
            if (!times.TryGetValue(requestedUser.ToLower(), out var joinTime))
                alice.SendGroupMessage(room, $"Haven't seen them so far.{(alice.SupportsCommand("!uptime") ? " (BTW, I can tell you my !uptime)" : "")}");
            else if (!joinTime.HasValue)
                alice.SendGroupMessage(room, $"I can see {requestedUser} in the room :)");
            else
                alice.SendGroupMessage(room, $"I've seen {requestedUser} leaving at {joinTime.Value} ({Utils.ToHumanReadable(DateTime.Now - joinTime.Value)} ago)");
        }
    }
}
