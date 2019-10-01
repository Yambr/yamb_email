namespace Yambr.Email.Processor
{
    public static class RabbitMQConstants
    {
     
        public const string QueueMailboxDownload = "mailbox-download-queue";

        public const string RoutingKeyEmailEventCreated = "email.event.created";
        public const string QueueEmailCreated = "email-created-queue";


        public const string ExchangeEmail = "email";

        public const string QueueMailboxEvents = "mailbox-events-queue";
        public const string RoutingKeyMailboxErrorLoading = "mailbox.event.error";
        public const string RoutingKeyMailboxSuccessLoading = "mailbox.event.success";
    }
}
