using System;
using System.Collections.Immutable;
using System.Runtime.InteropServices;

namespace Rythin.src   
{
    class lexer
    {
        private readonly string input;
        private int position;
        private char current_input;
        private int line;
       

        public lexer(string input)
        {
            this.input = input;
            position = 0;
            current_input = input.Length > 0 ? input[position] : '\0';
            line = 1;
        }

        public Tokens nextToken()
        {
            SkipWhitespace();
            if (current_input == '\0')
            {
                return new Tokens(Types.EOF, "", line);
            }
            else if (current_input == ';')
            {
                return Comment(); // Ignorar comentários
            }
            else if (char.IsLetter(current_input))
            {
                return Identifier();
            }
            else if (current_input == ':')
            {
                advance();
                return new Tokens(Types.COLON, ":", line);
            }
            else if (current_input == '"')
            {
                return StringLiteral();
            }
            else if (current_input == '=')
            {
                advance();
                if (current_input == '=')
                {
                    advance();
                    return new Tokens(Types.EQUALSCOND, "==", line);
                }
                else
                {
                    return new Tokens(Types.EQUALS, "=", line);
                }
            }
            else if (current_input == ';')
            {
                advance();
                if (current_input == ';')
                {
                    advance();
                    return new Tokens(Types.COMMENT, ";;", line);
                }
                else
                {
                    return new Tokens(Types.SEMICOLON, ";", line);
                }
            }
            else if (current_input == '-')
            {
                advance();
                if (current_input == '>')
                {
                    advance();
                    return new Tokens(Types.ARROW_SET, "->", line);
                }
                else
                {
                    return new Tokens(Types.IDENTIFIER, "-", line);
                }
            }
            else if (current_input == '{')
            {
                advance();
                return new Tokens(Types.CURLY_OPEN, "{", line);
            }
            else if (current_input == '}')
            {
                advance();
                return new Tokens(Types.CURLY_CLOSE, "}", line);
            }
            else if (current_input == '(')
            {
                advance();
                return new Tokens(Types.OPEN_PARENTESES, "(", line);
            }
            else if (current_input == ')')
            {
                advance();
                return new Tokens(Types.CLOSE_PARENTESES, ")", line);
            }
            else if (current_input == '.')
            {
                advance();
                return new Tokens(Types.SEPARATOR, ".", line);
            }
            else if (char.IsDigit(current_input))
            {
                return Number();
            }
            else
            {
                Console.WriteLine($"Unexpected character: '{current_input}' at line {line}");
                throw new Exception($"Unexpected character: '{current_input}' at line {line}");
            }
        }

        private Tokens StringLiteral()
        {
            advance(); // Skip opening quote
            var start = position;
            var ivalue = string.Empty;
            while (current_input != '"' && current_input != '\0')
            {
                if (current_input == '$' && peekNextChar() == '{')
                {
                    // Process interpolation
                    advance(); // Skip $
                    advance(); // Skip {
                    var interpolatedValue = ProcessInterpolation();
                    ivalue += interpolatedValue;
                }
                else
                {
                    ivalue += current_input;
                    advance();
                }
            }
            if (current_input == '\0')
            {
                throw new Exception($"Unterminated string literal at line {line}");
            }
            advance(); // Skip closing quote
            var value = input.Substring(start, position - start - 1); // Exclude quotes
            return new Tokens(Types.STRING_LITERAL, value, line);
        }

        private char peekNextChar()
        {
            int nextPosition = position + 1;
            return nextPosition < input.Length ? input[nextPosition] : '\0';
        }

        private string ProcessInterpolation()
        {
            var start = position;
            while (current_input != '}' && current_input != '\0')
            {
                advance();
            }
            if (current_input == '}')
            {
                var interpolatedValue = input.Substring(start, position - start);
                advance(); // Skip }
                return interpolatedValue;
            }
            else
            {
                throw new Exception($"Unterminated interpolation at line {line}");
            }
        }

        private Tokens Identifier()
        {
            var start = position;
            while (char.IsLetterOrDigit(current_input) || current_input == '_' || current_input == '-')
            {
                advance();
            }

            var value = input.Substring(start, position - start);

            // Keywords and types
            return value switch
            {
                "use" => new Tokens(Types.KEYWORD_USE, value, line),
                "def" => new Tokens(Types.KEYWORD_DEF, value, line),
                "class" => new Tokens(Types.KEYWORD_CLASS, value, line),
                "print" => new Tokens(Types.PRINT, value, line),
                "interface" => new Tokens(Types.KEYWORD_INTERFACE, value, line),
                "func" => new Tokens(Types.KEYWORD_FUNC, value, line),
                "public-static" => new Tokens(Types.KEYWORD_PUBLIC_STATIC, value, line),
                "public-nonstatic" => new Tokens(Types.KEYWORD_PUBLIC_NONSTATIC, value, line),
                "static" => new Tokens(Types.KEYWORD_STATIC, value, line),
                "nonstatic" => new Tokens(Types.KEYWORD_NONSTATIC, value, line),
                "public" => new Tokens(Types.KEYWORD_PUBLIC, value, line),
                "private" => new Tokens(Types.KEYWORD_PRIVATE, value, line),
                "return" => new Tokens(Types.KEYWORD_RETURN, value, line),
                "null" => new Tokens(Types.KEYWORD_NULL, value, line),
                "str" => new Tokens(Types.TYPE_STR, value, line),
                "dbl" => new Tokens(Types.TYPE_DOUBLE, value, line),
                "int" => new Tokens(Types.TYPE_INT, value, line),
                "lng" => new Tokens(Types.TYPE_LONG, value, line),
                "char" => new Tokens(Types.TYPE_CHARSEQ, value, line),
                _ => new Tokens(Types.IDENTIFIER, value, line),
            };
        }

        public void advance()
        {
            if (current_input == '\n')
                line++;
            position++;
            current_input = position < input.Length ? input[position] : '\0';
        }

        public void SkipWhitespace()
        {
            while (current_input != '\0' && char.IsWhiteSpace(current_input))
            {
                advance();
            }
        }

        public string digits()
        {
            var res = string.Empty;
            while (current_input != '\0' && char.IsDigit(current_input))
            {
                res += current_input;
                advance();
            }
            return res;
        }

        public Tokens Number()
        {
            var start = position;
            var numbstr = digits();
            if (string.IsNullOrEmpty(numbstr))
            {
                throw new Exception($"Invalid number format at line {line}");
            }
            return new Tokens(Types.NUMBER, numbstr, line);
        }

        private Tokens Comment()
        {
            advance(); // Skip the first ';'
            advance(); // Skip the second ';'
            while (current_input != '\0' && current_input != '\n')
            {
                advance();
            }
            // Continue to next token
            return nextToken();
        }
    }
}
