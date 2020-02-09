using System;
using System.Threading.Tasks;

namespace Orlenko.EventSourcing.Example.Domain.Options
{
    public interface IOption<TResult>
    {
        bool IsSome { get; }

        bool IsNone { get; }

        IOption<TOther> Map<TOther>(Func<TResult, TOther> some);

        TMatchResult Match<TMatchResult>(TMatchResult nothing, Func<TResult, TMatchResult> some);

        TMatchResult Match<TMatchResult>(Func<TMatchResult> nothing, Func<TResult, TMatchResult> some);

        Task<TMatchResult> MatchAsync<TMatchResult>(TMatchResult nothing, Func<TResult, Task<TMatchResult>> some);

        IOption<Tuple<TResult, TSecondResult>> Both<TSecondResult>(IOption<TSecondResult> second);
    }
}
