namespace DogNode.Services
{
    public enum NodeAction {
        SOUND,
        BUZZ
    }

    public record ProcessResult(string Message, bool Passed = false);

    public interface IProcessInput {
        Task<ProcessResult> Process(string input, NodeAction action);
    }

    public class InputProcessor : IProcessInput
    {
        private readonly string _storedPassword;
        private readonly IMqttClient _mqttClient;

        public InputProcessor(IConfiguration configuration, IMqttClient mqttClient)
        {
            _storedPassword = configuration["SecretInfo:Value"] ?? throw new ArgumentNullException("password");
            _mqttClient = mqttClient;
        }

        public async Task<ProcessResult> Process(string input, NodeAction action)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return new ProcessResult("Input was empty or null!");
            }

            if (!input.Equals(_storedPassword)) {
                return new ProcessResult("Wrong password");
            }
            
            string mqttMessage = action switch
            {
                NodeAction.SOUND => "S",
                NodeAction.BUZZ => "V",
                _ => throw new Exception("Invalid action type")
            };

            await _mqttClient.Send(mqttMessage);

            var message = $"Sent {action.ToString()} at {DateTime.Now}";

            return new ProcessResult(message, true); 
        }

    }
}