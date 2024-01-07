using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

////////////////////////////////////////////
/// Create RabbitMQ Connection
////////////////////////////////////////////

// Create a connection factory and set the Uri to the RabbitMQ server URI (amqp://guest:guest@localhost:5672)
ConnectionFactory factory = new();
factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
factory.ClientProvidedName = "Rabbit Receiver2 App";

IConnection cnn = factory.CreateConnection();

IModel channel = cnn.CreateModel();

string exchangeName = "Demo Exchange";
string routingKey = "demo-routing-key";
string queueName = "DemoQueue";

// The channel.ExchangeDeclare method is used to declare an exchange. 
channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
// The channel.QueueDeclare method is used to declare a queue. 
channel.QueueDeclare(queueName, false, false, false, null);
// The channel.QueueBind method is used to bind the queue to the exchange.
channel.QueueBind(queueName, exchangeName, routingKey, null);
channel.BasicQos(0, 1, false);


////////////////////////////////////////////
/// Receive Message
/// ////////////////////////////////////////
var consumer = new EventingBasicConsumer(channel);
consumer.Received += (sender, args) =>
{
    Task.Delay(TimeSpan.FromSeconds(3)).Wait();
    var body = args.Body.ToArray();
    string message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Received message: {message}");
    channel.BasicAck(args.DeliveryTag, false);
};

string consumerTag = channel.BasicConsume(queueName, false, consumer);

Console.ReadLine();

channel.BasicCancel(consumerTag);

channel.Close();
cnn.Close();