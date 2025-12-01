namespace Application.Validators
{
    public class LoginValidator
    {
        public void Validate(string email, string password)
        {
            new EmailValidator(email).Validate();
            new PasswordValidator(password).Validate();
        }
    }
}