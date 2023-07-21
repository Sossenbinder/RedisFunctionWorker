using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.Configuration;
using RedisListener.Binding;
using RedisListener.Context;
using StackExchange.Redis;

namespace RedisListener.Config
{
	internal class RedisExtensionConfigProvider : IExtensionConfigProvider
	{
		private readonly IConfiguration _configuration;

		public RedisExtensionConfigProvider(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public void Initialize(ExtensionConfigContext context)
		{
			var triggerRule = context.AddBindingRule<RedisTriggerAttribute>();
			triggerRule.BindToTrigger(new RedisListenerTriggerBindingProvider(this));
		}

		public IConnectionMultiplexer CreateMultiplexer(RedisTriggerAttribute triggerAttribute)
		{
			return ConnectionMultiplexer.Connect(GetValueOrSecretFromConfig(triggerAttribute.ConnectionString));
		}

		private string GetValueOrSecretFromConfig(string value)
		{
			if (value.StartsWith("%") && value.EndsWith("%"))
			{
				return _configuration[value[1..^1]];
			}

			return value;
		}
	}
}