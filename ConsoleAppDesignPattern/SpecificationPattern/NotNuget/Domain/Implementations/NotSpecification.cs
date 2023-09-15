using ConsoleAppDesignPattern.SpecificationPattern.NotNuget.Domain.Interfaces;

namespace ConsoleAppDesignPattern.SpecificationPattern.NotNuget.Domain.Implementations
{
    public class NotSpecification<T> : CompositeSpecification<T>
    {
        private readonly ISpecification<T> _notSpecification;

        public NotSpecification(ISpecification<T> not)
        {
            _notSpecification = not;
        }

        public override bool IsSatisfiedBy(T candidate)
        {
            return !_notSpecification.IsSatisfiedBy(candidate);
        }
    }
}


