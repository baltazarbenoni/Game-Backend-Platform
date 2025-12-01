namespace Application.Validators
{
    public class DisplayNameValidator : Validator
    {
        public DisplayNameValidator(string suggestion)
        {
            this.suggestion = suggestion;
            condition = IsValid();
            message = $"Invalid display name. Name {exceptionSpecification}."; 
        }
        bool IsValid()
        {
            if(suggestion.Length < 6)
            {
                exceptionSpecification = "must be at least 6 characters long";
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}