using Microsoft.Azure.Functions.Worker;
using RedisListener.Context;
using StackExchange.Redis;

namespace RedisListenerFunction.Functions
{
	public class RedisListener
	{
		[Function("RedisConsumer")]
		public async Task HandleRedisMessage(
			[RedisTrigger("localhost:5789", "test1234")]
			StreamEntry entry)
		{
			await Task.Yield();
			Console.WriteLine("Hello world");
		}
	}
}