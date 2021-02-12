using System;
using System.IO;
using AliceBot.Commands;

namespace AliceBot
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            string server, username, password, room;
            try
            {
                if (args.Length != 1)
                {
                    Console.WriteLine($"{System.Reflection.Assembly.GetExecutingAssembly().FullName} alice.conf");
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

            alice.JoinRoom("testbot@conference.server.com");

            while (Console.ReadLine() != null)
                ; // loop
        }
    }
}