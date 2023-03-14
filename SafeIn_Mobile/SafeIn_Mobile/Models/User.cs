namespace SafeIn_Mobile.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public bool IsInside { get; set; }
        public bool IsAdmin { get; set; }

    }
}
