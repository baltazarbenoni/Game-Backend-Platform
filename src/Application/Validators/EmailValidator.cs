using System.ComponentModel.DataAnnotations;

namespace Application.Validators
{
    public class EmailValidator : Validator
    {
        public EmailValidator(string suggestion)
        {
            this.suggestion = suggestion;
            condition = IsValid();
            message = "Invalid email format."; 
        }
        bool IsValid()
        {
            var email = new EmailAddressAttribute();
            return email.IsValid(suggestion);
        }
    }
}