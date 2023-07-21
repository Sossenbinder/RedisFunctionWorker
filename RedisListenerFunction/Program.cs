using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RedisListener.Common;

var host = new HostBuilder()
	.ConfigureFunctionsWorkerDefaults()
	.ConfigureServices(services =>
	{
		services.Configure<JsonSerializerOptions>(cfg =>
		{
			cfg.Converters.Add(new StreamEntryJsonConverter());
			cfg.Converters.Add(new NameValueEntryConverter());
		});
	})
	.Build();

host.Run();