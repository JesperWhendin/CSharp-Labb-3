using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using Labb3ProgTemplate.DataModels.Users;
using Labb3ProgTemplate.Enums;

namespace Labb3ProgTemplate.Managerrs;

public static class UserManager
{
    private static readonly IEnumerable<User>? _users = new List<User>();
    private static User _currentUser;

    public static IEnumerable<User>? Users => _users;

    public static User CurrentUser
    {
        get => _currentUser;
        set
        {
            _currentUser = value;
            CurrentUserChanged?.Invoke();
        }
    }

    public static event Action CurrentUserChanged;

    // Skicka detta efter att användarlistan ändrats eller lästs in
    public static event Action UserListChanged;

    public static bool IsAdminLoggedIn => CurrentUser.Type is UserTypes.Admin;
    public static bool IsCustomerLoggedIn => CurrentUser.Type is UserTypes.Customer;
    public static bool IsGuest => CurrentUser.Type is UserTypes.Guest;

    public static string UserFilePath = string.Empty;
    
    public static void FindUserFilePath()
    {
        var dirPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Labb 3 JW");
        Directory.CreateDirectory(dirPath);
        UserFilePath = Path.Combine(dirPath, "Users.json");
    }

    public static void StartUp()
    {
        Guest guest = new("", "");
        CurrentUser = guest;
    }

    public static void ChangeCurrentUser(string name, string password, UserTypes type)
    {
    }

    public static void LogIn(string name, string password)
    {
        foreach (var user in _users)
        {
            if (name == user.Name)
            {
                if (user.Authenticate(password))
                {
                    CurrentUser = user;
                    if (user.Name == "Oregorger")
                    {
                        MessageBox.Show($"Välkommen {user.Name}! Du är inloggad som the bane of Eichmann.", "Välkommen");
                        CurrentUserChanged.Invoke();
                        return;
                    }
                    MessageBox.Show($"Välkommen {user.Name}! Du är inloggad som {user.Type.ToString().ToLower()}.", "Välkommen");
                    CurrentUserChanged.Invoke();
                    return;
                }
                MessageBox.Show("Du har skrivit in fel lösenord.", "Felmeddelande");
                return;
            }
        }
        MessageBox.Show("Du har inte skrivit in ett giltigt användarnamn.", "Felmeddelande");
    }

    public static void LogOut()
    {
        CurrentUser.Cart.Clear();
        Guest guest = new ("", "");
        CurrentUser = guest;
        CurrentUserChanged.Invoke();
    }

    public static async Task SaveUsersToFile(string name, string password, UserTypes type)
    {
        if (_users.Any(u => u.Name == name))
        {
            MessageBox.Show("Det finns redan en användare med detta namn.", "Felmeddelande");
            return;
        }
        if (type == UserTypes.Admin)
        {
            var newAdmin = new Admin(name, password);
            ((List<User>)_users).Add(newAdmin);
        }
        if (type == UserTypes.Customer)
        {
            var newCustomer = new Customer(name, password);
            ((List<User>)_users).Add(newCustomer);
        }
        using var streamWriter = new StreamWriter(UserFilePath);
        var json = JsonSerializer.Serialize(_users, new JsonSerializerOptions() { WriteIndented = true });
        streamWriter.WriteLine(json);
        UserListChanged?.Invoke();
    }

    public static async Task LoadUsersFromFile()
    {
        if (!File.Exists(UserFilePath))
        {
            ((List<User>)_users).AddRange(new User[]
            {
                new Admin ( "Admin",  "123" ),
                new Admin ( "Jesper", "qwe" ),
                new Customer ( "Olle",      "asd" ),
                new Customer ( "Oregorger", "kek" )
            });
            using var streamWriter = new StreamWriter(UserFilePath);
            var json = JsonSerializer.Serialize(_users, new JsonSerializerOptions() { WriteIndented = true });
            streamWriter.WriteLine(json);
            UserListChanged?.Invoke();
        }

        var jsonText = string.Empty;
        string? jsonLine = string.Empty;
        using StreamReader streamReader = new (UserFilePath);
        while ((jsonLine = streamReader.ReadLine()) != null)
        {
            jsonText += jsonLine;
        }
        var jsonDesUserList = new List<User>();
        using (var jsonDocument = JsonDocument.Parse(jsonText))
        {
            if (jsonDocument.RootElement.ValueKind == JsonValueKind.Array)
            {
                foreach (var jsonElement in jsonDocument.RootElement.EnumerateArray())
                {
                    User u;
                    switch (jsonElement.GetProperty("Type").GetInt32())
                    {
                        case 0:
                            u = jsonElement.Deserialize<Admin>();
                            jsonDesUserList.Add(u);
                            break;
                        case 1:
                            u = jsonElement.Deserialize<Customer>();
                            jsonDesUserList.Add(u);
                            break;
                    }
                }
            }
        }
        ((List<User>)_users).Clear();
        foreach (var u in jsonDesUserList)
        {
            ((List<User>)_users).Add(u);
        }
        UserListChanged?.Invoke();
    }
}