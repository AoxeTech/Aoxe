using System;

namespace QueryService.Models
{
    public class UserReadModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}