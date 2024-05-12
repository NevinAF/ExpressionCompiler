namespace ExpressionCompilerTests;

using NAF.ExpressionCompiler;

public class ExampleClass
{
	public int IntField;
	public float FloatProperty { get; set; }
	public int AddMethod(int a) => a + IntField;
	public int this[int index] => index + 5;
	public int this[int index, int index2]
	{
		get => index + index2;
		set => IntField = this[index] + index2;
	}

	public string[] Strings = new string[] { "a", "b", "c" };

	public static string? StaticStringField = "This is a string that is not null because it has a value.";
	public static string? StaticStringProperty { get; set; } = "This is a string that is not null because it has a value.";
	public static int StaticIntField;
	public const double StaticDoubleConstant = 5;
	public static double StaticDivideMethod(double a, double b) => a / b;

	[Test]
	public void RandomishExpressions()
	{
		var expressions = new (string, Type?, object?)[] {
			("1 + 2 * 3", null, null),
			("'hello' == 'goodbye' && 'world' == 'world'", null, null),
			("43.21f >= 43.22", null, null),
			("+Math.PI / Math.E", null, null),
			("string.Empty + \"Hello\" + \"World\" + \"!\"", null, null),
			("DateTime.Now - DateTime.UtcNow > TimeSpan.Zero", null, null),
			("~(byte.MaxValue - 2035)", null, null),
			("Substring(0, 5)", typeof(string), "String Tester"),
			("Substring(2, 8)", typeof(string), "More examples of strings."),
			("Replace('e', 'a')", typeof(string), "Hello World!"),
			("Replace(\"Hello\", \"Goodbye\")", typeof(string), "Hello Nevin"),
			("this + 4", typeof(float), 25f),
			("this * Math.PI", typeof(double), 23.67),
			("'The special number is ' + this", typeof(int), 34),
			("int.Parse(this.Replace('x', '4'))", typeof(string), "3457xx5x"),
			("this.Value.AddDays(1)", typeof(DateTime?), new DateTime?(DateTime.Today)),
			("this.HasValue ? this.Value : DateTime.MinValue", typeof(DateTime?), new DateTime?(DateTime.Now)),
			("0x45 >> 2", null, null),
			("0x45 | 0x23", null, null),
			("this - 63", typeof(decimal), 53m),
			("this - 63", typeof(uint), 35u),
			("this ?? 5", typeof(int?), 23),
			("this ?? 5", typeof(int?), null),
			("this?.Substring(0, 5)", typeof(string), "String Tester"),
			("IntField", typeof(ExampleClass), new ExampleClass { IntField = 5 }),
			("FloatProperty", typeof(ExampleClass), new ExampleClass { FloatProperty = 5.5f }),
			("AddMethod(5)", typeof(ExampleClass), new ExampleClass { IntField = 5 }),
			("this[5]", typeof(ExampleClass), new ExampleClass()),
			("this[5, 6]", typeof(ExampleClass), new ExampleClass()),
			("Strings[1]", typeof(ExampleClass), new ExampleClass()),
			("ExampleClass.StaticStringField", null, null),
			("ExampleClass.StaticStringProperty", null, null),
			("ExampleClass.StaticIntField", null, null),
			("ExampleClass.StaticDoubleConstant", null, null),
			("ExampleClass.StaticDivideMethod(5, 2)", null, null),
			("this[2] + this[3] * this[4]", typeof(ExampleClass), new ExampleClass()),
			("this.IntField + this.FloatProperty * 2", typeof(ExampleClass), new ExampleClass { IntField = 5, FloatProperty = 3.5f }),
			("this.AddMethod(5) * 2 - this.IntField", typeof(ExampleClass), new ExampleClass { IntField = 5 }),
			("Math.Pow(this.IntField, 2) + Math.Sqrt(this.FloatProperty)", typeof(ExampleClass), new ExampleClass { IntField = 5, FloatProperty = 3.5f }),
			("ExampleClass.StaticDivideMethod(5, 2) * ExampleClass.StaticIntField - ExampleClass.StaticDoubleConstant", null, null),
			("this[5, 6] + this.Strings[1].Length", typeof(ExampleClass), new ExampleClass()),
			("this.HasValue ? this.Value.AddDays(1) : DateTime.MinValue", typeof(DateTime?), new DateTime?(DateTime.Now)),
			("this.Replace(\"Hello\", \"Goodbye\") + \" \" + this.Substring(0, 5)", typeof(string), "Hello Nevin"),
			("int.Parse(this.Replace('x', '4')) + this.Length", typeof(string), "3457xx5x"),
			("this.Value.AddDays(1) - this.Value", typeof(DateTime?), DateTime.Today),
			("this ?? 5 + 2 * 3", typeof(int?), null),
			("this?.Substring(0, 5) + \" \" + this?.Substring(5, 5)", typeof(string), "String Tester"),
			("StaticStringField + \" \" + StaticStringProperty", typeof(ExampleClass), new ExampleClass()),
			("StaticIntField * StaticDoubleConstant / 2", typeof(ExampleClass), new ExampleClass()),
			("ExampleClass.StaticDivideMethod(5, 2) +ExampleClass.StaticIntField * 2", null, null),
			("DateTime.Now.DayOfWeek + \" \" + DateTime.Now.Day", null, null),
			("TimeSpan.FromHours(1).TotalMinutes", null, null),
			("Guid.NewGuid().ToString().Length", null, null),
			("Math.Max(this.IntField, 10)", typeof(ExampleClass), new ExampleClass { IntField = 5 }),
			("Math.Min(this.FloatProperty, 3.5f)", typeof(ExampleClass), new ExampleClass { FloatProperty = 4.5f }),
			("this.ToString().Length", typeof(int), 12345),
			("this.ToString().Substring(0, 2)", typeof(DateTime), DateTime.Now),
			("this.Split(',').Length", typeof(string), "one,two,three,four,five"),
			("this.ToUpper().Replace('A', 'B')", typeof(string), "banana"),
			("this.ToLower().Contains('apple')", typeof(string), "Pineapple"),
			("this.HasValue ? this.Value : new DateTime(2000, 1, 1)", typeof(DateTime?), new DateTime?(DateTime.Now)),
			("this ?? new TimeSpan(1, 0, 0)", typeof(TimeSpan?), TimeSpan.Zero),
			("this?.Length ?? 0", typeof(string), "Hello"),
			("this?.Length ?? 0", typeof(string), "Another string! WOOOO.")
		};

		var types = new Type[] { typeof(DateTime), typeof(TimeSpan), typeof(ExampleClass), typeof(Math), typeof(Guid) };

		var parser = new Compiler(types);

		for (int i = 0; i < expressions.Length; i++)
		{
			var (expression, type, param) = expressions[i];

			if (type == null)
				parser.CompileAnonymous(expression)();
			else parser.CompileAnonymous(expression, type)(param != null ? param : (type.IsValueType ? Activator.CreateInstance(type) : null)!);
		}
	}
}