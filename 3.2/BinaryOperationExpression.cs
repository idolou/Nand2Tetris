using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public class BinaryOperationExpression : Expression
    {
        public string Operator { get;  set; }
        public Expression Operand1 { get;  set; }
        public Expression Operand2 { get;  set; }

        public override string ToString()
        {
            return "(" + Operand1 + " " + Operator + " " + Operand2 + ")";
        }

        public override void Parse(TokensStack sTokens)
        {
            // will get the (
            Token t = sTokens.Pop();
            if(!(t is Parentheses) || ((Parentheses) t).Name != '(')
                throw new SyntaxErrorException("Expected open ( paranthesis , received " + t, t);
            
            //will get operand 1
            Expression exp = Expression.Create(sTokens);
            exp.Parse(sTokens);
            Operand1 = exp;
            
            
            // check if there is 
            t = sTokens.Pop();
            if(!(t is Operator))
                throw new SyntaxErrorException("Expected '=' , received " + t, t);
            Operator = t.ToString();
            
            
            // get the 2'd operand
            Expression exp1 = Expression.Create(sTokens);
            exp1.Parse(sTokens);
            Operand2 = exp1;
            
            
            // check for )
            t = sTokens.Pop();
            if(!(t is Parentheses) || ((Parentheses)t).Name != ')')
                throw new SyntaxErrorException("Expected close ) parenthesis , received " + t, t);


            // throw new NotImplementedException();

        }
    }
}
