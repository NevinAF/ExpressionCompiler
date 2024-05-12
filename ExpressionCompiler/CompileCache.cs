#nullable enable
using System.Collections.Concurrent;

namespace NAF.ExpressionCompiler
{
	public class CompilerBag
	{
		private readonly ConcurrentBag<Compiler> _bag = new ConcurrentBag<Compiler>();
		public Type[]? ReferenceTypes = null;

		public Compiler Get()
		{
			if (_bag.TryTake(out Compiler? compiler))
			{
				compiler.ReferenceTypes = ReferenceTypes ?? ReflectionUtility.AllDeclaringTypes;
				return compiler;
			}

			return new Compiler(ReferenceTypes ?? ReflectionUtility.AllDeclaringTypes);
		}

		public void Return(Compiler compiler) => _bag.Add(compiler);
	
	}

	public static class CompileCache
	{
		private readonly struct Key
		{
			public readonly string expression;
			public readonly Type[] types;

			public Key(string expression, Type[] types)
			{
				this.expression = expression;
				this.types = types;
			}

			public override readonly bool Equals(object? obj)
			{
				if (obj is not Key key)
					return false;

				if (expression != key.expression || types.Length != key.types.Length)
					return false;

				for (int i = 0; i < types.Length; i++)
				{
					if (types[i] != key.types[i])
						return false;
				}

				return true;
			}

			public override readonly int GetHashCode()
			{
				int hash = expression.GetHashCode();
				for (int i = 0; i < types.Length; i++)
					hash = HashCode.Combine(hash, types[i].GetHashCode());

				return hash;
			}
		}

		private static readonly Dictionary<Key, Delegate> _cache = new Dictionary<Key, Delegate>();
		private static readonly CompilerBag _compilerBag = new CompilerBag();

		public static Type[]? ReferenceTypes
		{
			get => _compilerBag.ReferenceTypes;
			set => _compilerBag.ReferenceTypes = value;
		}

		public static Func<object> Compile(string expression)
		{
			Key key = new Key(expression, Type.EmptyTypes);

			if (_cache.TryGetValue(key, out Delegate? del))
				return (Func<object>)del;

			Compiler compiler = _compilerBag.Get();
			Func<object> func = compiler.CompileAnonymous(expression);
			_cache[key] = func;
			_compilerBag.Return(compiler);

			return func;
		}

		public static Func<object, object> Compile(string expression, Type t0)
		{
			
			Key key = new Key(expression, Type.EmptyTypes);

			if (_cache.TryGetValue(key, out Delegate? del))
				return (Func<object, object>)del;

			Compiler compiler = _compilerBag.Get();
			Func<object, object> func = compiler.CompileAnonymous(expression, t0);
			_cache[key] = func;
			_compilerBag.Return(compiler);

			return func;
		}

		public static Func<object, object, object> Compile(string expression, Type t0, Type t1)
		{
			Key key = new Key(expression, new Type[] { t0, t1 });

			if (_cache.TryGetValue(key, out Delegate? del))
				return (Func<object, object, object>)del;

			Compiler compiler = _compilerBag.Get();
			Func<object, object, object> func = compiler.CompileAnonymous(expression, t0, t1);
			_cache[key] = func;
			_compilerBag.Return(compiler);

			return func;
		}

		public static Func<object, object, object, object> Compile(string expression, Type t0, Type t1, Type t2)
		{
			Key key = new Key(expression, new Type[] { t0, t1, t2 });

			if (_cache.TryGetValue(key, out Delegate? del))
				return (Func<object, object, object, object>)del;

			Compiler compiler = _compilerBag.Get();
			Func<object, object, object, object> func = compiler.CompileAnonymous(expression, t0, t1, t2);
			_cache[key] = func;
			_compilerBag.Return(compiler);

			return func;
		}

		public static Func<object, object, object, object, object> Compile(string expression, Type t0, Type t1, Type t2, Type t3)
		{
			Key key = new Key(expression, new Type[] { t0, t1, t2, t3 });

			if (_cache.TryGetValue(key, out Delegate? del))
				return (Func<object, object, object, object, object>)del;

			Compiler compiler = _compilerBag.Get();
			Func<object, object, object, object, object> func = compiler.CompileAnonymous(expression, t0, t1, t2, t3);
			_cache[key] = func;
			_compilerBag.Return(compiler);

			return func;
		}
	}

	public static class CompileCache<T> where T : Delegate
	{
		private static readonly Dictionary<string, T> _cache = new Dictionary<string, T>();
		private static readonly CompilerBag _compilerBag = new CompilerBag();

		public static Type[]? ReferenceTypes
		{
			get => _compilerBag.ReferenceTypes;
			set => _compilerBag.ReferenceTypes = value;
		}

		public static T Compile(string expression)
		{
			if (_cache.TryGetValue(expression, out T? del))
				return del;

			Compiler compiler = _compilerBag.Get();
			T func = compiler.Compile<T>(expression);
			_cache[expression] = func;
			_compilerBag.Return(compiler);

			return func;
		}
	}
}