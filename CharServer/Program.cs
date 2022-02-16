using System;
using System.Globalization;
using System.IO;
using System.Threading;
using CharServer.Database;
using CharServer.Database.Interfaces;
using CharServer.Network;
using Digital_World;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Yggdrasil.Database;
using Yggdrasil.Database.Interfaces;
using Yggdrasil.Network.Interfaces;

namespace CharServer
{
    class Program
    {
        private static IServer _characterServer;
        public static Thread LobbyServerThread;

        public static void Main(string[] args)
        {
            // Watch for unhandled exceptions
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;

            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture; // Use invariant culture - we have to set it explicitly for every thread we create.

            var host = Services();
            _characterServer = ActivatorUtilities.CreateInstance<CharServ>(host.Services);
            StartServer();

            while (true)
            {
                Console.ReadKey(true);
            }
        }

        private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.IsTerminating)
                SysCons.LogError("Terminating because of unhandled exception.");
            else
                SysCons.LogError("Caught unhandled exception.");
            Console.ReadLine();
        }

        public static bool StartServer()
        {
            LobbyServerThread = new Thread(_characterServer.Run) { IsBackground = true, CurrentCulture = CultureInfo.InvariantCulture };
            LobbyServerThread.Start();

            return true;
        }

        public static bool StopServer()
        {
            SysCons.LogInfo("Stopping CharServer..");
            _characterServer.Shutdown();
            LobbyServerThread.Abort();
            return true;
        }

        public static void Shutdown()
        {
            if (_characterServer != null)
            {
                SysCons.LogInfo("Shutting down CharServer..");
                _characterServer.Shutdown();
            }
            Environment.Exit(0);
        }
        
        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
        }

        
        static IHost Services()
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            var host = Host.CreateDefaultBuilder().ConfigureServices((context, services) =>
            {
                services.AddTransient<IServer, CharServ>();
                services.AddTransient<IDatabaseContext, BaseDB>();
                services.AddTransient<ICharacterDatabase, CharDB>();
            }).Build();
            return host;
        }
    }
}
