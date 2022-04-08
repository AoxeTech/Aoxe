# Zaaby.AspNetCore.Formatters

Formatters for asp.net core

[JilFormatters](https://github.com/PicoHex/Zaaby/tree/master/src/Server/Formatters/Zaaby.AspNetCore.Formatters.Jil)

[MsgPackFormatters](https://github.com/PicoHex/Zaaby/tree/master/src/Server/Formatters/Zaaby.AspNetCore.Formatters.MsgPack)

[ProtobufFormatters](https://github.com/PicoHex/Zaaby/tree/master/src/Server/Formatters/Zaaby.AspNetCore.Formatters.Protobuf)

[Utf8JsonFormatters](https://github.com/PicoHex/Zaaby/tree/master/src/Server/Formatters/Zaaby.AspNetCore.Formatters.Utf8Json)

[ZeroFormatters](https://github.com/PicoHex/Zaaby/tree/master/src/Server/Formatters/Zaaby.AspNetCore.Formatters.ZeroFormatter)

## Benchmark

``` ini
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1466 (21H1/May2021Update)
Intel Core i7-6600U CPU 2.60GHz (Skylake), 1 CPU, 4 logical and 2 physical cores
.NET SDK=6.0.101
  [Host]     : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT
  DefaultJob : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT
```

|       Method |     Mean |   Error |  StdDev |      Min |      Max |   Median |
|------------- |---------:|--------:|--------:|---------:|---------:|---------:|
| JilPost | 148.9 μs | 2.78 μs | 2.73 μs | 143.0 μs | 153.5 μs | 148.4 μs |
| ProtobufPost | 146.0 μs | 2.83 μs | 4.32 μs | 125.7 μs | 152.1 μs | 146.2 μs |
| JsonPost | 2249 μs | 152.8 μs | 445.8 μs | 2076 μs | 1622 μs | 3417 μs |
| MsgPackPost | 201.7 μs | 3.87 μs | 7.90 μs | 198.3 μs | 192.9 μs | 225.3 μs |
| Utf8JsonPost | 155.9 μs | 2.23 μs | 1.98 μs | 153.8 μs | 159.8 μs | 155.5 μs |
| ZeroFormatterPost | 127.8 μs | 2.46 μs | 3.20 μs | 119.2 μs | 132.7 μs | 127.6 μs |
