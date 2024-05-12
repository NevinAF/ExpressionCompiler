namespace ExpressionCompilerTests;

using System.Reflection;
using System.Runtime.InteropServices;
using NAF.ExpressionCompiler;

#nullable disable

public class BufferedClassExample
{
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

	public int IntField = 5;
	public float FloatField = 7.3f;
	[BufferSize(sizeof(float) * 3)] public float[] FloatArray = new float[] { 1.1f, 2.2f, 3.3f };
	[BufferSize("IntField + 2")] public string StringField;
	[BufferSize("Math.Round(FloatField) * 2")] public ushort[] UShortArray;
	[BufferSize("Math.PI + FloatArray[2]")] public byte[] ByteArray;
	[BufferSize("InnerClass == null ? 0 : InnerClass.FloatArray.Length")] public BufferedClassExample InnerClass;


	[Test]
	public void ValidateAttribute()
	{
		void Check(BufferedClassExample obj)
		{
			Assert.Multiple(() =>
			{
				Assert.That(BufferSizeAttribute.GetSize(obj, "IntField"), Is.EqualTo(sizeof(int)));
				Assert.That(BufferSizeAttribute.GetSize(obj, "FloatField"), Is.EqualTo(sizeof(float)));
				Assert.That(BufferSizeAttribute.GetSize(obj, "FloatArray"), Is.EqualTo(sizeof(float) * 3));
				Assert.That(BufferSizeAttribute.GetSize(obj, "StringField"), Is.EqualTo(obj.IntField + 2));
				Assert.That(BufferSizeAttribute.GetSize(obj, "UShortArray"), Is.EqualTo((int)(Math.Round(obj.FloatField) * 2)));
				Assert.That(BufferSizeAttribute.GetSize(obj, "ByteArray"), Is.EqualTo((int)(Math.PI + obj.FloatArray[2])));
				Assert.That(BufferSizeAttribute.GetSize(obj, "InnerClass"), Is.EqualTo(obj.InnerClass == null ? 0 : obj.InnerClass.FloatArray.Length));
			});
		}

		BufferedClassExample example = new BufferedClassExample();

		Check(example);

		example.InnerClass = new BufferedClassExample
		{
			FloatArray = new float[] { 1.1f, 2.2f, 3.3f, 4.4f }
		};

		Check(example);

		example.UShortArray = new ushort[] { 1, 2, 3, 4, 5 };
		example.IntField = 7;
		example.FloatField = 8.8f;
		example.StringField = "Hello, World!";

		Check(example);
	}
}