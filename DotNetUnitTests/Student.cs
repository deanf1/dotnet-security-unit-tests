namespace DotNetUnitTests
{
    public class Student
    {
        public Student()
        {

        }

        public Student(string lastName, string firstName, string username, string password)
        {
            LastName = lastName;
            FirstName = firstName;
            Username = username;
            Password = password;
        }

        public virtual int ID { get; set; }
        public virtual string LastName { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
    }
}