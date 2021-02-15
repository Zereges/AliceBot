using System;
using System.IO;
using System.Reflection;
using AliceBot.Commands;

namespace AliceBot
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(Path.GetFullPath(args[0]));
            string server, username, password, room;
            try
            {
                if (args.Length != 1)
                {
                    Console.WriteLine($"Usage: dotnet {Assembly.GetExecutingAssembly().GetName().Name} alice.conf");
                    return;
                }

                using StreamReader reader = new StreamReader(args[0]);
                server = reader.ReadLine();
                username = reader.ReadLine();
                password = reader.ReadLine();
                room = reader.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            var aliceTask = Alice.CreateAsync(server, username, password, "AliceBot");
            using var alice = aliceTask.Result;

            alice.RegisterCommand(new SeenCommand(alice));
            alice.RegisterCommand(new UptimeCommand(alice));
            alice.RegisterCommand(new InfoCommand(alice));

            alice.JoinRoom("testbot@conference.server.com");

            while (Console.ReadLine() != null)
                ; // loop
        }
    }
}
