using System;
using System.Globalization;
using System.IO;
using System.Threading;
using AuthServer.Database;
using AuthServer.Database.Interfaces;
using AuthServer.Network;
using Digital_World;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Yggdrasil.Database;
using Yggdrasil.Database.Interfaces;
using Yggdrasil.Network.Interfaces;

namespace AuthServer
{
    public class Program
    {
        private static IServer _authServer;
        public static Thread AuthServThread;
        
		public static void Main(string[] args)
		{
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture; // Use invariant culture - we have to set it explicitly for every thread we create.

            var host = Services();
            _authServer = ActivatorUtilities.CreateInstance<AuthServ>(host.Services);
            StartAuth();
			
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
		
		public static bool StartAuth()
        {
            AuthServThread = new Thread(_authServer.Run) { IsBackground = true, CurrentCulture = CultureInfo.InvariantCulture };
            AuthServThread.Start();

            return true;
        }

        public static bool StopAuth()
        {
            SysCons.LogInfo("Stopping AuthServer..");
            _authServer.Shutdown();
            _authServer = null;

            return true;
        }
        
        public static void Shutdown()
        {
            if (_authServer != null)
            {
                SysCons.LogInfo("Shutting down AuthServer..");
                _authServer.Shutdown();
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
                services.AddTransient<IServer, AuthServ>();
                services.AddTransient<IDatabaseContext, BaseDB>();
                services.AddTransient<ILoginDatabase, AuthDB>();
            }).Build();
            return host;
        }
	}
}
