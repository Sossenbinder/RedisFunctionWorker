using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Triggers;
using RedisListener.Config;
using RedisListener.Context;

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

		public Task<ITriggerBinding> TryCreateAsync(TriggerBindingProviderContext context)
		{
			var redisTriggerAttribute = context.Parameter.GetCustomAttribute<RedisTriggerAttribute>(false);

			return Task.FromResult<ITriggerBinding>(redisTriggerAttribute is null
				? null
				: new RedisListenerTriggerBinding(new RedisTriggerContext(_redisExtensionConfigProvider.CreateMultiplexer(redisTriggerAttribute), redisTriggerAttribute),
					context.Parameter.ParameterType));
		}
	}
}