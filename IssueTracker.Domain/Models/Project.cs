using System;
using System.Collections.Generic;

#nullable disable

namespace IssueTracker.Domain.Models
{
    public class Project
    {
        public Project(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; }
        public string Name { get; }
    }
}
