using System;
namespace consul_dotnet.Options
{
	public class AppSettings
	{
		public AppSettings()
		{
		}

		public Service Service { get; set; }
		public ConsulServer ConsulServer { get; set; }

	}

	public class Service
	{
		public string Name { get; set; }
		public string Port { get; set; }
	}


	public class ConsulServer
	{
		public string IP { get; set; }
		public string Port { get; set; }
	}
}


