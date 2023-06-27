using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using RedisListener.Context;
using StackExchange.Redis;

namespace RedisListenerInProcess.Functions
{
	public class RedisListener
	{
		[FunctionName("RedisHandler")]
		public async Task HandleRedis(
			[RedisTrigger("localhost:5789", "test1234")]
			StreamEntry entry)
		{
			await Task.Yield();
			Console.WriteLine("Hello world");
		}
	}
}