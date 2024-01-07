using RabbitMQ.Client;
using System.Text;


////////////////////////////////////////////
/// Create RabbitMQ Connection
////////////////////////////////////////////

// Create a connection factory and set the Uri to the RabbitMQ server URI (amqp://guest:guest@localhost:5672)
ConnectionFactory factory = new();
factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
factory.ClientProvidedName = "Rabbit Sender App";

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


////////////////////////////////////////////
/// Send Message
////////////////////////////////////////////

for (int i = 0; i < 60; i++)
{
    Console.WriteLine($"Sending message: {i}");
    byte[] messageBodyBytes = Encoding.UTF8.GetBytes( $"Message: #{i}");
    channel.BasicPublish(exchangeName, routingKey, null, messageBodyBytes);
    Thread.Sleep(1000); 
}

// Close the channel and the connection
channel.Close();
cnn.Close();