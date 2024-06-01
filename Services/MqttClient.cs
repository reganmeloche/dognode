using HiveMQtt.Client;
using Microsoft.Extensions.Options;

namespace DogNode.Services
{
    public class MqttOptions {
        public string Host {get; set;}
        public int Port {get;set;}
        public string Username {get;set;}
        public string Password {get;set;}
        public bool UseTls {get;set;}
    }

    public interface IMqttClient {
        public Task Send(string message);
    }

    public class MqttClient : IMqttClient
    {
        private readonly HiveMQClient _client;

        public MqttClient(IOptions<MqttOptions> options) {
            var optValue = options.Value;

            var mqttOptions = new HiveMQClientOptionsBuilder().
                    WithBroker(optValue.Host).
                    WithPort(optValue.Port).
                    WithUseTls(optValue.UseTls).
                    WithUserName(optValue.Username).
                    WithPassword(optValue.Password).
                    Build();
            _client = new HiveMQClient(mqttOptions);

            // _client.OnMessageReceived += (sender, args) =>
            // {
            //     Console.WriteLine("Message Received: {}", args.PublishMessage.PayloadAsString);
            // };
        }

        public async Task Send(string message) {
            // Connect to broker
            var connectResult = await _client.ConnectAsync().ConfigureAwait(false);

            // Configure the subscriptions we want and subscribe
            // var builder = new SubscribeOptionsBuilder();
            // builder.WithSubscription("topic1", QualityOfService.AtLeastOnceDelivery)
            //     .WithSubscription("topic2", QualityOfService.ExactlyOnceDelivery);
            // var subscribeOptions = builder.Build();
            // var subscribeResult = await _client.SubscribeAsync(subscribeOptions);

            // Publish a message
            var publishResult = await _client.PublishAsync("sample", message);
        }
    }
}