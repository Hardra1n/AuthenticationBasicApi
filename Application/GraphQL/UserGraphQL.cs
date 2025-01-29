namespace Application.GraphQL
{
    public class UserGraphQL
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateOnly Birthday { get; set; }
    }
}
