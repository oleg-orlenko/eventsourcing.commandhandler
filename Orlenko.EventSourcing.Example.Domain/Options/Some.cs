using System;
using System.Threading.Tasks;

namespace Orlenko.EventSourcing.Example.Domain.Options
{
    public class Some<TResult> : IOption<TResult>
    {
        private readonly TResult Value;

        public Some(TResult value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            Value = value;
        }

        public bool IsSome => true;

        public bool IsNone => false;

        public TMatchResult Match<TMatchResult>(TMatchResult nothing, Func<TResult, TMatchResult> some)
        {
            return some(Value);
        }

        public TMatchResult Match<TMatchResult>(Func<TMatchResult> nothing, Func<TResult, TMatchResult> some)
        {
            return some(Value);
        }

        public IOption<TOther> Map<TOther>(Func<TResult, TOther> some)
        {
            var result = some(Value);
            return new Some<TOther>(result);
        }

        public IOption<Tuple<TResult, TSecondResult>> Both<TSecondResult>(IOption<TSecondResult> second)
        {
            var result = second.Match<IOption<Tuple<TResult, TSecondResult>>>(
                new None<Tuple<TResult, TSecondResult>>(),
                secondValue => new Some<Tuple<TResult, TSecondResult>>(new Tuple<TResult, TSecondResult>(Value, secondValue)));

            return result;
        }

        public Task<TMatchResult> MatchAsync<TMatchResult>(TMatchResult nothing, Func<TResult, Task<TMatchResult>> some)
        {
            return some(Value);
        }
    }
}
