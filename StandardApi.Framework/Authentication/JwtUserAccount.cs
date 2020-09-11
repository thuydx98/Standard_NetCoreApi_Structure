namespace StandardApi.Framework.Authentication
{
    public class JwtUserAccount
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string UserCode { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
