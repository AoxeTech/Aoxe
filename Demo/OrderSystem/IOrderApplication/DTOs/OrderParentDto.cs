using System;

namespace IOrderApplication.DTOs
{
    public class OrderParentDto
    {
        public string Id { get; set; }
        public DateTimeOffset CreateTimeOffset { get; set; }
    }
}