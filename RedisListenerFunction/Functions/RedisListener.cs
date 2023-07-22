using Microsoft.Azure.Functions.Worker;
using RedisListener.WorkerBinding;
using StackExchange.Redis;

namespace RedisListenerFunction.Functions
{
	public static class RedisListener
	{
		[Function("RedisConsumer")]
		public static async Task HandleRedisMessage([RedisTrigger("localhost:5789", "test1234")] StreamEntry entry)
		{
			await Task.Yield();
			Console.WriteLine(entry.ToString());
		}
	}
}