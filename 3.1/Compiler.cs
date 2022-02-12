using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SimpleCompiler
{
    class Compiler
    {
        
        public Compiler()
        {
            
        }
        
        
        //reads a file into a list of strings, each string represents one line of code
        public List<string> ReadFile(string sFileName)
        {
            StreamReader sr = new StreamReader(sFileName);
            List<string> lCodeLines = new List<string>();
            while (!sr.EndOfStream)
            {
                lCodeLines.Add(sr.ReadLine());
            }
            sr.Close();
            return lCodeLines;
        }



        //Computes the next token in the string s, from the begining of s until a delimiter has been reached. 
        //Returns the string without the token.
        private string Next(string s, char[] aDelimiters, out string sToken, out int cChars)
        {
            cChars = 1;
            sToken = s[0] + "";
            if (aDelimiters.Contains(s[0]))
                return s.Substring(1);
            int i = 0;
            for (i = 1; i < s.Length; i++)
            {
                if (aDelimiters.Contains(s[i]))
                    return s.Substring(i);
                else
                    sToken += s[i];
                cChars++;
            }
            return null;
        }
        

        //Splits a string into a list of tokens, separated by delimiters
        private List<string> Split(string s, char[] aDelimiters)
        {
            List<string> lTokens = new List<string>();
            while (s.Length > 0)
            {
                string sToken = "";
                int i = 0;
                for (i = 0; i < s.Length; i++)
                {
                    if (aDelimiters.Contains(s[i]))
                    {
                        if (sToken.Length > 0)
                            lTokens.Add(sToken);
                        lTokens.Add(s[i] + "");
                        break;
                    }
                    else
                        sToken += s[i];
                }
                if (i == s.Length)
                {
                    lTokens.Add(sToken);
                    s = "";
                }
                else
                    s = s.Substring(i + 1);
            }
            return lTokens;
        }

        //This is the main method for the Tokenizing assignment. 
        //Takes a list of code lines, and returns a list of tokens.
        //For each token you must identify its type, and instantiate the correct subclass accordingly.
        //You need to identify the token position in the file (line, index within the line).
        //You also need to identify errors, in this assignement - illegal identifier names.
        public List<Token> Tokenize(List<string> lCodeLines)
        {
            List<Token> lTokens = new List<Token>();
            

            char[] deli = CreateDeli(Token.Operators, Token.Parentheses, Token.Separators);
            
            int LineCounter = 0;
            
            List<string> separated = new List<string>();
            int TokenNumber;
            List<string> CodeLines = cleanComments(lCodeLines);
            
            foreach (string Line in CodeLines)
            {
                int tokenIndex = 0;
                separated = Split(Line, deli);

                for (int i = 0; i < separated.Count; i++)
                {
                    if (separated[i] == " " || separated[i] == "\t")
                    {
                        tokenIndex++;
                        continue;
                    }

                    if (Token.Statements.Contains(separated[i]))
                    {
                        lTokens.Add(new Statement(separated[i], LineCounter, tokenIndex));
                        tokenIndex += separated[i].Length;
                    }

                    else if (Token.VarTypes.Contains(separated[i]))
                    {
                        lTokens.Add(new VarType(separated[i], LineCounter, tokenIndex));
                        tokenIndex += separated[i].Length;
                    }
                    
                    else if (Token.Constants.Contains(separated[i]))
                    {
                        lTokens.Add(new Constant(separated[i], LineCounter, tokenIndex));
                        tokenIndex += separated[i].Length;
                    }
                    
                    else if (Token.Operators.Contains(separated[i].ToCharArray()[0]))
                    {
                        lTokens.Add(new Operator(separated[i].ToCharArray()[0], LineCounter, tokenIndex));
                        tokenIndex += separated[i].Length;
                    }
                    
                    else if (Int32.TryParse(separated[i], out TokenNumber) == true)
                    {
                        lTokens.Add(new Number(separated[i], LineCounter, tokenIndex));
                        tokenIndex += separated[i].Length;
                    }
                    
                    else if (Token.Parentheses.Contains(separated[i].ToCharArray()[0]))
                    {
                        lTokens.Add(new Parentheses(separated[i].ToCharArray()[0], LineCounter, tokenIndex));
                        tokenIndex += separated[i].Length;
                    }
                    
                    else if (Token.Separators.Contains(separated[i].ToCharArray()[0]))
                    {
                        lTokens.Add(new Separator(separated[i].ToCharArray()[0], LineCounter, tokenIndex));
                        tokenIndex += separated[i].Length;
                        
                    }


                    else if(Char.IsLetter(separated[i].ToCharArray()[0]))
                    {
                        lTokens.Add(new Identifier(separated[i], LineCounter, tokenIndex));
                            tokenIndex += separated[i].Length;
                    }

                    else
                    {
                        throw new SyntaxErrorException("invalid token", new Statement(separated[i], LineCounter, tokenIndex));
                    }


                }

                LineCounter++;

            }
            
            
            
            
            
            return lTokens;
        }

        private List<string> cleanComments(List<string> lines)
        {
            List<string> result = new List<string>();
            for (int i = 0; i < lines.Count; i++)
            {
                string Line = lines[i];
                if (Line.Contains("//"))
                {
                    if (Line.StartsWith("//"))
                    {
                        result.Add("");
                        continue;
                    }
                    Line = Line.Substring(0, Line.IndexOf("//"));
                    result.Add(Line);
                }
                else{ result.Add(Line);}
            }
            return result;
        }


        private char[] CreateDeli(char[] Operators, char[] Parentheses, char[] Separators)
        {
            string result = "";
            string a = new string(Operators);
            string b = new string(Separators);
            string c = new string(Parentheses);

            result = result + a + b + c + " " + "\t";
            return result.ToCharArray();
        }
        

    }
}

