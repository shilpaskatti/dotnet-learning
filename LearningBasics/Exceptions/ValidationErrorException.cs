namespace LearningBasics.Exceptions
{
    public sealed class ValidationErrorException :AppException
    {
        public IDictionary<string, string[]> errors { get; }
        public ValidationErrorException(IDictionary<string, string[]> errors):
            base("One or more validation errors occurred.", System.Net.HttpStatusCode.BadRequest)
        {
            this.errors = errors;
        }

        public ValidationErrorException(string field, string error):
            base("One or more validation errors occurred.", System.Net.HttpStatusCode.BadRequest)
        {
            this.errors = new Dictionary<string, string[]>
            {
                { field, new string[] { error } }
            };
        }
    }
}
