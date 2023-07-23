using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using RedisListener;
using RedisListener.Config;

[assembly: WebJobsStartup(typeof(RedisListenerStartup))]

namespace RedisListener
{
	internal class RedisListenerStartup : IWebJobsStartup
	{
		public void Configure(IWebJobsBuilder builder)
		{
			builder.AddExtension<RedisExtensionConfigProvider>();
		}
	}
}