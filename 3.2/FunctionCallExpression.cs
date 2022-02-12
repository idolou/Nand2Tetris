using System;
using System.Collections.Generic;

namespace SimpleCompiler
{
    public class FunctionCallExpression : Expression
    {
        public string FunctionName { get; private set; }
        public List<Expression> Args { get; private set; }


        public FunctionCallExpression()
        {
            Args = new List<Expression>();
        }
        
        
        
        public override void Parse(TokensStack sTokens)
        {
            Token funcName = sTokens.Pop();
            this.FunctionName = funcName.ToString();
            if (!(funcName is Identifier) )
                throw new SyntaxErrorException("Expected function name ID received: " + funcName, funcName);

            //After the name there should be opening paranthesis for the arguments
            Token t = sTokens.Pop(); //(
            if(!(t is Parentheses) || ((Parentheses) t).Name != '(')
                throw new SyntaxErrorException("Expected open paranthesis ( , received " + t, t);
            
            
            //Now we extract the arguments from the stack until we see a closing parathesis
            while (sTokens.Count > 0  && !(sTokens.Peek().ToString().Equals(")"))) //)
            {
                
                //will get all the arguments (can be number, var, function etc)
                Expression arg_exp = Expression.Create(sTokens);
                arg_exp.Parse(sTokens);
                Args.Add(arg_exp);
                
                
                //If there is a comma, then there is another argument name
                if (sTokens.Count > 0 && sTokens.Peek() is Separator)//,
                    sTokens.Pop(); 

            }
            
            t = sTokens.Pop();//)
            if(!(t is Parentheses) || ((Parentheses) t).Name != ')')
                throw new SyntaxErrorException("Expected open paranthesis , received " + t, t);
            
        }

        public override string ToString()
        {
            string sFunction = FunctionName + "(";
            for (int i = 0; i < Args.Count - 1; i++)
                sFunction += Args[i] + ",";
            if (Args.Count > 0)
                sFunction += Args[Args.Count - 1];
            sFunction += ")";
            return sFunction;
        }
    }
}