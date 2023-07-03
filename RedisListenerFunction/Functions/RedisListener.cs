using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using RedisListener.Model;
using RedisListener.WorkerBinding;
using StackExchange.Redis;

namespace RedisListenerFunction.Functions
{
	public static class RedisListener
	{
		[Function("RedisConsumer")]
		public static async Task HandleRedisMessage([RedisTrigger("localhost:5789", "test1234")] string entry)
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

		[Function("rabbit")]
		public static string Run([RabbitMQTrigger("queue", ConnectionStringSetting = "rabbitMQConnectionAppSetting")] string item,
			FunctionContext context)
		{
			var logger = context.GetLogger("RabbitMQFunction");

			logger.LogInformation(item);

			var message = $"Output message created at {DateTime.Now}";
			return message;
		}
	}
}