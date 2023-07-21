using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
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


		[Function("http listener")]
		public static async Task<HttpResponseData> HandleHttp([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "test")] HttpRequestData data)
		{
			await Task.Yield();
			return data.CreateResponse(HttpStatusCode.AlreadyReported);
		}
	}
}