using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using RedisListener.WorkerBinding;
using StackExchange.Redis;
using System.Net;

namespace RedisListenerFunction.Functions
{
	public static class RedisListener
	{
		[Function("RedisConsumer")]
		public static void HandleRedisMessage([RedisTrigger("localhost:5789", "test1234")] StreamEntry entry)
		{
			Console.WriteLine($"{entry.Id} - {entry.Values[0].Name}:{entry.Values[0].Value}");
		}

		[Function("http listener")]
		public static async Task<HttpResponseData> HandleHttp([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "test")] HttpRequestData data)
		{
			await Task.Yield();
			return data.CreateResponse(HttpStatusCode.AlreadyReported);
		}
	}
}