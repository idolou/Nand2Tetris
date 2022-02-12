using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public class IfStatement : StatetmentBase
    {
        public Expression Term { get; private set; }
        public List<StatetmentBase> DoIfTrue { get; private set; }
        public List<StatetmentBase> DoIfFalse { get; private set; }

        //init object lists
        public IfStatement()
        {
            DoIfTrue = new List<StatetmentBase>();
            DoIfFalse = new List<StatetmentBase>();
        }
        
        
        public override void Parse(TokensStack sTokens)
        {
            
            //check for if state
            Token ifT = sTokens.Pop();
            if (!(ifT is Statement) || ((Statement)ifT).Name != "if")
                throw new SyntaxErrorException("Expected if, received: " + ifT, ifT);
            
            
            // '('
            ifT = sTokens.Pop();
            if (!(ifT is Parentheses) || ((Parentheses)ifT).Name != '(')
                throw new SyntaxErrorException("Expected '(', received: " + ifT, ifT);

            // look for the term
            Expression exp = Expression.Create(sTokens);
            exp.Parse(sTokens);
            Term = exp;
            
            ifT = sTokens.Pop();
            if (!(ifT is Parentheses) || ((Parentheses)ifT).Name != ')')
                throw new SyntaxErrorException("Expected ')', received: " + ifT, ifT);
            
            
            
            ifT = sTokens.Pop();
            if (!(ifT is Parentheses) || ((Parentheses)ifT).Name != '{')
                throw new SyntaxErrorException("Expected '{', received: " + ifT, ifT);
            
            
            // get al the if statements
            while (sTokens.Count > 0 && !(sTokens.Peek() is Parentheses)) //)
            {
                StatetmentBase sb = StatetmentBase.Create(sTokens.Peek());
                sb.Parse(sTokens);
                DoIfTrue.Add(sb);
            }
            
            ifT = sTokens.Pop();
            if (!(ifT is Parentheses) || ((Parentheses)ifT).Name != '}')
                throw new SyntaxErrorException("Expected '}', received: " + ifT, ifT);
            
            // check for else condition
            if (sTokens.Peek().ToString() != "else")
                return;
            
            ifT = sTokens.Pop();
            if (!(ifT is Statement) || ((Statement)ifT).Name != "else")
                throw new SyntaxErrorException("Expected else, received: " + ifT, ifT);
            
            
            ifT = sTokens.Pop();
            if (!(ifT is Parentheses) || ((Parentheses)ifT).Name != '{')
                throw new SyntaxErrorException("Expected '{', received: " + ifT, ifT);
            
            //get all the else statements
            while (sTokens.Count > 0 && !(sTokens.Peek() is Parentheses)) //)
            {
                StatetmentBase sb = StatetmentBase.Create(sTokens.Peek());
                sb.Parse(sTokens);
                DoIfFalse.Add(sb);
            }
            
            //"}"
            ifT = sTokens.Pop();
            if (!(ifT is Parentheses) || ((Parentheses)ifT).Name != '}')
                throw new SyntaxErrorException("Expected '{', received: " + ifT, ifT);



            // throw new NotImplementedException();
        }

        public override string ToString()
        {
            string sIf = "if(" + Term + "){\n";
            foreach (StatetmentBase s in DoIfTrue)
                sIf += "\t\t\t" + s + "\n";
            sIf += "\t\t}";
            if (DoIfFalse.Count > 0)
            {
                sIf += "else{";
                foreach (StatetmentBase s in DoIfFalse)
                    sIf += "\t\t\t" + s + "\n";
                sIf += "\t\t}";
            }
            return sIf;
        }

    }
}
