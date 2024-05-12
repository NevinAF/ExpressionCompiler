namespace ExpressionCompilerTests;

using NAF.ExpressionCompiler;

public class ParserTests
{
	[Test]
	public void ParserNumbers()
	{
		Delegate a;
		Compiler compiler = new Compiler();

		a = compiler.Compile("123");
		// Assert the the delegate is Func<int> and the result of the function is 123
		Assert.IsInstanceOf<Func<int>>(a);
		Assert.That(((Func<int>)a)(), Is.EqualTo(123));

		a = compiler.Compile("123.456");
		Assert.IsInstanceOf<Func<double>>(a);
		Assert.That(((Func<double>)a)(), Is.EqualTo(123.456));

		a = compiler.Compile("123.456f");
		Assert.IsInstanceOf<Func<float>>(a);
		Assert.That(((Func<float>)a)(), Is.EqualTo(123.456f));

		a = compiler.Compile("123.456m");
		Assert.IsInstanceOf<Func<decimal>>(a);
		Assert.That(((Func<decimal>)a)(), Is.EqualTo(123.456m));

		a = compiler.Compile("123.4e5f");
		Assert.IsInstanceOf<Func<float>>(a);
		Assert.That(((Func<float>)a)(), Is.EqualTo(123.4e5f));

		a = compiler.Compile("12234d");
		Assert.IsInstanceOf<Func<double>>(a);
		Assert.That(((Func<double>)a)(), Is.EqualTo(12234d));

		a = compiler.Compile("0xef12");
		Assert.IsInstanceOf<Func<int>>(a);
		Assert.That(((Func<int>)a)(), Is.EqualTo(0xef12));

		a = compiler.Compile("123u");
		Assert.IsInstanceOf<Func<uint>>(a);
		Assert.That(((Func<uint>)a)(), Is.EqualTo(123u));

		a = compiler.Compile("645ul");
		Assert.IsInstanceOf<Func<ulong>>(a);
		Assert.That(((Func<ulong>)a)(), Is.EqualTo(645ul));

		a = compiler.Compile("0x1234L");
		Assert.IsInstanceOf<Func<long>>(a);
		Assert.That(((Func<long>)a)(), Is.EqualTo(0x1234L));

		a = compiler.Compile("0xaL");
		Assert.IsInstanceOf<Func<long>>(a);
		Assert.That(((Func<long>)a)(), Is.EqualTo(0xaL));

		a = compiler.Compile("0xb");
		Assert.IsInstanceOf<Func<int>>(a);
		Assert.That(((Func<int>)a)(), Is.EqualTo(0xb));

		a = compiler.Compile("0xcu");
		Assert.IsInstanceOf<Func<uint>>(a);
		Assert.That(((Func<uint>)a)(), Is.EqualTo(0xcu));

		a = compiler.Compile("0xdUL");
		Assert.IsInstanceOf<Func<ulong>>(a);
		Assert.That(((Func<ulong>)a)(), Is.EqualTo(0xdUL));
	}

	[Test]
	public void SimpleMath()
	{
		Delegate a;
		Compiler compiler = new Compiler();

		a = compiler.Compile("1 + 2");
		Assert.IsInstanceOf<Func<int>>(a);
		Assert.That(((Func<int>)a)(), Is.EqualTo(1 + 2));

		a = compiler.Compile("1 - 2");
		Assert.IsInstanceOf<Func<int>>(a);
		Assert.That(((Func<int>)a)(), Is.EqualTo(1 - 2));

		a = compiler.Compile("1 * 2");
		Assert.IsInstanceOf<Func<int>>(a);
		Assert.That(((Func<int>)a)(), Is.EqualTo(1 * 2));

		a = compiler.Compile("1 / 2");
		Assert.IsInstanceOf<Func<int>>(a);
		Assert.That(((Func<int>)a)(), Is.EqualTo(1 / 2));

		a = compiler.Compile("1 % 2");
		Assert.IsInstanceOf<Func<int>>(a);
		Assert.That(((Func<int>)a)(), Is.EqualTo(1 % 2));

		a = compiler.Compile("1 + 2 * 3");
		Assert.IsInstanceOf<Func<int>>(a);
		Assert.That(((Func<int>)a)(), Is.EqualTo(1 + 2 * 3));

		a = compiler.Compile("(1 + 2) * 3");
		Assert.IsInstanceOf<Func<int>>(a);
		Assert.That(((Func<int>)a)(), Is.EqualTo((1 + 2) * 3));

		a = compiler.Compile("1 + 2 * 3 + 4");
		Assert.IsInstanceOf<Func<int>>(a);
		Assert.That(((Func<int>)a)(), Is.EqualTo(1 + 2 * 3 + 4));

		a = compiler.Compile("1 + 2 * (3 + 4)");
		Assert.IsInstanceOf<Func<int>>(a);
		Assert.That(((Func<int>)a)(), Is.EqualTo(1 + 2 * (3 + 4)));

		a = compiler.Compile("1 + 2 * (3 + 4) / 5");
		Assert.IsInstanceOf<Func<int>>(a);
		Assert.That(((Func<int>)a)(), Is.EqualTo(1 + 2 * (3 + 4) / 5));

		a = compiler.Compile("1 + 2 * (3 + 4) / 5 - 6");
		Assert.IsInstanceOf<Func<int>>(a);
		Assert.That(((Func<int>)a)(), Is.EqualTo(1 + 2 * (3 + 4) / 5 - 6));

		a = compiler.Compile("1 + 2 * (3 + 4) / 5 - 6 * 7");
		Assert.IsInstanceOf<Func<int>>(a);
		Assert.That(((Func<int>)a)(), Is.EqualTo(1 + 2 * (3 + 4) / 5 - 6 * 7));
	}

	[Test]
	public void StringOperations()
	{
		Delegate a;
		Compiler compiler = new Compiler();

		a = compiler.Compile("\"Hello\" + \"World\" + \"!\"");
		Assert.IsInstanceOf<Func<string>>(a);
		Assert.That(((Func<string>)a)(), Is.EqualTo("Hello" + "World" + "!"));

		a = compiler.Compile("\"Hello\" + \"World\" + \"!\" + \" \" + \"How\" + \" \" + \"Are\" + \" \" + \"You\" + \"?\" + \" \" + 1 + 2 + 3");
		Assert.IsInstanceOf<Func<string>>(a);
		Assert.That(((Func<string>)a)(), Is.EqualTo("Hello" + "World" + "!" + " " + "How" + " " + "Are" + " " + "You" + "?" + " " + 1 + 2 + 3));

		a = compiler.Compile("1 + 2 + 'Hello'");
		Assert.IsInstanceOf<Func<string>>(a);
		Assert.That(((Func<string>)a)(), Is.EqualTo(1 + 2 + "Hello"));

		// Escape sequences
		a = compiler.Compile("\"\\\"\"");
		Assert.IsInstanceOf<Func<string>>(a);
		Assert.That(((Func<string>)a)(), Is.EqualTo("\""));

		a = compiler.Compile("\"\\\\\"");
		Assert.IsInstanceOf<Func<string>>(a);
		Assert.That(((Func<string>)a)(), Is.EqualTo("\\"));

		a = compiler.Compile("'\\\\'");
		Assert.IsInstanceOf<Func<char>>(a);
		Assert.That(((Func<char>)a)(), Is.EqualTo('\\'));

		a = compiler.Compile("'\\''");
		Assert.IsInstanceOf<Func<char>>(a);
		Assert.That(((Func<char>)a)(), Is.EqualTo('\''));

		a = compiler.Compile("'\n'");
		Assert.IsInstanceOf<Func<char>>(a);
		Assert.That(((Func<char>)a)(), Is.EqualTo('\n'));

		a = compiler.Compile("'This is a string and it\\'s awesome to have \" characters.'");
		Assert.IsInstanceOf<Func<string>>(a);
		Assert.That(((Func<string>)a)(), Is.EqualTo("This is a string and it's awesome to have \" characters."));

		a = compiler.Compile("\"'More shenanigans' is a string inside a string! What about \\\\ this?? One backslash lol\"");
		Assert.IsInstanceOf<Func<string>>(a);
		Assert.That(((Func<string>)a)(), Is.EqualTo("'More shenanigans' is a string inside a string! What about \\ this?? One backslash lol"));
	}

	[Test]
	public void BooleanOperations()
	{
		Delegate a;
		Compiler compiler = new Compiler();

		a = compiler.Compile("true");
		Assert.IsInstanceOf<Func<bool>>(a);
		Assert.That(((Func<bool>)a)(), Is.EqualTo(true));

		a = compiler.Compile("false");
		Assert.IsInstanceOf<Func<bool>>(a);
		Assert.That(((Func<bool>)a)(), Is.EqualTo(false));

		a = compiler.Compile("true && true");
		Assert.IsInstanceOf<Func<bool>>(a);
		Assert.That(((Func<bool>)a)(), Is.EqualTo(true && true));

		a = compiler.Compile("true && false");
		Assert.IsInstanceOf<Func<bool>>(a);
		Assert.That(((Func<bool>)a)(), Is.EqualTo(true && false));

		a = compiler.Compile("false && true");
		Assert.IsInstanceOf<Func<bool>>(a);
		Assert.That(((Func<bool>)a)(), Is.EqualTo(false && true));

		a = compiler.Compile("false && false");
		Assert.IsInstanceOf<Func<bool>>(a);
		Assert.That(((Func<bool>)a)(), Is.EqualTo(false && false));

		a = compiler.Compile("true || true");
		Assert.IsInstanceOf<Func<bool>>(a);
		Assert.That(((Func<bool>)a)(), Is.EqualTo(true || true));

		a = compiler.Compile("true || false");
		Assert.IsInstanceOf<Func<bool>>(a);
		Assert.That(((Func<bool>)a)(), Is.EqualTo(true || false));

		a = compiler.Compile("false || true");
		Assert.IsInstanceOf<Func<bool>>(a);
		Assert.That(((Func<bool>)a)(), Is.EqualTo(false || true));

		a = compiler.Compile("false || false");
		Assert.IsInstanceOf<Func<bool>>(a);
		Assert.That(((Func<bool>)a)(), Is.EqualTo(false || false));

		a = compiler.Compile("!true");
		Assert.IsInstanceOf<Func<bool>>(a);
		Assert.That(((Func<bool>)a)(), Is.EqualTo(!true));

		a = compiler.Compile("!false");
		Assert.IsInstanceOf<Func<bool>>(a);
		Assert.That(((Func<bool>)a)(), Is.EqualTo(!false));

		a = compiler.Compile("!(true && true)");
		Assert.IsInstanceOf<Func<bool>>(a);
		Assert.That(((Func<bool>)a)(), Is.EqualTo(!(true && true)));

		a = compiler.Compile("!(true && false)");
		Assert.IsInstanceOf<Func<bool>>(a);
		Assert.That(((Func<bool>)a)(), Is.EqualTo(!(true && false)));

		a = compiler.Compile("!(false && true)");
		Assert.IsInstanceOf<Func<bool>>(a);
		Assert.That(((Func<bool>)a)(), Is.EqualTo(!(false && true)));
	}

	[Test]
	public void ComparisonOperations()
	{
		Delegate a;
		Compiler compiler = new Compiler();

		a = compiler.Compile("'hello' == 'hello'");
		Assert.IsInstanceOf<Func<bool>>(a);
		Assert.That(((Func<bool>)a)(), Is.EqualTo("hello" == "hello"));

		a = compiler.Compile("'hello' != 'world'");
		Assert.IsInstanceOf<Func<bool>>(a);
		Assert.That(((Func<bool>)a)(), Is.EqualTo("hello" != "world"));

		a = compiler.Compile("1.23 < 4.56");
		Assert.IsInstanceOf<Func<bool>>(a);
		Assert.That(((Func<bool>)a)(), Is.EqualTo(1.23 < 4.56));

		a = compiler.Compile("1.23 <= 1.23");
		Assert.IsInstanceOf<Func<bool>>(a);
		Assert.That(((Func<bool>)a)(), Is.EqualTo(1.23 <= 1.23));

		a = compiler.Compile("4.56 > 1.23");
		Assert.IsInstanceOf<Func<bool>>(a);
		Assert.That(((Func<bool>)a)(), Is.EqualTo(4.56 > 1.23));

		a = compiler.Compile("4.56 >= 4.56");
		Assert.IsInstanceOf<Func<bool>>(a);
		Assert.That(((Func<bool>)a)(), Is.EqualTo(4.56 >= 4.56));

		a = compiler.Compile("'hello' == 'hello' && 'world' == 'world'");
		Assert.IsInstanceOf<Func<bool>>(a);
		Assert.That(((Func<bool>)a)(), Is.EqualTo("hello" == "hello" && "world" == "world"));

		a = compiler.Compile("'hello' == 'hello' && 'world' == 'universe'");
		Assert.IsInstanceOf<Func<bool>>(a);
		Assert.That(((Func<bool>)a)(), Is.EqualTo("hello" == "hello" && "world" == "universe"));

		a = compiler.Compile("'hello' == 'goodbye' && 'world' == 'world'");
		Assert.IsInstanceOf<Func<bool>>(a);
		Assert.That(((Func<bool>)a)(), Is.EqualTo("hello" == "goodbye" && "world" == "world"));

		a = compiler.Compile("'hello' == 'goodbye' && 'world' == 'universe'");
		Assert.IsInstanceOf<Func<bool>>(a);
		Assert.That(((Func<bool>)a)(), Is.EqualTo("hello" == "goodbye" && "world" == "universe"));

		a = compiler.Compile("'hello' == 'hello' || 'world' == 'world'");
		Assert.IsInstanceOf<Func<bool>>(a);
		Assert.That(((Func<bool>)a)(), Is.EqualTo("hello" == "hello" || "world" == "world"));

		// Type mismatch (without errors)
		a = compiler.Compile("1 == 1.0");
		Assert.IsInstanceOf<Func<bool>>(a);
		Assert.That(((Func<bool>)a)(), Is.EqualTo(1 == 1.0));

		a = compiler.Compile("1f == 1.1");
		Assert.IsInstanceOf<Func<bool>>(a);
		Assert.That(((Func<bool>)a)(), Is.EqualTo(1f == 1.1));

		a = compiler.Compile("43.21f >= 43.22");
		Assert.IsInstanceOf<Func<bool>>(a);
		Assert.That(((Func<bool>)a)(), Is.EqualTo(43.21f >= 43.22));

		a = compiler.Compile("34u != 34m");
		Assert.IsInstanceOf<Func<bool>>(a);
		Assert.That(((Func<bool>)a)(), Is.EqualTo(34u != 34m));
	}

	[Test]
	public void StaticMembers()
	{
		Delegate a;
		Compiler compiler = new Compiler();

		a = compiler.Compile("Math.PI");
		Assert.IsInstanceOf<Func<double>>(a);
		Assert.That(((Func<double>)a)(), Is.EqualTo(Math.PI));

		a = compiler.Compile("Math.E");
		Assert.IsInstanceOf<Func<double>>(a);
		Assert.That(((Func<double>)a)(), Is.EqualTo(Math.E));

		a = compiler.Compile("+Math.PI / Math.E");
		Assert.IsInstanceOf<Func<double>>(a);
		Assert.That(((Func<double>)a)(), Is.EqualTo(+Math.PI / Math.E));

		a = compiler.Compile("string.Empty + \"Hello\" + \"World\" + \"!\"");
		Assert.IsInstanceOf<Func<string>>(a);
		Assert.That(((Func<string>)a)(), Is.EqualTo(string.Empty + "Hello" + "World" + "!"));

		a = compiler.Compile("DateTime.Now - DateTime.UtcNow > TimeSpan.Zero");
		Assert.IsInstanceOf<Func<bool>>(a);
		Assert.That(((Func<bool>)a)(), Is.EqualTo(DateTime.Now - DateTime.UtcNow > TimeSpan.Zero));

		a = compiler.Compile("int.MaxValue + 10");
		Assert.IsInstanceOf<Func<int>>(a);
		int result; unchecked { result = int.MaxValue + 10; }
		Assert.That(((Func<int>)a)(), Is.EqualTo(result));

		a = compiler.Compile("ushort.MaxValue + 1L");
		Assert.IsInstanceOf<Func<long>>(a);
		Assert.That(((Func<long>)a)(), Is.EqualTo(ushort.MaxValue + 1L));

		a = compiler.Compile("~(byte.MaxValue - 2035)");
		Assert.IsInstanceOf<Func<int>>(a);
		Assert.That(((Func<int>)a)(), Is.EqualTo(~(byte.MaxValue - 2035)));
	}

	[Test]
	public void SimpleTypedDelegates()
	{
		Delegate a;
		string s;
		Compiler compiler = new Compiler();

		a = compiler.Compile("Substring(0, 5)", typeof(string));
		Assert.IsInstanceOf<Func<string, string>>(a);
		s = "Hello World!";
		Assert.That(((Func<string, string>)a)(s), Is.EqualTo(s.Substring(0, 5)));
		s = "This is a test";
		Assert.That(((Func<string, string>)a)(s), Is.EqualTo(s.Substring(0, 5)));

		a = compiler.Compile("Replace('e', 'a')", typeof(string));
		Assert.IsInstanceOf<Func<string, string>>(a);
		s = "Hello World!";
		Assert.That(((Func<string, string>)a)(s), Is.EqualTo(s.Replace('e', 'a')));
		s = "This is a test";
		Assert.That(((Func<string, string>)a)(s), Is.EqualTo(s.Replace('e', 'a')));

		int x;

		a = compiler.Compile("this + 4", typeof(int));
		Assert.IsInstanceOf<Func<int, int>>(a);
		x = 5;
		Assert.That(((Func<int, int>)a)(x), Is.EqualTo(x + 4));
		x = 10;
		Assert.That(((Func<int, int>)a)(x), Is.EqualTo(x + 4));

		a = compiler.Compile("'The special number is ' + this", typeof(int));
		Assert.IsInstanceOf<Func<int, string>>(a);
		x = 5;
		Assert.That(((Func<int, string>)a)(x), Is.EqualTo("The special number is " + x));
		x = 10;
		Assert.That(((Func<int, string>)a)(x), Is.EqualTo("The special number is " + x));

		a = compiler.Compile("int.Parse(this.Replace('x', '4'))", typeof(string));
		Assert.IsInstanceOf<Func<string, int>>(a);
		s = "x123x";
		Assert.That(((Func<string, int>)a)(s), Is.EqualTo(int.Parse(s.Replace('x', '4'))));
		s = "23xx98";
		Assert.That(((Func<string, int>)a)(s), Is.EqualTo(int.Parse(s.Replace('x', '4'))));

		DateTime? dateTime;

		a = compiler.Compile("this.Value.AddDays(1)", typeof(DateTime?));
		Assert.IsInstanceOf<Func<DateTime?, DateTime>>(a);
		dateTime = DateTime.Now;
		Assert.That(((Func<DateTime?, DateTime>)a)(dateTime), Is.EqualTo(dateTime.Value.AddDays(1)));

		a = compiler.Compile("this.Value.AddHours(2) - this.Value", typeof(DateTime?));
		Assert.IsInstanceOf<Func<DateTime?, TimeSpan>>(a);
		dateTime = DateTime.Now;
		Assert.That(((Func<DateTime?, TimeSpan>)a)(dateTime), Is.EqualTo(dateTime.Value.AddHours(2) - dateTime.Value));

		a = compiler.Compile("this.HasValue ? this.Value : DateTime.MinValue", typeof(DateTime?));
		Assert.IsInstanceOf<Func<DateTime?, DateTime>>(a);
		dateTime = DateTime.Now;
		Assert.That(((Func<DateTime?, DateTime>)a)(dateTime), Is.EqualTo(dateTime.Value));
		dateTime = null;
		Assert.That(((Func<DateTime?, DateTime>)a)(dateTime), Is.EqualTo(DateTime.MinValue));
	}

	[Test]
	public void OperatorChecklist()
	{
		Delegate a;
		Compiler compiler = new Compiler();

		// Additive

		a = compiler.Compile("124f + 5293e-2f");
		Assert.IsInstanceOf<Func<float>>(a);
		Assert.That(((Func<float>)a)(), Is.EqualTo(124f + 5293e-2f));

		a = compiler.Compile("0x462a7f - 1_324_402");
		Assert.IsInstanceOf<Func<int>>(a);
		Assert.That(((Func<int>)a)(), Is.EqualTo(0x462a7f - 1_324_402));

		// Multiplicative

		a = compiler.Compile("4m * Math.PI");
		Assert.IsInstanceOf<Func<decimal>>(a);
		Assert.That(((Func<decimal>)a)(), Is.EqualTo(4m * (decimal)Math.PI));

		a = compiler.Compile("1.23d / 4.56d");
		Assert.IsInstanceOf<Func<double>>(a);
		Assert.That(((Func<double>)a)(), Is.EqualTo(1.23d / 4.56d));

		a = compiler.Compile("131u % 4u");
		Assert.IsInstanceOf<Func<uint>>(a);
		Assert.That(((Func<uint>)a)(), Is.EqualTo(131u % 4u));

		// Unary

		a = compiler.Compile("-(1 + 2)");
		Assert.IsInstanceOf<Func<int>>(a);
		Assert.That(((Func<int>)a)(), Is.EqualTo(-(1 + 2)));

		a = compiler.Compile("~45");
		Assert.IsInstanceOf<Func<int>>(a);
		Assert.That(((Func<int>)a)(), Is.EqualTo(~45));

		a = compiler.Compile("!true");
		Assert.IsInstanceOf<Func<bool>>(a);
		Assert.That(((Func<bool>)a)(), Is.EqualTo(!true));

		a = compiler.Compile("+'r'");
		Assert.IsInstanceOf<Func<int>>(a);
		Assert.That(((Func<int>)a)(), Is.EqualTo(+'r'));

		int y;

		a = compiler.Compile("++this", typeof(int));
		Assert.IsInstanceOf<Func<int, int>>(a);
		y = 5;
		Assert.That(((Func<int, int>)a)(5), Is.EqualTo(++y));

		a = compiler.Compile("--this", typeof(int));
		Assert.IsInstanceOf<Func<int, int>>(a);
		y = 5;
		Assert.That(((Func<int, int>)a)(5), Is.EqualTo(--y));

		a = compiler.Compile("this++", typeof(int));
		Assert.IsInstanceOf<Func<int, int>>(a);
		y = 5;
		Assert.That(((Func<int, int>)a)(5), Is.EqualTo(y++));

		a = compiler.Compile("this--", typeof(int));
		Assert.IsInstanceOf<Func<int, int>>(a);
		y = 5;
		Assert.That(((Func<int, int>)a)(5), Is.EqualTo(y--));

		// Shift

		a = compiler.Compile("1 << 2");
		Assert.IsInstanceOf<Func<int>>(a);
		Assert.That(((Func<int>)a)(), Is.EqualTo(1 << 2));

		a = compiler.Compile("0x45 >> 2");
		Assert.IsInstanceOf<Func<int>>(a);
		Assert.That(((Func<int>)a)(), Is.EqualTo(0x45 >> 2));

		// Relational

		a = compiler.Compile("1 < 2");
		Assert.IsInstanceOf<Func<bool>>(a);
		Assert.That(((Func<bool>)a)(), Is.EqualTo(1 < 2));

		a = compiler.Compile("5.2 <= 2.4");
		Assert.IsInstanceOf<Func<bool>>(a);
		Assert.That(((Func<bool>)a)(), Is.EqualTo(5.2 <= 2.4));

		a = compiler.Compile("1 > 2");
		Assert.IsInstanceOf<Func<bool>>(a);
		Assert.That(((Func<bool>)a)(), Is.EqualTo(1 > 2));

		a = compiler.Compile("5.2 >= 2.4");
		Assert.IsInstanceOf<Func<bool>>(a);
		Assert.That(((Func<bool>)a)(), Is.EqualTo(5.2 >= 2.4));

		// Equality

		a = compiler.Compile("1 == 2");
		Assert.IsInstanceOf<Func<bool>>(a);
		Assert.That(((Func<bool>)a)(), Is.EqualTo(1 == 2));

		a = compiler.Compile("'Hello' != 'Hello'");
		Assert.IsInstanceOf<Func<bool>>(a);
		Assert.That(((Func<bool>)a)(), Is.EqualTo("Hello" != "Hello"));

		// Logical AND / OR

		a = compiler.Compile("true && true");
		Assert.IsInstanceOf<Func<bool>>(a);
		Assert.That(((Func<bool>)a)(), Is.EqualTo(true && true));

		a = compiler.Compile("false || true");
		Assert.IsInstanceOf<Func<bool>>(a);
		Assert.That(((Func<bool>)a)(), Is.EqualTo(false || true));

		// Bitwise AND / OR / XOR

		a = compiler.Compile("0x45 & 0x23");
		Assert.IsInstanceOf<Func<int>>(a);
		Assert.That(((Func<int>)a)(), Is.EqualTo(0x45 & 0x23));

		a = compiler.Compile("0x45 | 0x23");
		Assert.IsInstanceOf<Func<int>>(a);
		Assert.That(((Func<int>)a)(), Is.EqualTo(0x45 | 0x23));

		a = compiler.Compile("0x45 ^ 0x23");
		Assert.IsInstanceOf<Func<int>>(a);
		Assert.That(((Func<int>)a)(), Is.EqualTo(0x45 ^ 0x23));

		// Conditional

		a = compiler.Compile("true ? 1 : 2");
		Assert.IsInstanceOf<Func<int>>(a);
		Assert.That(((Func<int>)a)(), Is.EqualTo(true ? 1 : 2));

		a = compiler.Compile("false ? 1 : 2");
		Assert.IsInstanceOf<Func<int>>(a);
		Assert.That(((Func<int>)a)(), Is.EqualTo(false ? 1 : 2));

		// Assignments

		a = compiler.Compile("this = 5", typeof(int));
		Assert.IsInstanceOf<Func<int, int>>(a);
		y = 5;
		Assert.That(((Func<int, int>)a)(5), Is.EqualTo(y = 5));

		a = compiler.Compile("this += 5", typeof(int));
		Assert.IsInstanceOf<Func<int, int>>(a);
		y = 5;
		Assert.That(((Func<int, int>)a)(5), Is.EqualTo(y += 5));

		a = compiler.Compile("this -= 63", typeof(int));
		Assert.IsInstanceOf<Func<int, int>>(a);
		y = 5;
		Assert.That(((Func<int, int>)a)(5), Is.EqualTo(y -= 63));

		a = compiler.Compile("this *= 5", typeof(int));
		Assert.IsInstanceOf<Func<int, int>>(a);
		y = 5;
		Assert.That(((Func<int, int>)a)(5), Is.EqualTo(y *= 5));

		a = compiler.Compile("this /= 3", typeof(int));
		Assert.IsInstanceOf<Func<int, int>>(a);
		y = 5;
		Assert.That(((Func<int, int>)a)(5), Is.EqualTo(y /= 3));

		a = compiler.Compile("this %= 2", typeof(int));
		Assert.IsInstanceOf<Func<int, int>>(a);
		y = 5;
		Assert.That(((Func<int, int>)a)(5), Is.EqualTo(y %= 2));

		a = compiler.Compile("this <<= 2", typeof(int));
		Assert.IsInstanceOf<Func<int, int>>(a);
		y = 5;
		Assert.That(((Func<int, int>)a)(5), Is.EqualTo(y <<= 2));

		a = compiler.Compile("this >>= 5", typeof(int));
		Assert.IsInstanceOf<Func<int, int>>(a);
		y = 5;
		Assert.That(((Func<int, int>)a)(5), Is.EqualTo(y >>= 5));

		a = compiler.Compile("this &= 0x45", typeof(int));
		Assert.IsInstanceOf<Func<int, int>>(a);
		y = 5;
		Assert.That(((Func<int, int>)a)(5), Is.EqualTo(y &= 0x45));

		a = compiler.Compile("this |= 0x23", typeof(int));
		Assert.IsInstanceOf<Func<int, int>>(a);
		y = 5;
		Assert.That(((Func<int, int>)a)(5), Is.EqualTo(y |= 0x23));

		a = compiler.Compile("this ^= 0x88", typeof(int));
		Assert.IsInstanceOf<Func<int, int>>(a);
		y = 5;
		Assert.That(((Func<int, int>)a)(5), Is.EqualTo(y ^= 0x88));

		// Nullish

		int? z;
		int? w;

		a = compiler.Compile("this ?? 5", typeof(int?));
		Assert.IsInstanceOf<Func<int?, int>>(a);
		z = null;
		Assert.That(((Func<int?, int>)a)(z), Is.EqualTo(5));
		z = 10;
		Assert.That(((Func<int?, int>)a)(z), Is.EqualTo(10));

		a = compiler.Compile("this ??= 10", typeof(int?));
		Assert.IsInstanceOf<Func<int?, int?>>(a);
		z = 5;
		w = 5;
		Assert.That(((Func<int?, int?>)a)(z), Is.EqualTo(5));
		z = 5;
		Assert.That(((Func<int?, int?>)a)(z), Is.EqualTo(w ??= 10));
		z = null;
		w = null;
		Assert.That(((Func<int?, int?>)a)(z), Is.EqualTo(10));
		z = null;
		Assert.That(((Func<int?, int?>)a)(z), Is.EqualTo(w ??= 10));
	}

	[Test]
	public void NullConditionals()
	{
		Delegate a;
		string? s;
		Compiler compiler = new Compiler();

		a = compiler.Compile("this?.Substring(0, 5)", typeof(string));
		Assert.IsInstanceOf<Func<string?, string?>>(a);
		s = null;
		Assert.That(((Func<string?, string?>)a)(s), Is.EqualTo(s?.Substring(0, 5)));
		s = "Hello World!";
		Assert.That(((Func<string?, string?>)a)(s), Is.EqualTo(s?.Substring(0, 5)));

		a = compiler.Compile("this?.Replace('e', 'a')?.Split('l')", typeof(string));
		Assert.IsInstanceOf<Func<string?, string[]>>(a);
		s = null;
		Assert.That(((Func<string?, string[]>)a)(s), Is.EqualTo(s?.Replace('e', 'a')?.Split('l')));
		s = "Hello World!";
		Assert.That(((Func<string?, string[]>)a)(s), Is.EqualTo(s?.Replace('e', 'a')?.Split('l')));

		int? x;

		a = compiler.Compile("this?.ToString() + 4", typeof(int?));
		Assert.IsInstanceOf<Func<int?, string>>(a);
		x = null;
		Assert.That(((Func<int?, string>)a)(x), Is.EqualTo(x?.ToString() + 4));
		x = 5;
		Assert.That(((Func<int?, string>)a)(x), Is.EqualTo(x?.ToString() + 4));
	}

	[Test]
	public void Indexers()
	{
		Delegate a;
		string[] s;
		Compiler compiler = new Compiler();

		a = compiler.Compile("this[0]", typeof(string[]));
		Assert.IsInstanceOf<Func<string[], string>>(a);
		s = new string[] { "Hello", "World!" };
		Assert.That(((Func<string[], string>)a)(s), Is.EqualTo(s[0]));
		s = new string[] { "This", "is", "a", "test" };
		Assert.That(((Func<string[], string>)a)(s), Is.EqualTo(s[0]));

		a = compiler.Compile("this[1]", typeof(string[]));
		Assert.IsInstanceOf<Func<string[], string>>(a);
		s = new string[] { "Hello", "World!" };
		Assert.That(((Func<string[], string>)a)(s), Is.EqualTo(s[1]));
		s = new string[] { "This", "is", "a", "test" };
		Assert.That(((Func<string[], string>)a)(s), Is.EqualTo(s[1]));

		a = compiler.Compile("this[2]", typeof(string[]));
		Assert.IsInstanceOf<Func<string[], string>>(a);
		s = new string[] { "Hello", "World!" };
		Assert.Throws<IndexOutOfRangeException>(() => ((Func<string[], string>)a)(s));
		s = new string[] { "This", "is", "a", "test" };
		Assert.That(((Func<string[], string>)a)(s), Is.EqualTo(s[2]));

		string s2 = "Hello World!";

		a = compiler.Compile("this[0]", typeof(string));
		Assert.IsInstanceOf<Func<string, char>>(a);
		Assert.That(((Func<string, char>)a)(s2), Is.EqualTo(s2[0]));

		a = compiler.Compile("this[1]", typeof(string));
		Assert.IsInstanceOf<Func<string, char>>(a);
		Assert.That(((Func<string, char>)a)(s2), Is.EqualTo(s2[1]));

		a = compiler.Compile("this[2]", typeof(string));
		Assert.IsInstanceOf<Func<string, char>>(a);
		Assert.That(((Func<string, char>)a)(s2), Is.EqualTo(s2[2]));

		a = compiler.Compile("this[3]", typeof(string));
		Assert.IsInstanceOf<Func<string, char>>(a);
		Assert.That(((Func<string, char>)a)(s2), Is.EqualTo(s2[3]));

		int[,] ints = new int[,] { { 1, 2, 3 }, { 4, 5, 6 } };

		a = compiler.Compile("this[0, 0]", typeof(int[,]));
		Assert.IsInstanceOf<Func<int[,], int>>(a);
		Assert.That(((Func<int[,], int>)a)(ints), Is.EqualTo(ints[0, 0]));

		a = compiler.Compile("this[0, 1]", typeof(int[,]));
		Assert.IsInstanceOf<Func<int[,], int>>(a);
		Assert.That(((Func<int[,], int>)a)(ints), Is.EqualTo(ints[0, 1]));

		a = compiler.Compile("this[0, 2]", typeof(int[,]));
		Assert.IsInstanceOf<Func<int[,], int>>(a);
		Assert.That(((Func<int[,], int>)a)(ints), Is.EqualTo(ints[0, 2]));

		a = compiler.Compile("this[1, 0]", typeof(int[,]));
		Assert.IsInstanceOf<Func<int[,], int>>(a);
		Assert.That(((Func<int[,], int>)a)(ints), Is.EqualTo(ints[1, 0]));

		a = compiler.Compile("this[1, 1]", typeof(int[,]));
		Assert.IsInstanceOf<Func<int[,], int>>(a);
		Assert.That(((Func<int[,], int>)a)(ints), Is.EqualTo(ints[1, 1]));

		char[][] chars = new char[][] { new char[] { 'H', 'e', 'l', 'l', 'o' }, new char[] { 'W', 'o', 'r', 'l', 'd' } };

		a = compiler.Compile("this[0][0]", typeof(char[][]));
		Assert.IsInstanceOf<Func<char[][], char>>(a);
		Assert.That(((Func<char[][], char>)a)(chars), Is.EqualTo(chars[0][0]));

		a = compiler.Compile("this[0][1]", typeof(char[][]));
		Assert.IsInstanceOf<Func<char[][], char>>(a);
		Assert.That(((Func<char[][], char>)a)(chars), Is.EqualTo(chars[0][1]));

		a = compiler.Compile("this[0][2]", typeof(char[][]));
		Assert.IsInstanceOf<Func<char[][], char>>(a);
		Assert.That(((Func<char[][], char>)a)(chars), Is.EqualTo(chars[0][2]));
	}

	[Test]
	public void MultipleParameters()
	{
		// Parameters are reference by {number}. The first parameter is {0}, the second is {1}, etc. The first parameter is always the "Host" object where members do not need to be prefixed with a parameter index. Also, the first parameter can be optionally referenced by "this" instead of {0}.

		Func<object, object> one;
		Compiler compiler = new Compiler();

		one = compiler.CompileAnonymous("{0}.ToString()", typeof(object));
		Assert.That(one(5), Is.EqualTo("5"));
		Assert.That(one("Hello"), Is.EqualTo("Hello"));
		Assert.That(one(5.5), Is.EqualTo("5.5"));

		one = compiler.CompileAnonymous("this.ToString()", typeof(object));
		Assert.That(one(5), Is.EqualTo("5"));
		Assert.That(one("Hello"), Is.EqualTo("Hello"));
		Assert.That(one(5.5), Is.EqualTo("5.5"));

		one = compiler.CompileAnonymous("{0} + 5.ToString() + \"SomeString\"", typeof(object));
		Assert.That(one(5), Is.EqualTo(5 + 5.ToString() + "SomeString"));
		Assert.That(one("Hello"), Is.EqualTo("Hello" + 5.ToString() + "SomeString"));
		Assert.That(one(5.5), Is.EqualTo(5.5 + 5.ToString() + "SomeString"));


		Func<object, object, object> two;

		two = compiler.CompileAnonymous("{0} + {1}", typeof(int), typeof(int));
		Assert.That(two(5, 10), Is.EqualTo(5 + 10));
		Assert.That(two(1053, 705), Is.EqualTo(1053 + 705));

		two = compiler.CompileAnonymous("{0} + {1}", typeof(string), typeof(string));
		Assert.That(two("Hello", "World"), Is.EqualTo("Hello" + "World"));
		Assert.That(two("This is a ", "test"), Is.EqualTo("This is a " + "test"));

		two = compiler.CompileAnonymous("{0} >= {1}", typeof(int), typeof(int));
		Assert.That(two(5, 10), Is.EqualTo(5 >= 10));
		Assert.That(two(1053, 705), Is.EqualTo(1053 >= 705));

		two = compiler.CompileAnonymous("Math.Min({0}, {1})", typeof(int), typeof(int));
		Assert.That(two(5, 10), Is.EqualTo(Math.Min(5, 10)));
		Assert.That(two(1053, 705), Is.EqualTo(Math.Min(1053, 705)));

		two = compiler.CompileAnonymous("'Result is : ' + {0} + ' % ' + {1} + ' or ' + (int.Parse({0}) % {1})", typeof(string), typeof(int));
		Assert.That(two("5", 10), Is.EqualTo("Result is : " + "5" + " % " + 10 + " or " + (int.Parse("5") % 10)));
		Assert.That(two("1053", 705), Is.EqualTo("Result is : " + "1053" + " % " + 705 + " or " + (int.Parse("1053") % 705)));


		Func<object, object, object, object> three;

		three = compiler.CompileAnonymous("(-{1} + Math.Sqrt({1} * {1} - 4 * {0} * {2})) / (2 * {0})", typeof(double), typeof(double), typeof(double));
		Assert.That(three(1d, 5d, 6d), Is.EqualTo((-5d + Math.Sqrt(5d * 5d - 4 * 1d * 6d)) / (2 * 1d)));
		Assert.That(three(1d, 3d, 2d), Is.EqualTo((-3d + Math.Sqrt(3d * 3d - 4 * 1d * 2d)) / (2 * 1d)));

		three = compiler.CompileAnonymous("{0}.Substring({1}, {2})", typeof(string), typeof(int), typeof(int));
		Assert.That(three("Hello World!", 0, 5), Is.EqualTo("Hello World!".Substring(0, 5)));

		three = compiler.CompileAnonymous("{0}.Replace({1}, {2})", typeof(string), typeof(char), typeof(char));
		Assert.That(three("Hello World!", 'e', 'a'), Is.EqualTo("Hello World!".Replace('e', 'a')));


		Func<object, object, object, object, object> four;

		four = compiler.CompileAnonymous("{0} * {1} + 0.5d * {2} * {1} * {1} + 1d/6d * {3} * {1} * {1} * {1}", typeof(double), typeof(double), typeof(double), typeof(double));
		Assert.That(four(1d, 2d, 3d, 4d), Is.EqualTo(1d * 2d + 0.5 * 3d * 2d * 2d + 1d / 6d * 4d * 2d * 2d * 2d));
		Assert.That(four(5d, 6d, 7d, 8d), Is.EqualTo(5d * 6d + 0.5 * 7d * 6d * 6d + 1d / 6d * 8d * 6d * 6d * 6d));

		four = compiler.CompileAnonymous("{0} >= 4 && {1} >= 4 ? {2} : {3}", typeof(int), typeof(int), typeof(int), typeof(int));

		Assert.That(four(5, 5, 10, 20), Is.EqualTo(5 >= 4 && 5 >= 4 ? 10 : 20));
		Assert.That(four(3, 5, 10, 20), Is.EqualTo(3 >= 4 && 5 >= 4 ? 10 : 20));
		Assert.That(four(5, 3, 10, 20), Is.EqualTo(5 >= 4 && 3 >= 4 ? 10 : 20));
	}
}