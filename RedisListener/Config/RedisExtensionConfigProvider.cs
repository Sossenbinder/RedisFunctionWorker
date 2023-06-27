using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.Configuration;
using RedisListener.Binding;
using RedisListener.Context;
using StackExchange.Redis;
using System.Data;
using System;
using System.Text;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using System.Diagnostics;

namespace RedisListener.Config
{
	internal class PocoToBytesConverter<T> : IConverter<T, ReadOnlyMemory<byte>>
	{
		public ReadOnlyMemory<byte> Convert(T input)
		{
			_ = input ?? throw new ArgumentNullException(nameof(input));

			Debugger.Launch();
			Debugger.Break();
			string res = JsonConvert.SerializeObject(input);
			return Encoding.UTF8.GetBytes(res);
		}
	}

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
			triggerRule.AddConverter<string, ReadOnlyMemory<byte>>(arg => Encoding.UTF8.GetBytes(arg));
			triggerRule.AddConverter<byte[], ReadOnlyMemory<byte>>(arg => arg);
			triggerRule.BindToTrigger(new RedisListenerTriggerBindingProvider(this));
			triggerRule.AddOpenConverter<OpenType.Poco, ReadOnlyMemory<byte>>(typeof(PocoToBytesConverter<>));
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