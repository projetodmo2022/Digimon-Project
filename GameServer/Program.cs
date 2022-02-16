using System;
using System.Globalization;
using System.IO;
using System.Threading;
using Digital_World;
using GameServer.Database;
using GameServer.Database.Interfaces;
using GameServer.Network;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Yggdrasil.Database;
using Yggdrasil.Database.Interfaces;
using Yggdrasil.Network.Interfaces;

namespace GameServer
{
    class Program
	{
        public static GameServ GServ;
        public static Thread GameServerThread;

        private static IServer _gameServer;
        
		public static void Main(string[] args)
		{
            // Watch for unhandled exceptions
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;

            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture; // Use invariant culture - we have to set it explicitly for every thread we create.
			
            var host = Services();
            _gameServer = ActivatorUtilities.CreateInstance<GameServ>(host.Services);
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
            GameServerThread = new Thread(_gameServer.Run) { IsBackground = true, CurrentCulture = CultureInfo.InvariantCulture };
            GameServerThread.Start();

            return true;
        }

        public static bool StopServer()
        {
            SysCons.LogInfo("Stopping GameServer..");
            _gameServer.Shutdown();
            GameServerThread.Abort();
            return true;
        }
        
        public static void Shutdown()
        {
            if (_gameServer != null)
            {
                SysCons.LogInfo("Shutting down GameServer..");
                _gameServer.Shutdown();
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
                services.AddTransient<IServer, GameServ>();
                services.AddTransient<IDatabaseContext, BaseDB>();
                services.AddTransient<IGameDatabase, GameDB>();
            }).Build();
            return host;
        }
	}
}
