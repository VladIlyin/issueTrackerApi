using System;
using System.Collections.Generic;

#nullable disable

namespace IssueTracker.Persistance.Models
{
    public class Role
    {
        public Role(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; }
    }
}
