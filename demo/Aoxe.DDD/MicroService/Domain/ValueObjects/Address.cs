namespace Domain.ValueObjects
{
    public record Address
    {
        public string Country { get; protected set; }
        public string State { get; protected set; }
        public string City { get; protected set; }
        public string Street { get; protected set; }

        public Address(string country, string state, string city, string street) =>
            (Country, State, City, Street) = (country, state, city, street);
    }
}