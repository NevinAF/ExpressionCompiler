.NET Expression Compiler
========================

The .NET Expression Compiler is a library that allows you to compile string expressions into C# code at runtime. This is intended to be an extremely lightweight alternative to using a full-blown compiler.

This library uses `System.Reflection` and `System.Linq.Expressions` to compile expressions into delegates and is highly optimized for performance. There is hardly any intrinsic performance cost with most of the performance coming from compiling the `System.Linq.Expressions` tree. That is, compiling expressions from strings is nearly as fast as manually writing a whole expression tree.

## Why?

This project was initially created to work around the limitations of C# attributes by allowing you to define lambda expressions as string literals. Attributes can then use the `Compiler` class to quickly compile the expression into a delegate.

```csharp
public class AttributeExample : NAFSampleBehaviour
{
	[Validate("{1} > 0")]
	public int Positive = -12;
	[Validate("{1}.Length < 24")]
	public string LessThan24Chars = "Hello World and everyone who is reading this!";
	[Validate("float.IsNaN({1}.x) || float.IsNaN({1}.y)")]
	public Vector2 OneIsNaN = new Vector2(1, 2);

	[Validate("{1}.Length > 0", Icon = EditorIcons.d_console_warnicon, Label = "Empty is Skipped!")]
	public int[] ArrayNotEmptyWarning = new int[0];

	[EnableIf("=(bool)gameobject && gameobject.activeSelf")]
	public string Gameobject_Is_Active = "Enabled if the gameobject active.";

	[InlineLabel(Label = "Math.Round({1}) + ' is a nicer number'")]
	public double RoundedLabel = 4.35d;
}
```


## Usage

This library is very simple to use, but can be a little tricky to understand at first. Most complications will come from trying to type the expression correctly, although this can almost always be worked around with Delegate.DynamicInvoke.

All functionality in this library is accessed through the `Compiler` class. This class is a manager and buffer for single threaded compilation. To compile an expression, simply create a new instance of `Compiler` and call one of the methods listed below. All methods take in a `ReadonlySpan<char>` parameter named `expression` which is explained more in the next section

### Delegate Compilation

Most simple use cases will use the `Compiler.Compile<T>(ReadonlySpan<char> expression)` method, where `T` is a delegate type and the parameters/return is casted to the expected types. This is the most versatile method and allows for performant execution without needing to cast/spread parameters like with `Delegate.DynamicInvoke`.

```csharp
class Example { public int Value = 6; }
public static void Main()
{
	Compiler compiler = new Compiler();

	Func<int, int, int> addFunc = compiler.Compile<Func<int, int, int>>("{0} + {1}");
	Console.WriteLine(addFunc(5, 7)); // 12

	Func<string[], int> getLengthFunc = compiler.Compile<Func<string[], int>>("Length");
	Console.WriteLine(getLengthFunc(new string[] { "Hello", "World" })); // 2

	Action<Example, int> incValueFunc = compiler.Compile<Action<Example, int>>("Value += {1}");
	Example example = new Example();
	incValueFunc(example, 3);
	Console.WriteLine(example.Value); // 9
}
```

### Anonymous/Boxed Compilation

If you don't know the delegate type at compile time, you can use the `Compiler.CompileAnonymous(ReadonlySpan<char> expression, params Type[] parameters)` method. This method will return a `Func<object, ..., object>` which will automatically box/unbox parameters to match the expected types. This allows for a `Delegate.DynamicInvoke` like experience, but without the performance cost.

> Using this method will prevent the parameters from being writable, preventing value type assignments. This is due to the boxing/unboxing process.

```csharp
class Example {
	public string value;
	public static string operator +(Example example, int other) => example.value + other.ToString();
}

public static void Main()
{
	Compiler compiler = new Compiler();

	// The compiler delegate should likely be stored in a field to prevent recompilation
	object AddOp(object left, object right) =>
		compiler.CompileAnonymous("{0} + {1}", left.GetType(), right.GetType())(left, right);

	Console.WriteLine(AddOp(3, 8)); // 11
	Console.WriteLine(AddOp("Hello", " World")); // "Hello World"
	Console.WriteLine(AddOp(3.14, 2.71)); // 5.85f

	Example example = new Example { value = "The answer is " };
	Console.WriteLine(AddOp(example, 42)); // "The answer is 42"
}
```

### Attribute with Expressions Example

```csharp
public class Program
{
	public int IntField = 5;
	public float FloatField = 7.3f;
	[BufferSize(sizeof(float) * 3)] public float[] FloatArray = new float[] { 1.1f, 2.2f, 3.3f };
	[BufferSize("IntField + 2")] public string StringField;
	[BufferSize("Math.Round(FloatField) * 2")] public ushort[] UShortArray;
	[BufferSize("Math.PI + FloatArray[2]")] public byte[] ByteArray;
	[BufferSize("InnerClass == null ? 0 : InnerClass.FloatArray.Length")] public Program InnerClass;

	public static void Main(string[] args)
	{
		Program example = new Program();

		Console.WriteLine(BufferSizeAttribute.GetSize(example, "IntField"));    // 4
		Console.WriteLine(BufferSizeAttribute.GetSize(example, "FloatField"));  // 4
		Console.WriteLine(BufferSizeAttribute.GetSize(example, "FloatArray"));  // 12
		Console.WriteLine(BufferSizeAttribute.GetSize(example, "StringField")); // 7
		Console.WriteLine(BufferSizeAttribute.GetSize(example, "UShortArray")); // 14
		Console.WriteLine(BufferSizeAttribute.GetSize(example, "ByteArray"));   // 6
		Console.WriteLine(BufferSizeAttribute.GetSize(example, "InnerClass"));  // 0

		example.InnerClass = new Program
		{
			FloatArray = new float[] { 1.1f, 2.2f, 3.3f, 4.4f }
		};
		example.UShortArray = new ushort[] { 1, 2, 3, 4, 5 };
		example.IntField = 7;
		example.FloatField = 8.8f;
		example.StringField = "Hello, World!";

		Console.WriteLine(BufferSizeAttribute.GetSize(example, "IntField"));    // 4
		Console.WriteLine(BufferSizeAttribute.GetSize(example, "FloatField"));  // 4
		Console.WriteLine(BufferSizeAttribute.GetSize(example, "FloatArray"));  // 12
		Console.WriteLine(BufferSizeAttribute.GetSize(example, "StringField")); // 9
		Console.WriteLine(BufferSizeAttribute.GetSize(example, "UShortArray")); // 18
		Console.WriteLine(BufferSizeAttribute.GetSize(example, "ByteArray"));   // 6
		Console.WriteLine(BufferSizeAttribute.GetSize(example, "InnerClass"));  // 4
	}
}

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class BufferSizeAttribute : Attribute
{
	private static Compiler _compiler = new Compiler();

	public readonly object _size;
	public BufferSizeAttribute(object size) => _size = size;

	public static int GetSize<T>(T target, string fieldName)
	{
		var field = typeof(T).GetField(fieldName);
		var attr = field.GetCustomAttribute<BufferSizeAttribute>();

		if (attr == null)
		{
			if (field.FieldType.IsValueType)
				return Marshal.SizeOf(field.FieldType);
			else throw new ArgumentException("Field must have a BufferSizeAttribute or be a value type.");
		}

		if (attr._size is int size)
			return size;
		
		// Even if this is just a float, the compiler will do all casting needed to get the size.
		return _compiler.Compile<Func<T, int>>(attr._size.ToString())(target);
	}
}
```

## Supported Syntax

Most C# syntax *expressions* are supported. This does not include blocks or declarations. There are two main differences between C# and expressions compiled by this library:

1. All parameters are notated with `{#}` where `#` is the index of the parameter. Parameters are not named for API simplicity. The first parameter, `{0}`, is always the context object, meaning that all properties and methods on this object can be accessed by name without the `{#}` notation. The `this` keyword is synonymous with `{0}`.
2. All single quotes `'` are treated as double quotes unless they capture exactly one character (in which case it is treated as a char literal). This prevents the need for escaping quotes in strings.

The following is a list of supported syntax. `var` is used to represent any expression, including literals, parameters, other member accesses or a parenthesized expression. `Type` is used to represent any type name.

- Literals: `1`, `1.0`, `"Hello, World!"`, `true`, `null`, `'a'`, `'\n'`, `35ul`, `3.14f`, `3.14d`, `3.14m`
- All operators including user defined overloads
	- Unary: `+`, `-`, `!`, `~`, `++` (pre/post), `--` (pre/post)
	- Binary: `+`, `-`, `*`, `/`, `%`, `&`, `|`, `^`, `<<`, `>>`, `&&`, `||`, `==`, `!=`, `<`, `>`, `<=`, `>=`, `??`, `?:`, `+=`, `-=`, `*=`, `/=`, `%=`, `&=`, `|=`, `^=`, `<<=`, `>>=`
	- Ternary: `? :`
- Member Access: `var.field`, `var.Property`, `var.Method()`, `var[var]`, `var.Method(var, var)`
- Index Access: `var[var]`, `var[var,var]`, `var[var,var,var]`
- Type Casts: `(Type)var`, `var as Type`
- Constructor Calls: `new Type(var, var)`
- Typeof: `typeof(Type)`
- Parenthesized Expressions: `(var)`
- Static Member Access: `Type.field`, `Type.Method()`