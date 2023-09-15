using ConsoleAppDesignPattern.SpecificationPattern.NotNuget.Domain.Interfaces;
using ConsoleAppDesignPattern.SpecificationPattern.NotNuget.ValueObjects;
using ConsoleAppDesignPattern.SpecificationPattern.NotNuget.ValueObjects.Specification;

namespace ConsoleAppDesignPattern.SpecificationPattern.NotNuget.Entities.Specification
{
    public class PersonValidSpecification<T> : CompositeSpecification<T>
    {
        public override bool IsSatisfiedBy(T candidate)
        {
            var person = candidate as Person;

            var personNameSpecification = new PersonNameValidSpecification<Person>(true);
            if (!personNameSpecification.IsSatisfiedBy(person))
                return false;

            var emailSpecification = new EmailValidSpecification<Email>();
            if (!emailSpecification.IsSatisfiedBy(person?.Email))
                return false;

            return true;
        }
    }
}
