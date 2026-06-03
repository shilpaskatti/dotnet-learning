namespace LearningBasics.Models
{
    public class User
    {
        public int? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        //public List<string>? Subjects { get; set; }
        public string? Subjects { get; set; }

        public string? DateOfBirth { get; set; }
    }
}
