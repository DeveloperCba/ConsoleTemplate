using ConsoleAppDesignPattern.SpecificationPattern.NotNuget.Domain.Implementations;

namespace ConsoleAppDesignPattern.SpecificationPattern.NotNuget.Domain.Interfaces
{
    public abstract class CompositeSpecification<T> : ISpecification<T>
    {
        public abstract bool IsSatisfiedBy(T candidate);
        public ISpecification<T> And(ISpecification<T> specification)
        {
            return new AndSpecification<T>(this, specification);
        }

        public ISpecification<T> AndNot(ISpecification<T> specification)
        {
            return new AndNotSpecification<T>(this, specification);
        }

        public ISpecification<T> Or(ISpecification<T> specification)
        {
            return new OrSpecification<T>(this, specification);
        }

        public ISpecification<T> OrNot(ISpecification<T> specification)
        {
            return new OrNotSpecification<T>(this, specification);
        }

        public ISpecification<T> Not(ISpecification<T> specification)
        {
            return new NotSpecification<T>(specification);
        }
    }
}


