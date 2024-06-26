﻿using System.Collections.Generic;
using Labb3ProgTemplate.DataModels.Products;
using Labb3ProgTemplate.Enums;

namespace Labb3ProgTemplate.DataModels.Users;

public abstract class User
{
    public string Name { get; }

    public string Password { get; }

    public abstract UserTypes Type { get; }

    public List<Product> Cart = new ();

    protected User(string name, string password)
    {
        Name = name;
        Password = password;
    }

    public bool Authenticate(string password)
    {
        return Password.Equals(password);
    }
}