using ConsoleAppDesignPattern.SpecificationPattern.NotNuget.Domain.Interfaces;

namespace ConsoleAppDesignPattern.SpecificationPattern.NotNuget.Entities
{
    public class Entity
    {
        protected CompositeSpecification<object>? ValidSpecification;

        public bool IsValid()
        {
            return ValidSpecification?.IsSatisfiedBy(this) ?? true;
        }
    }
}
