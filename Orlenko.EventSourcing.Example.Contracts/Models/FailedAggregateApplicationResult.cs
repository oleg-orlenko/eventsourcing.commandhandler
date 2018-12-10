namespace Orlenko.EventSourcing.Example.Contracts.Models
{
    public class FailedAggregateApplicationResult : AggregateApplicationResult
    {
        public readonly string Error;

        public FailedAggregateApplicationResult(string error)
        {
            Error = error;
        }
    }
}
