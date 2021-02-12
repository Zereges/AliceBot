using System;
using System.Collections.Generic;
using AliceBot.Commands;
using Matrix;
using Matrix.Xmpp;

namespace AliceBot
{
    internal class SeenCommand : BaseCommand, IPresenceHandler, IMessageHandler
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

        public void HandleMessage(Jid room, string user, string server, string text, DateTime time, bool delay)
        {
            if (delay)
                return;

            var result = ParseCommandArgs(text, out var args);
            if (result == ParseCommandResult.InvalidCommand)
                return;
            if (result == ParseCommandResult.TooFewParams)
                alice.SendGroupMessage(room, "Seen whom? Try !seen <user>");
            if (result != ParseCommandResult.Valid)
                return;

            if (!times.TryGetValue(args[0].ToLower(), out var joinTime))
                alice.SendGroupMessage(room, $"Haven't seen them so far.{(alice.SupportsCommand<UptimeCommand>() ? " (BTW, I can tell you my !uptime)" : "")}");
            else if (!joinTime.HasValue)
                alice.SendGroupMessage(room, $"I can see {args[0]} in the room :)");
            else
                alice.SendGroupMessage(room, $"I've seen {user} leaving at {joinTime.Value} ({Utils.ToHumanReadable(DateTime.Now - joinTime.Value)} ago)");
        }
    }
}
