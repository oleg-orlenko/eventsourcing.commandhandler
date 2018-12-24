namespace Orlenko.EventSourcing.Example.Contracts.Models
{
    public class ItemAlreadyExistsApplicationResult : AggregateApplicationResult
    {
        public readonly string Message;

        public ItemAlreadyExistsApplicationResult(string message)
        {
            this.Message = message;
        }
    }
}
