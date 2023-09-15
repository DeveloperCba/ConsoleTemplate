using ConsoleAppDesignPattern.SpecificationPattern.NotNuget.Domain.Interfaces;
using ConsoleAppDesignPattern.SpecificationPattern.NotNuget.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleAppDesignPattern.SpecificationPattern.NotNuget.ValueObjects.Specification
{
    public class EmailValidSpecification<T> : CompositeSpecification<T>
    {
        private readonly bool _required;

        public EmailValidSpecification(bool required = false)
        {
            _required = required;
        }

        public override bool IsSatisfiedBy(T candidate)
        {
            var email = candidate as Email;

            if (string.IsNullOrEmpty(email?.Address) && !_required)
                return true;

            if ((email?.Address ?? "").Length < Email.AddressMinLength)
                return false;

            if ((email?.Address ?? "").Length > Email.AddressMaxLength)
                return false;

            const string pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            return Regex.IsMatch(email?.Address ?? "", pattern);

        }
    }
}
