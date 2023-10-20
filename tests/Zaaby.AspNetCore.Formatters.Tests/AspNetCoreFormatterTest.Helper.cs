namespace Zaaby.AspNetCore.Formatters.Tests;

public partial class FormatterTest
{
    private static bool CompareDtos(List<TestDto>? first, List<TestDto>? second)
    {
        if (first is null || second is null) return false;
        if (first.Count != second.Count) return false;

        for (var i = 0; i < first.Count; i++)
        {
            var dtoOne = first[i];
            var dtoTwo = second[i];

            if (dtoOne.Id != dtoTwo.Id
                || dtoOne.CreateTime.ToUniversalTime() != dtoTwo.CreateTime.ToUniversalTime()
                || dtoOne.Enum != dtoTwo.Enum
                || dtoOne.Name != dtoTwo.Name
                || dtoOne.Tag != dtoTwo.Tag)
//                    dtoOne.TestTime != dtoTwo.TestTime ||
//                    !CompareDtos(dtoOne.Kids, dtoTwo.Kids))
                return false;
        }

        return true;
    }

    private static List<TestDto> GetDtos()
    {
        return new List<TestDto>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Tag = long.MaxValue,
                CreateTime = DateTime.Now,
//                    TestTime = DateTimeOffset.Now,
                Name = "0",
                Enum = TestEnum.Apple,
//                    Kids = new List<TestDto>
//                    {
//                        new TestDto
//                        {
//                            Id = Guid.NewGuid(),
//                            Tag = long.MaxValue - 1,
//                            CreateTime = DateTime.Now,
////                            TestTime = DateTimeOffset.Now,
//                            Name = "00",
//                            Enum = TestEnum.Banana
//                        },
//                        new TestDto
//                        {
//                            Id = Guid.NewGuid(),
//                            Tag = long.MaxValue - 2,
//                            CreateTime = DateTime.Now,
////                            TestTime = DateTimeOffset.Now,
//                            Name = "01",
//                            Enum = TestEnum.Pear
//                        }
//                    }
            },
            new()
            {
                Id = Guid.NewGuid(),
                Tag = long.MaxValue - 3,
                CreateTime = DateTime.Now,
//                    TestTime = DateTimeOffset.Now,
                Name = "1",
                Enum = TestEnum.Apple,
//                    Kids = new List<TestDto>
//                    {
//                        new TestDto
//                        {
//                            Id = Guid.NewGuid(),
//                            Tag = long.MaxValue - 4,
//                            CreateTime = DateTime.Now,
////                            TestTime = DateTimeOffset.Now,
//                            Name = "10",
//                            Enum = TestEnum.Banana
//                        },
//                        new TestDto
//                        {
//                            Id = Guid.NewGuid(),
//                            Tag = long.MaxValue - 5,
//                            CreateTime = DateTime.Now,
////                            TestTime = DateTimeOffset.Now,
//                            Name = "11",
//                            Enum = TestEnum.Pear
//                        }
//                    }
            }
        };
    }
}