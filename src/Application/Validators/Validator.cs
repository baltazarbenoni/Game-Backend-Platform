namespace Application.Validators
{
    public class Validator
    {
        protected string suggestion = "";
        protected bool condition;
        protected string message = "";
        protected string exceptionSpecification  = "";
        public void Validate()
        {
            if(!condition)
            {
                throw new Exception(message);
            }
        }
    }
}