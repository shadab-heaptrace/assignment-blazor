using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Models.CustomValidators
{
    public class EmailDomainValidator : ValidationAttribute
    {
        public string AllowedDomain { get; set; }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string[] str = value.ToString().Split('@');
                if (str.Length > 1 && str[1].ToLower() == AllowedDomain.ToLower())
                {
                    return null;
                }

                return new ValidationResult($"Domain must be {AllowedDomain}",
                    new[] { validationContext.MemberName });
            }
            return null;
        }
    }
}
