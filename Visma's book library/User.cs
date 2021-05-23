namespace Vismas_book_library
{
    /// <summary>
    /// User class, with only characteristic being their name
    /// </summary>
    public class User
    {
        public string Name { get; set; }

        public User(string name)
        {
            Name = name;
        }
    }
}