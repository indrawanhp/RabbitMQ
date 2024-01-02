using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
//Here we specify the Rabbit MQ Server. we use rabbitmq docker image and use it

var factory = new ConnectionFactory
{
    HostName = "172.18.247.61",
    Port = 8001,
    UserName = "test",
    Password = "test"
};

//Create the RabbitMQ connection using connection factory details as i mentioned above
var connection = factory.CreateConnection();
//Here we create channel with session and model
var channel = connection.CreateModel();

int messageIndex = 1;

//declare the queue after mentioning name and a few property related to that
channel.QueueDeclare("product", exclusive: false);
//Set Event object which listen message from chanel which is sent by producer
var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"[{messageIndex}] Product message received: {message}");

    messageIndex++;
};
//read the message
channel.BasicConsume(queue: "product", autoAck: true, consumer: consumer);
Console.ReadKey();