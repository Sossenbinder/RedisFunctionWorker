using Microsoft.Azure.WebJobs;
using RedisListener.Config;

namespace RedisListener
{
	public static class IWebJobsBuilderExtensions
	{
		public static IWebJobsBuilder AddRedisListener(this IWebJobsBuilder builder)
		{
			builder.AddExtension<RedisExtensionConfigProvider>();
			return builder;
		}
	}
}