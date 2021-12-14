namespace DeltaWare.Dependencies.Benchmark.Mocking
{
    internal class MessagingService : IMessagingService
    {
        public MessagingService(IEmailService email, IPostalService postal)
        {
        }
    }
}