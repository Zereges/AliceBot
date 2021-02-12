using System;
using AliceBot.Commands;

namespace AliceBot
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var aliceTask = Alice.CreateAsync("server.com", "user", "pass", "AliceBot");
            using var alice = aliceTask.Result;

            alice.RegisterCommand(new SeenCommand(alice));
            alice.RegisterCommand(new UptimeCommand(alice));

            alice.JoinRoom("group@conference.server.com");

            while (Console.ReadLine() != null)
                ; // loop
        }
    }
}