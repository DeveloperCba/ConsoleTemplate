using ConsoleAppDesignPattern.SpecificationPattern.NotNuget.Domain.Interfaces;

namespace ConsoleAppDesignPattern.SpecificationPattern.NotNuget.Domain.Implementations
{
    public class ExpressionSpecification<T> : CompositeSpecification<T>
    {
        private readonly Func<T, bool> _expression;

        public ExpressionSpecification(Func<T, bool> expression)
        {
            _expression = expression ?? throw new ArgumentException();
        }

        public override bool IsSatisfiedBy(T candidate)
        {
            return _expression(candidate);
        }
    }
}


