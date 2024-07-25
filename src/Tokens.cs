namespace Rythin.src
{
    internal class Tokens
    {
        public Types types { get; }
        public string value { get; }
        public int Lines { get; }

        public Tokens(Types types, string value, int line) {
            this.types = types;
            this.value = value;
            Lines = line;
        }

        public override string ToString()
        {
            return $"Token [{types}, {value} at line {Lines}]";
        }

        public Types getType()
        {
            return types;
        }

        public string getValue()
        {
            return value;
        }

        public int getLines()
        {
            return Lines;
        }
    }
}
