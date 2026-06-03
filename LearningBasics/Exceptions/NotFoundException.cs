namespace LearningBasics.Exceptions
{
    public sealed class NotFoundException : AppException
    {
        public NotFoundException(string resourceName, string key)
            :base($"Resource {resourceName} with key {key} was not found", System.Net.HttpStatusCode.NotFound) 
        {
            
        }
    }
}
