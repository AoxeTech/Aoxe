using System;
using System.Collections.Generic;
using System.Text;

namespace IFinanceApplication.DTOs
{
    public class CustomerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreateTime { get; set; }
    }
}