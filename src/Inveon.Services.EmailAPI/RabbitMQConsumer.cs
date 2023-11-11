
using Inveon.Services.EmailAPI;
using MailKit.Net.Smtp;
using MimeKit;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Inveon.Services.EmailAPI
{
    public class RabbitMQConsumer : BackgroundService
    {
        private readonly ILogger<RabbitMQConsumer> _logger;
        private IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQConsumer(ILogger<RabbitMQConsumer> logger)
        {
            _logger = logger;
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = "localhost",
                    UserName = "guest",
                    Password = "guest"
                };
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.QueueDeclare(queue: "checkoutqueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            }
            catch (Exception)
            {
                //log exception
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (model, e) =>
            {
                var body = e.Body;
                var json = System.Text.Encoding.UTF8.GetString(body.ToArray());
                MessageSender messageDto = JsonConvert.DeserializeObject<MessageSender>(json);
                sendMessage(messageDto);
            };
            _channel.BasicConsume("checkoutqueue", false, consumer);

           
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }

        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {

            return base.StopAsync(cancellationToken);
        }


        public void sendMessage(MessageSender messageSender)
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("Inveon", "fehem@hotmail.com"));

            message.To.Add(MailboxAddress.Parse(messageSender.Email));
            String email = prepareEmailContent(messageSender);
            message.Subject = "Şipariş Detayları";
            message.Body = new TextPart("body")
            {
                Text = email
            };

            SmtpClient client = new SmtpClient();
            try
            {
                client.Connect("smtp.gmail.com", 465, true);
                client.Authenticate("fehem@hotmail.com", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9");
                client.Send(message);

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();



            }


        }

        public String prepareEmailContent(MessageSender messageSender)
        {
            String email =
            @$" {messageSender.FirstName} {messageSender.LastName},Siparisiniz onaylanmıstır.

                                    {messageSender.OrderTotal}
                                    {messageSender.DiscountTotal}
                                    {messageSender.PickupDateTime.ToString("MM/dd/yyyy h:mm tt")}
                                    {messageSender.Phone}
                                    {messageSender.Email}";
            return email;


        }
    }
}
