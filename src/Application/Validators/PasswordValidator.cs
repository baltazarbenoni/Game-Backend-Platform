namespace Application.Validators
{
    public class PasswordValidator : Validator
    {
        public PasswordValidator(string suggestion)
        {
            this.suggestion = suggestion;
            condition = IsValid();
            message = $"Invalid input, password {exceptionSpecification}.";
        }
        bool IsValid()
        {
            if(suggestion.Length < 8)
            {
                exceptionSpecification = "must contain at least 8 characters";
                return false;
            }
            else if(!HasSpecialCharsOrDigit())
            {
                exceptionSpecification = "must contain at least one special character (not a letter or a digit) and a number";
                return false;
            }
            else if(!HasUpperAndLowerCase())
            {
                exceptionSpecification = "must contain both upper and lower case letters";
                return false;
            }
            else
            {
                return true;
            }
        }
        bool HasSpecialCharsOrDigit()
        {
            bool hasSpecial = suggestion.Any(c => !char.IsLetterOrDigit(c));
            bool hasDigit = suggestion.Any(char.IsDigit);
            return hasDigit && hasSpecial;
        }

        bool HasUpperAndLowerCase()
        {
            bool hasUpper = suggestion.Any(char.IsUpper);
            bool hasLower = suggestion.Any(char.IsLower);
            return hasUpper && hasLower; 
        }
    }
}