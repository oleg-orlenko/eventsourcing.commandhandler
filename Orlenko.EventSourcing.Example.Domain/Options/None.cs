using System;
using System.Threading.Tasks;

namespace Orlenko.EventSourcing.Example.Domain.Options
{
    public class None<TResult> : IOption<TResult>
    {
        public bool IsSome => false;

        public bool IsNone => true;

        public TMatchResult Match<TMatchResult>(TMatchResult nothing, Func<TResult, TMatchResult> some)
        {
            return nothing;
        }

        public TMatchResult Match<TMatchResult>(Func<TMatchResult> nothing, Func<TResult, TMatchResult> some)
        {
            var result = nothing();
            return result;
        }

        public IOption<TOther> Map<TOther>(Func<TResult, TOther> some)
        {
            return new None<TOther>();
        }

        public IOption<Tuple<TResult, TSecondResult>> Both<TSecondResult>(IOption<TSecondResult> second)
        {
            return new None<Tuple<TResult, TSecondResult>>();
        }

        public Task<TMatchResult> MatchAsync<TMatchResult>(TMatchResult nothing, Func<TResult, Task<TMatchResult>> some)
        {
            return Task.FromResult(nothing);
        }
    }
}
