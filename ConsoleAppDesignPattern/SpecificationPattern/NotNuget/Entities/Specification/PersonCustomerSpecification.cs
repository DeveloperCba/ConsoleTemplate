using ConsoleAppDesignPattern.SpecificationPattern.NotNuget.Domain.Interfaces;
using ConsoleAppDesignPattern.SpecificationPattern.NotNuget.Entities;

namespace ConsoleAppDesignPattern.SpecificationPattern.NotNuget.Entities.Specification
{
    public class PersonCustomerSpecification<T> : CompositeSpecification<T>
    {
        private readonly int _categoryId;

        public PersonCustomerSpecification()
        {
            _categoryId = 1;
        }

        public override bool IsSatisfiedBy(T candidate)
        {
            var person = candidate as Person;
            return person != null && person.Category?.CategoryId == _categoryId;
        }
    }
}
