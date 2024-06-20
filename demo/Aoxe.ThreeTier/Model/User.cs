using System;

namespace Model
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Pwd { get; set; }
        public string Nickname { get; set; }
        public DateTime CreatedUtcTime { get; set; }
    }
}