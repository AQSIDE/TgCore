namespace AdvancedBot.Data;

public class UserProfile
{
    public long Id { get; init; }
    public string Username { get; set; }

    public UserProfile(long id, string username)
    {
        Id = id;
        Username = username;
    }
}