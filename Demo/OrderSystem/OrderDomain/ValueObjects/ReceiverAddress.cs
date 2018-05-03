namespace OrderDomain.ValueObjects
{
    public struct ReceiverAddress
    {
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string SubAddress { get; set; }
    }
}