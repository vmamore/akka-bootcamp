using Akka.Actor;

namespace DoThis
{
    public class ValidationActor : UntypedActor
    {
        private readonly IActorRef _consoleWriterActor;

        public ValidationActor(IActorRef consoleWriterActor) => _consoleWriterActor = consoleWriterActor;

        protected override void OnReceive(object message)
        {
            var msg = message as string;

            if (string.IsNullOrEmpty(msg))
            {
                _consoleWriterActor.Tell(new Messages.NullInputError("No input received."));
            }
            else
            {
                if (IsValid(msg))
                {
                    _consoleWriterActor.Tell(new Messages.InputSuccess("Thank you! Message was valid."));
                }
                else
                {
                    _consoleWriterActor.Tell(new Messages.ValidationError("Invalid: input had odd number of characters"));
                }
            }

            Sender.Tell(new Messages.ContinueProcessing());

        }

        public static bool IsValid(string msg) => msg.Length % 2 == 0;
    }
}