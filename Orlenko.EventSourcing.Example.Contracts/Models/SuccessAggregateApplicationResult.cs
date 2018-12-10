
namespace Orlenko.EventSourcing.Example.Contracts.Models
{
    public class SuccessAggregateApplicationResult : AggregateApplicationResult
    {
        public readonly int CurrentAggregateVersion;

        public SuccessAggregateApplicationResult(int version)
        {
            this.CurrentAggregateVersion = version;
        }
    }
}
