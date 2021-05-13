using System;
using System.Collections.Generic;

#nullable disable

namespace IssueTracker.Domain.Models
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
