using Microsoft.Extensions.DependencyInjection;
using redis_client.Model;
using RedisClient.IoC;
using RedisClient.Service.Interface;
using System;
using System.Diagnostics;

namespace RedisClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceProvider serviceProvider = DependencyResolve.Resolve();

            ICacheService _cacheService = serviceProvider.GetService<ICacheService>();

            const string exitCode = "exit";

            var watch = new Stopwatch();

            while (true)
            {
                Console.WriteLine($"Type any key to save/read to the redis cache or {exitCode} to exit: ");

                string inputText = Console.ReadLine();

                if (inputText.Trim() == string.Empty)
                {
                    continue;
                }
                else if (inputText.Trim() == exitCode)
                {
                    break;
                }

                watch.Start();

                var reponse = _cacheService
                                    .GetOrSet<ItemKey>(inputText, () =>
                                                   {
                                                       return ItemKey.Create(inputText); //Can be your database read
                                                   });
                watch.Stop();

                Console.WriteLine(reponse);

                Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} - Runtime {watch.ElapsedMilliseconds} milliseconds");
            }
        }
    }
}
