namespace _6toDAES13.Models
{
    public class Student
    {
        public int StudentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int GradeID { get; set; }
        public bool Active { get; set; }

        public Grade? Grade { get; set; }

    }
}
