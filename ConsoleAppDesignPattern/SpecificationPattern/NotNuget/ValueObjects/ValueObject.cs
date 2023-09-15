using ConsoleAppDesignPattern.SpecificationPattern.NotNuget.Domain.Interfaces;
using ConsoleAppDesignPattern.SpecificationPattern.NotNuget.ValueObjects.Specification;

namespace ConsoleAppDesignPattern.SpecificationPattern.NotNuget.ValueObjects
{
    public abstract class ValueObject
    {
        protected CompositeSpecification<object>? ValidSpecification;

        public bool IsValid()
        {
            return ValidSpecification?.IsSatisfiedBy(this) ?? true;
        }
    }

    public class Email : ValueObject
    {
        public const int AddressMinLength = 3;
        public const int AddressMaxLength = 255;

        public Email(string address)
        {
            Address = address;
            ValidSpecification = new EmailValidSpecification<object>();
        }

        public string Address { get; }

        public override string ToString() => Address;
    }
}
