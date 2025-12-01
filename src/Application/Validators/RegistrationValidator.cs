namespace Application.Validators
{
    public class RegistrationValidator
    {
        public void Validate(string email, string password, string displayName)
        {
            new EmailValidator(email).Validate();
            new PasswordValidator(password).Validate();
            new DisplayNameValidator(displayName).Validate();
        }
    }
}