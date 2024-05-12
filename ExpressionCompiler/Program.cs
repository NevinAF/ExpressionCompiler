using System.Reflection;
using System.Runtime.InteropServices;

namespace NAF.ExpressionCompiler
{
	public class Program
	{
		class Example {
			public string value = "";
			public static string operator +(Example example, int other) => example.value + other.ToString();
		}

		public static void Main()
		{
			Compiler compiler = new Compiler();

			object AddOp(object left, object right) =>
				compiler.CompileAnonymous("{0} + {1}", left.GetType(), right.GetType())(left, right);

			Console.WriteLine(AddOp(3, 8)); // 11
			Console.WriteLine(AddOp("Hello", " World")); // "Hello World"
			Console.WriteLine(AddOp(3.14, 2.71)); // 5.85f

			Example example = new Example { value = "The answer is " };
			Console.WriteLine(AddOp(example, 42)); // "The answer is 42"
		}
	}

	
}