using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using AliceBot.Commands;
using Matrix;
using Matrix.Xml;
using Matrix.Xmpp;
using Matrix.Xmpp.Client;

namespace AliceBot
{
    public class Alice : IDisposable
    {
        private class RemoveSelfFromRoomsInternalHandler : IPresenceHandler
        {
            private readonly Alice alice;
            public RemoveSelfFromRoomsInternalHandler(Alice alice)
            {
                this.alice = alice;
            }

            public void HandlePresence(Jid room, string user, string server, PresenceType type, DateTime time)
            {
                if (user == alice.NickName && type == PresenceType.Unavailable)
                    alice.roomsList.Remove(room);
            }
        }

        // ToDo: sync local time with server time

        public string NickName { get; }
        private readonly XmppClient xmppClient;
        private readonly MucManager mucManager;
        private readonly Dictionary<string, DateTime> roomsList = new Dictionary<string, DateTime>();

        public readonly List<IPresenceHandler> presenceHandlers = new List<IPresenceHandler>();
        private readonly List<IMessageHandler> messageHandlers = new List<IMessageHandler>();

        public static async Task<Alice> CreateAsync(string server, string username, string password, string nickName)
        {
            Alice alice = new Alice(server, username, password, nickName);
            await alice.xmppClient.ConnectAsync();
            Console.WriteLine("XMPP Connection Established.");
            //var roster = await alice.xmppClient.RequestRosterAsync();

            alice.xmppClient.XmppXElementStreamObserver
                .Where(el => el is Presence)
                .Subscribe(el => alice.PresenceHandler(el.Cast<Presence>()));

            alice.presenceHandlers.Add(new RemoveSelfFromRoomsInternalHandler(alice));

            return alice;
        }

        private Alice(string server, string username, string password, string nickName)
        {
            xmppClient = new XmppClient
            {
                XmppDomain = server,
                Username = username,
                Password = password,
            };
            mucManager = new MucManager(xmppClient);
            NickName = nickName;
        }

        public bool JoinRoom(string roomId)
        {
            mucManager.EnterRoomAsync(roomId, NickName).Wait(); // ToDo: check error, return false

            xmppClient.XmppXElementStreamObserver
                .Where(el => el is Message message && message.Type == MessageType.GroupChat && message.From.Bare == roomId)
                .Subscribe(el => GroupMessageHandler(el.Cast<Message>()));

            roomsList.Add(roomId, DateTime.Now);
            return true;
        }

        public void SendGroupMessage(Jid room, string text)
        {
            var msg = new Message
            {
                Type = MessageType.GroupChat,
                To = room,
                Body = text
            };
            xmppClient.SendAsync(msg).Wait();
        }

        public DateTime GetRoomJoinTime(Jid room)
        {
            return roomsList[room];
        }

        public void RegisterCommand(BaseCommand command)
        {
            if (command is IPresenceHandler presenceHandler)
                presenceHandlers.Add(presenceHandler);
            if (command is IMessageHandler messageHandler)
                messageHandlers.Add(messageHandler);
        }

        public bool SupportsCommand(string commandString)
        {
            foreach (var messageHandler in messageHandlers)
            {
                if (messageHandler is BaseCommand command && command.CommandString == commandString)
                    return true;
            }

            return false;
        }

        private void GroupMessageHandler(Message message)
        {
            try
            {
                foreach (var messageHandler in messageHandlers)
                {
                    if (message.Body != null)
                        messageHandler.HandleMessage(message.From.Bare, message.From.Resource, message.From.Server, message.Body, message.Delay?.Stamp ?? DateTime.Now, message.Delay != null);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void PresenceHandler(Presence presence)
        {
            try
            {
                foreach (var presenceHandler in presenceHandlers)
                {
                    if (presence.Delay != null)
                    {
                        Console.WriteLine("Debug: nonnull timestamp");
                        Console.WriteLine(presence);
                    }
                    if (presence.From.Resource != null)
                        presenceHandler.HandlePresence(presence.From.Bare, presence.From.Resource, presence.From.Server, presence.Type, DateTime.Now);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task ReleaseUnmanagedResources()
        {
            var roomListClone = roomsList.Keys.ToList();
            foreach (string room in roomListClone)
                await mucManager.ExitRoomAsync(room, NickName);

            await xmppClient.DisconnectAsync();
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources().Wait();
            GC.SuppressFinalize(this);
        }

        ~Alice()
        {
            ReleaseUnmanagedResources().Wait();
        }
    }
}
