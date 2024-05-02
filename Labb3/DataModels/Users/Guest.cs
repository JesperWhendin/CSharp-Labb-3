using Labb3ProgTemplate.Enums;

namespace Labb3ProgTemplate.DataModels.Users;

public class Guest : User
{
    public override UserTypes Type { get; }

    public Guest(string name, string password) : base(name, password)
    {
        Type = UserTypes.Guest;
    }
}