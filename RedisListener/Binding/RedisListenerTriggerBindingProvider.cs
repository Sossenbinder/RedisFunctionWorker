using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Triggers;
using RedisListener.Config;
using RedisListener.Context;
using StackExchange.Redis;

namespace RedisListener.Binding
{
	// Provides the binding of a listener
	internal class RedisListenerTriggerBindingProvider : ITriggerBindingProvider
	{
		private readonly RedisExtensionConfigProvider _redisExtensionConfigProvider;

		public RedisListenerTriggerBindingProvider(RedisExtensionConfigProvider redisExtensionConfigProvider)
		{
			_redisExtensionConfigProvider = redisExtensionConfigProvider;
		}

		public async Task<ITriggerBinding> TryCreateAsync(TriggerBindingProviderContext context)
		{
			var redisTriggerAttribute = context.Parameter.GetCustomAttribute<RedisTriggerAttribute>(false);

			return new RedisListenerTriggerBinding(
				new RedisTriggerContext(
					await _redisExtensionConfigProvider.CreateMultiplexer(redisTriggerAttribute),
					redisTriggerAttribute),
				typeof(StreamEntry));
		}
	}
}