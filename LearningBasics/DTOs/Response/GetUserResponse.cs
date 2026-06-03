namespace LearningBasics.DTOs.Response
{
    public class GetUserResponse
    {
        public int? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Subjects { get; set; }
        public string? DateOfBirth { get; set; }
    }
}
