using StackExchange.Redis;

namespace RedisListener.Context
{
	internal class RedisTriggerContext
	{
		public IConnectionMultiplexer ConnectionMultiplexer { get; set; }

		public RedisTriggerAttribute RedisTriggerAttribute { get; set; }

		public RedisTriggerContext(
			IConnectionMultiplexer connectionMultiplexer,
			RedisTriggerAttribute redisTriggerAttribute)
		{
			ConnectionMultiplexer = connectionMultiplexer;
			RedisTriggerAttribute = redisTriggerAttribute;
		}
	}
}