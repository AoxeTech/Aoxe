namespace Zaaby.Extensions.Configuration.Consul;

internal class JsonConfigurationObjectParser
{
	private readonly IDictionary<string, string> _data =
		new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

	private readonly Stack<string> _context = new();
	private string _currentPath = string.Empty;
	private JsonTextReader _reader = null!;

	public IDictionary<string, string> Parse(Stream input)
	{
		_data.Clear();
		_reader = new JsonTextReader(new StreamReader(input))
		{
			DateParseHandling = DateParseHandling.None
		};

		var jsonConfig = JObject.Load(_reader);

		VisitJObject(jsonConfig);

		return _data;
	}

	private void VisitJObject(JObject jObject)
	{
		foreach (var property in jObject.Properties())
		{
			EnterContext(property.Name);
			VisitToken(property.Value);
			ExitContext();
		}
	}

	private void VisitToken(JToken token)
	{
		switch (token.Type)
		{
			case JTokenType.Object:
				VisitJObject(token.Value<JObject>());
				break;
			case JTokenType.Array:
				VisitArray(token.Value<JArray>());
				break;
			case JTokenType.None:
			case JTokenType.Date:
			case JTokenType.Guid:
			case JTokenType.Integer:
			case JTokenType.Float:
			case JTokenType.String:
			case JTokenType.Boolean:
			case JTokenType.Bytes:
			case JTokenType.Raw:
			case JTokenType.Null:
				VisitPrimitive(token);
				break;
			case JTokenType.Constructor: throw new NotSupportedException(nameof(JTokenType.Constructor));
			case JTokenType.Property: throw new NotSupportedException(nameof(JTokenType.Property));
			case JTokenType.Comment: throw new NotSupportedException(nameof(JTokenType.Comment));
			case JTokenType.Undefined: throw new NotSupportedException(nameof(JTokenType.Undefined));
			case JTokenType.Uri: throw new NotSupportedException(nameof(JTokenType.Uri));
			case JTokenType.TimeSpan: throw new NotSupportedException(nameof(JTokenType.TimeSpan));
			default:
				throw new FormatException(
					$"Unsupported JSON token '{_reader.TokenType}' was found. Path '{_reader.Path}', line {_reader.LineNumber} position {_reader.LinePosition}.");
		}
	}

	private void VisitArray(JArray array)
	{
		for (var index = 0; index < array.Count; index++)
		{
			EnterContext(index.ToString());
			VisitToken(array[index]);
			ExitContext();
		}
	}

	private void VisitPrimitive(JToken data)
	{
		var key = _currentPath;
		if (_data.ContainsKey(key)) throw new FormatException($"A duplicate key '{key}' was found.");
		_data[key] = data.ToString();
	}

	private void EnterContext(string context)
	{
		_context.Push(context);
		_currentPath = string.Join(":", _context.Reverse());
	}

	private void ExitContext()
	{
		_context.Pop();
		_currentPath = string.Join(":", _context.Reverse());
	}
}