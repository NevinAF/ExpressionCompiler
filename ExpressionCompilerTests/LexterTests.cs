namespace ExpressionCompilerTests;

using NAF.ExpressionCompiler;

public class LexterTests
{
	private static void AssertTokens(string input, params Token[] expected)
	{
		int index = 0;
		Lexer lexer = new Lexer(input);

		Token token = default;
		do {
			Assert.That(index, Is.LessThan(expected.Length), input + ": Lexer is returning more tokens than expected! Ended at token " + token);

			token = lexer.NextToken();
			Assert.That(token, Is.EqualTo(expected[index]), input + ": Token at index " + index + " is not equal to expected token.");
			index++;
		} while (token.type != TokenType.EndOfFile);
	}

	[Test]
	public void Lexer_SimpleMath()
	{
		AssertTokens("1 + 2",
			new Token(TokenType.Number, 0, 1),
			new Token(TokenType.Plus, 2, 1),
			new Token(TokenType.Number, 4, 1),
			new Token(TokenType.EndOfFile, 5, 0)
		);

		AssertTokens("1 + 2 * 3",
			new Token(TokenType.Number, 0, 1),
			new Token(TokenType.Plus, 2, 1),
			new Token(TokenType.Number, 4, 1),
			new Token(TokenType.Multiplication, 6, 1),
			new Token(TokenType.Number, 8, 1),
			new Token(TokenType.EndOfFile, 9, 0)
		);

		AssertTokens("134 * 2234 * 323 / 467943",
			new Token(TokenType.Number, 0, 3),
			new Token(TokenType.Multiplication, 4, 1),
			new Token(TokenType.Number, 6, 4),
			new Token(TokenType.Multiplication, 11, 1),
			new Token(TokenType.Number, 13, 3),
			new Token(TokenType.Division, 17, 1),
			new Token(TokenType.Number, 19, 6),
			new Token(TokenType.EndOfFile, 25, 0)
		);
	}

	[Test]
	public void Lexer_NumberFormats()
	{
		AssertTokens("0.25f + 23",
			new Token(TokenType.Number, 0, 5),
			new Token(TokenType.Plus, 6, 1),
			new Token(TokenType.Number, 8, 2),
			new Token(TokenType.EndOfFile, 10, 0)
		);

		AssertTokens(".450 / 223d",
			new Token(TokenType.Number, 0, 4),
			new Token(TokenType.Division, 5, 1),
			new Token(TokenType.Number, 7, 4),
			new Token(TokenType.EndOfFile, 11, 0)
		);

		AssertTokens("23_2305_64u * 23.5e-2m",
			new Token(TokenType.Number, 0, 11),
			new Token(TokenType.Multiplication, 12, 1),
			new Token(TokenType.Number, 14, 8),
			new Token(TokenType.EndOfFile, 22, 0)
		);

		AssertTokens("23___34ul * 1_2_3_4.536",
			new Token(TokenType.Number, 0, 9),
			new Token(TokenType.Multiplication, 10, 1),
			new Token(TokenType.Number, 12, 11),
			new Token(TokenType.EndOfFile, 23, 0)
		);

		AssertTokens("123.ToString()",
			new Token(TokenType.Number, 0, 3),
			new Token(TokenType.MemberAccess, 3, 1),
			new Token(TokenType.Identifier, 4, 8),
			new Token(TokenType.ParenthesisOpen, 12, 1),
			new Token(TokenType.ParenthesisClose, 13, 1),
			new Token(TokenType.EndOfFile, 14, 0)
		);

		AssertTokens("123.34f.ToString()",
			new Token(TokenType.Number, 0, 7),
			new Token(TokenType.MemberAccess, 7, 1),
			new Token(TokenType.Identifier, 8, 8),
			new Token(TokenType.ParenthesisOpen, 16, 1),
			new Token(TokenType.ParenthesisClose, 17, 1),
			new Token(TokenType.EndOfFile, 18, 0)
		);

		AssertTokens("0xEFA35.034",
			new Token(TokenType.Number, 0, 11),
			new Token(TokenType.EndOfFile, 11, 0)
		);

		AssertTokens("2450.3250.034",
			new Token(TokenType.Number, 0, 13),
			new Token(TokenType.EndOfFile, 13, 0)
		);

		AssertTokens("0b1010.0x1234",
			new Token(TokenType.Number, 0, 13),
			new Token(TokenType.EndOfFile, 13, 0)
		);

		AssertTokens("0b10001023452",
			new Token(TokenType.Number, 0, 13),
			new Token(TokenType.EndOfFile, 13, 0)
		);

		AssertTokens("0b10001023452.0x1234",
			new Token(TokenType.Number, 0, 20),
			new Token(TokenType.EndOfFile, 20, 0)
		);

		AssertTokens("234.56e-23-144e+12+23",
			new Token(TokenType.Number, 0, 10),
			new Token(TokenType.Minus, 10, 1),
			new Token(TokenType.Number, 11, 7),
			new Token(TokenType.Plus, 18, 1),
			new Token(TokenType.Number, 19, 2),
			new Token(TokenType.EndOfFile, 21, 0)
		);

		AssertTokens("234.56e-23m-144e+12f+23b",
			new Token(TokenType.Number, 0, 11),
			new Token(TokenType.Minus, 11, 1),
			new Token(TokenType.Number, 12, 8),
			new Token(TokenType.Plus, 20, 1),
			new Token(TokenType.Number, 21, 3),
			new Token(TokenType.EndOfFile, 24, 0)
		);
	}

	[Test]
	public void Lexer_SimpleMemberAccess()
	{
		AssertTokens("a.b",
			new Token(TokenType.Identifier, 0, 1),
			new Token(TokenType.MemberAccess, 1, 1),
			new Token(TokenType.Identifier, 2, 1),
			new Token(TokenType.EndOfFile, 3, 0)
		);

		AssertTokens("a.b.c",
			new Token(TokenType.Identifier, 0, 1),
			new Token(TokenType.MemberAccess, 1, 1),
			new Token(TokenType.Identifier, 2, 1),
			new Token(TokenType.MemberAccess, 3, 1),
			new Token(TokenType.Identifier, 4, 1),
			new Token(TokenType.EndOfFile, 5, 0)
		);

		AssertTokens("a.b.c.d",
			new Token(TokenType.Identifier, 0, 1),
			new Token(TokenType.MemberAccess, 1, 1),
			new Token(TokenType.Identifier, 2, 1),
			new Token(TokenType.MemberAccess, 3, 1),
			new Token(TokenType.Identifier, 4, 1),
			new Token(TokenType.MemberAccess, 5, 1),
			new Token(TokenType.Identifier, 6, 1),
			new Token(TokenType.EndOfFile, 7, 0)
		);

		AssertTokens("a.b.c.d.e",
			new Token(TokenType.Identifier, 0, 1),
			new Token(TokenType.MemberAccess, 1, 1),
			new Token(TokenType.Identifier, 2, 1),
			new Token(TokenType.MemberAccess, 3, 1),
			new Token(TokenType.Identifier, 4, 1),
			new Token(TokenType.MemberAccess, 5, 1),
			new Token(TokenType.Identifier, 6, 1),
			new Token(TokenType.MemberAccess, 7, 1),
			new Token(TokenType.Identifier, 8, 1),
			new Token(TokenType.EndOfFile, 9, 0)
		);

		AssertTokens("Truely.Falsey",
			new Token(TokenType.Identifier, 0, 6),
			new Token(TokenType.MemberAccess, 6, 1),
			new Token(TokenType.Identifier, 7, 6),
			new Token(TokenType.EndOfFile, 13, 0)
		);

		AssertTokens("Truely.Falsey.Truthy",
			new Token(TokenType.Identifier, 0, 6),
			new Token(TokenType.MemberAccess, 6, 1),
			new Token(TokenType.Identifier, 7, 6),
			new Token(TokenType.MemberAccess, 13, 1),
			new Token(TokenType.Identifier, 14, 6),
			new Token(TokenType.EndOfFile, 20, 0)
		);
	}

	[Test]
	public void Lexer_MathWithAccesses()
	{
		AssertTokens("a.b + c.d",
			new Token(TokenType.Identifier, 0, 1),
			new Token(TokenType.MemberAccess, 1, 1),
			new Token(TokenType.Identifier, 2, 1),
			new Token(TokenType.Plus, 4, 1),
			new Token(TokenType.Identifier, 6, 1),
			new Token(TokenType.MemberAccess, 7, 1),
			new Token(TokenType.Identifier, 8, 1),
			new Token(TokenType.EndOfFile, 9, 0)
		);

		AssertTokens("a.b * c.d",
			new Token(TokenType.Identifier, 0, 1),
			new Token(TokenType.MemberAccess, 1, 1),
			new Token(TokenType.Identifier, 2, 1),
			new Token(TokenType.Multiplication, 4, 1),
			new Token(TokenType.Identifier, 6, 1),
			new Token(TokenType.MemberAccess, 7, 1),
			new Token(TokenType.Identifier, 8, 1),
			new Token(TokenType.EndOfFile, 9, 0)
		);

		AssertTokens("a.b * c.d / e.f",
			new Token(TokenType.Identifier, 0, 1),
			new Token(TokenType.MemberAccess, 1, 1),
			new Token(TokenType.Identifier, 2, 1),
			new Token(TokenType.Multiplication, 4, 1),
			new Token(TokenType.Identifier, 6, 1),
			new Token(TokenType.MemberAccess, 7, 1),
			new Token(TokenType.Identifier, 8, 1),
			new Token(TokenType.Division, 10, 1),
			new Token(TokenType.Identifier, 12, 1),
			new Token(TokenType.MemberAccess, 13, 1),
			new Token(TokenType.Identifier, 14, 1),
			new Token(TokenType.EndOfFile, 15, 0)
		);

		AssertTokens("System.Math.PI * System.Math.PI",
			new Token(TokenType.Identifier, 0, 6),
			new Token(TokenType.MemberAccess, 6, 1),
			new Token(TokenType.Identifier, 7, 4),
			new Token(TokenType.MemberAccess, 11, 1),
			new Token(TokenType.Identifier, 12, 2),
			new Token(TokenType.Multiplication, 15, 1),
			new Token(TokenType.Identifier, 17, 6),
			new Token(TokenType.MemberAccess, 23, 1),
			new Token(TokenType.Identifier, 24, 4),
			new Token(TokenType.MemberAccess, 28, 1),
			new Token(TokenType.Identifier, 29, 2),
			new Token(TokenType.EndOfFile, 31, 0)
		);

		AssertTokens("example * 234.56f / 344 + another.vector.x",
			new Token(TokenType.Identifier, 0, 7),
			new Token(TokenType.Multiplication, 8, 1),
			new Token(TokenType.Number, 10, 7),
			new Token(TokenType.Division, 18, 1),
			new Token(TokenType.Number, 20, 3),
			new Token(TokenType.Plus, 24, 1),
			new Token(TokenType.Identifier, 26, 7),
			new Token(TokenType.MemberAccess, 33, 1),
			new Token(TokenType.Identifier, 34, 6),
			new Token(TokenType.MemberAccess, 40, 1),
			new Token(TokenType.Identifier, 41, 1),
			new Token(TokenType.EndOfFile, 42, 0)
		);
	}

	[Test]
	public void Lexer_Parenthesis()
	{
		AssertTokens("(1 + 2)",
			new Token(TokenType.ParenthesisOpen, 0, 1),
			new Token(TokenType.Number, 1, 1),
			new Token(TokenType.Plus, 3, 1),
			new Token(TokenType.Number, 5, 1),
			new Token(TokenType.ParenthesisClose, 6, 1),
			new Token(TokenType.EndOfFile, 7, 0)
		);

		AssertTokens("(1 + 2) * 3",
			new Token(TokenType.ParenthesisOpen, 0, 1),
			new Token(TokenType.Number, 1, 1),
			new Token(TokenType.Plus, 3, 1),
			new Token(TokenType.Number, 5, 1),
			new Token(TokenType.ParenthesisClose, 6, 1),
			new Token(TokenType.Multiplication, 8, 1),
			new Token(TokenType.Number, 10, 1),
			new Token(TokenType.EndOfFile, 11, 0)
		);

		AssertTokens("1 + (2 * 3)",
			new Token(TokenType.Number, 0, 1),
			new Token(TokenType.Plus, 2, 1),
			new Token(TokenType.ParenthesisOpen, 4, 1),
			new Token(TokenType.Number, 5, 1),
			new Token(TokenType.Multiplication, 7, 1),
			new Token(TokenType.Number, 9, 1),
			new Token(TokenType.ParenthesisClose, 10, 1),
			new Token(TokenType.EndOfFile, 11, 0)
		);

		AssertTokens("1 + (2 * 3) * 4",
			new Token(TokenType.Number, 0, 1),
			new Token(TokenType.Plus, 2, 1),
			new Token(TokenType.ParenthesisOpen, 4, 1),
			new Token(TokenType.Number, 5, 1),
			new Token(TokenType.Multiplication, 7, 1),
			new Token(TokenType.Number, 9, 1),
			new Token(TokenType.ParenthesisClose, 10, 1),
			new Token(TokenType.Multiplication, 12, 1),
			new Token(TokenType.Number, 14, 1),
			new Token(TokenType.EndOfFile, 15, 0)
		);

		AssertTokens("1 + (2 * 3) * (4 + 5)",
			new Token(TokenType.Number, 0, 1),
			new Token(TokenType.Plus, 2, 1),
			new Token(TokenType.ParenthesisOpen, 4, 1),
			new Token(TokenType.Number, 5, 1),
			new Token(TokenType.Multiplication, 7, 1),
			new Token(TokenType.Number, 9, 1),
			new Token(TokenType.ParenthesisClose, 10, 1),
			new Token(TokenType.Multiplication, 12, 1),
			new Token(TokenType.ParenthesisOpen, 14, 1),
			new Token(TokenType.Number, 15, 1),
			new Token(TokenType.Plus, 17, 1),
			new Token(TokenType.Number, 19, 1),
			new Token(TokenType.ParenthesisClose, 20, 1),
			new Token(TokenType.EndOfFile, 21, 0)
		);

		AssertTokens("(int)Value + (float)123.45d",
			new Token(TokenType.ParenthesisOpen, 0, 1),
			new Token(TokenType.Identifier, 1, 3),
			new Token(TokenType.ParenthesisClose, 4, 1),
			new Token(TokenType.Identifier, 5, 5),
			new Token(TokenType.Plus, 11, 1),
			new Token(TokenType.ParenthesisOpen, 13, 1),
			new Token(TokenType.Identifier, 14, 5),
			new Token(TokenType.ParenthesisClose, 19, 1),
			new Token(TokenType.Number, 20, 7),
			new Token(TokenType.EndOfFile, 27, 0)
		);

		AssertTokens("class.Method()",
			new Token(TokenType.Identifier, 0, 5),
			new Token(TokenType.MemberAccess, 5, 1),
			new Token(TokenType.Identifier, 6, 6),
			new Token(TokenType.ParenthesisOpen, 12, 1),
			new Token(TokenType.ParenthesisClose, 13, 1),
			new Token(TokenType.EndOfFile, 14, 0)
		);

		AssertTokens("class.Method(1, 2)",
			new Token(TokenType.Identifier, 0, 5),
			new Token(TokenType.MemberAccess, 5, 1),
			new Token(TokenType.Identifier, 6, 6),
			new Token(TokenType.ParenthesisOpen, 12, 1),
			new Token(TokenType.Number, 13, 1),
			new Token(TokenType.Comma, 14, 1),
			new Token(TokenType.Number, 16, 1),
			new Token(TokenType.ParenthesisClose, 17, 1),
			new Token(TokenType.EndOfFile, 18, 0)
		);
	}

	[Test]
	public void Lexer_Strings()
	{
		AssertTokens("\"Hello World\"",
			new Token(TokenType.String, 0, 13),
			new Token(TokenType.EndOfFile, 13, 0)
		);

		AssertTokens("\"Hello \\\"World\\\"\"",
			new Token(TokenType.String, 0, 17),
			new Token(TokenType.EndOfFile, 17, 0)
		);

		AssertTokens("\"Hello \\\"World\\\"\" + \"Hello \\\"World\\\"\"",
			new Token(TokenType.String, 0, 17),
			new Token(TokenType.Plus, 18, 1),
			new Token(TokenType.String, 20, 17),
			new Token(TokenType.EndOfFile, 37, 0)
		);

		AssertTokens("'c' + '4' + 34.ToString()",
			new Token(TokenType.Character, 0, 3),
			new Token(TokenType.Plus, 4, 1),
			new Token(TokenType.Character, 6, 3),
			new Token(TokenType.Plus, 10, 1),
			new Token(TokenType.Number, 12, 2),
			new Token(TokenType.MemberAccess, 14, 1),
			new Token(TokenType.Identifier, 15, 8),
			new Token(TokenType.ParenthesisOpen, 23, 1),
			new Token(TokenType.ParenthesisClose, 24, 1),
			new Token(TokenType.EndOfFile, 25, 0)
		);

		AssertTokens("'c' + '4' + 34.ToString() + \"Hello \\\"World\\\"\"",
			new Token(TokenType.Character, 0, 3),
			new Token(TokenType.Plus, 4, 1),
			new Token(TokenType.Character, 6, 3),
			new Token(TokenType.Plus, 10, 1),
			new Token(TokenType.Number, 12, 2),
			new Token(TokenType.MemberAccess, 14, 1),
			new Token(TokenType.Identifier, 15, 8),
			new Token(TokenType.ParenthesisOpen, 23, 1),
			new Token(TokenType.ParenthesisClose, 24, 1),
			new Token(TokenType.Plus, 26, 1),
			new Token(TokenType.String, 28, 17),
			new Token(TokenType.EndOfFile, 45, 0)
		);

		Assert.Throws<SyntacticException>(() => 
			AssertTokens("'\\'", new Token())
		);

		

		AssertTokens("'\\\\'",
			new Token(TokenType.Character, 0, 4),
			new Token(TokenType.EndOfFile, 4, 0)
		);

		AssertTokens("\"This is a string with a \\\" in it\"!",
			new Token(TokenType.String, 0, 34),
			new Token(TokenType.LogicalNegation, 34, 1),
			new Token(TokenType.EndOfFile, 35, 0)
		);

		AssertTokens("\"String 1\" + \"String 2\" + \"String 3\"",
			new Token(TokenType.String, 0, 10),
			new Token(TokenType.Plus, 11, 1),
			new Token(TokenType.String, 13, 10),
			new Token(TokenType.Plus, 24, 1),
			new Token(TokenType.String, 26, 10),
			new Token(TokenType.EndOfFile, 36, 0)
		);

		Assert.Throws<SyntacticException>(() => 
			AssertTokens("\"String 1\" + \"String 2 + \"String 3\"",
				new Token(TokenType.String, 0, 10),
				new Token(TokenType.Plus, 11, 1),
				new Token(TokenType.String, 13, 13),
				new Token(TokenType.Identifier, 26, 6),
				new Token(TokenType.Number, 33, 1),
				new Token()
			)
		);

	}

	[Test]
	public void Lexer_LiteralKeywords()
	{
		AssertTokens("true",
			new Token(TokenType.True, 0, 4),
			new Token(TokenType.EndOfFile, 4, 0)
		);

		AssertTokens("false",
			new Token(TokenType.False, 0, 5),
			new Token(TokenType.EndOfFile, 5, 0)
		);

		AssertTokens("null",
			new Token(TokenType.Null, 0, 4),
			new Token(TokenType.EndOfFile, 4, 0)
		);

		AssertTokens("base",
			new Token(TokenType.Base, 0, 4),
			new Token(TokenType.EndOfFile, 4, 0)
		);

		AssertTokens("this",
			new Token(TokenType.This, 0, 4),
			new Token(TokenType.EndOfFile, 4, 0)
		);

		AssertTokens("typeof",
			new Token(TokenType.Typeof, 0, 6),
			new Token(TokenType.EndOfFile, 6, 0)
		);

		AssertTokens("Hello(typeof(int))",
			new Token(TokenType.Identifier, 0, 5),
			new Token(TokenType.ParenthesisOpen, 5, 1),
			new Token(TokenType.Typeof, 6, 6),
			new Token(TokenType.ParenthesisOpen, 12, 1),
			new Token(TokenType.Identifier, 13, 3),
			new Token(TokenType.ParenthesisClose, 16, 1),
			new Token(TokenType.ParenthesisClose, 17, 1),
			new Token(TokenType.EndOfFile, 18, 0)
		);
	}
}