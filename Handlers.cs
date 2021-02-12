using System;
using Matrix;
using Matrix.Xmpp;

namespace AliceBot
{
    public interface IPresenceHandler
    {
        void HandlePresence(Jid room, string user, string server, PresenceType type, DateTime time);
    }

    public interface IMessageHandler
    {
        void HandleMessage(Jid room, string user, string server, string text, DateTime time, bool delay);
    }

//    /*
    internal class DumpPresenceHandler : IPresenceHandler
    {
        public void HandlePresence(Jid room, string user, string server, PresenceType type, DateTime time)
        {
            if (!string.IsNullOrEmpty(user))
                Console.WriteLine($"({time}) {user} in {room.User} is now {type}");
        }
    }
    /*
    internal class DumpMessageHandler : IMessageHandler
    {
        public void HandleMessage(Jid room, string user, string server, string text, DateTime time, bool delay)
        {
            Console.WriteLine($"({time} {(delay ? "old" : "cur")}) {user} in {room.User}: {text}");
        }
    }
    */
}
