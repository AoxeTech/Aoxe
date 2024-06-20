namespace Zaaby.AspNetCore.Formatters.Demo;

[ProtoContract]
[ZeroFormattable]
public class TestDto
{
    [ProtoMember(1)] [Index(0)] public virtual Guid Id { get; set; }
    [ProtoMember(2)] [Index(1)] public virtual string? Name { get; set; }
    [ProtoMember(3)] [Index(2)] public virtual DateTime CreateTime { get; set; }
    [ProtoMember(4)] [Index(3)] public virtual long Tag { get; set; }
    [ProtoMember(5)] [Index(4)] public virtual TestEnum Enum { get; set; }
//        [ProtoMember(6)] [Index(5)] public virtual List<TestDto> Kids { get; set; } = new List<TestDto>();
//        [ProtoMember(7)] public DateTimeOffset TestTime { get; set; }
}