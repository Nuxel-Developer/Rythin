using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Rythin.src
{
    internal class Parser
    {
        public abstract class ASTNode { }

        public class Number : ASTNode
        {
            public Tokens Token { get; }

            public Number(Tokens token)
            {
                Token = token;
            }
        }

        public class BinaryOperation : ASTNode
        {
            public ASTNode Left { get; }
            public Tokens Operator { get; }
            public ASTNode Right { get; }

            public BinaryOperation(ASTNode left, Tokens operatorToken, ASTNode right)
            {
                Left = left;
                Operator = operatorToken;
                Right = right;
            }
        }

        public class ExpressionNode : ASTNode
        {
            public ASTNode body { get; }

            public ExpressionNode(ASTNode body)
            {
                this.body = body;
            }

        }

        public class FunctionDefinitionNode : ASTNode
        {
            public string name { get; }
            public List<ParameterNode> parameters { get; }
            ASTNode body { get; }

            public FunctionDefinitionNode(string name, List<ParameterNode> par, ASTNode body)
            {
                this.name = name;
                this.body = body;
                parameters = par;
            }
        }
        public class ParameterNode : ASTNode
        {
            public string Name { get; }
            public string Type { get; }

            public ParameterNode(string name, string type)
            {
                Name = name;
                Type = type;
            }
        }
    }
}
