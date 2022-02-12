using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //A two input bitwise gate takes as input two WireSets containing n wires, and computes a bitwise function - z_i=f(x_i,y_i)
    class BitwiseAndGate : BitwiseTwoInputGate
    {
        private AndGate[] And_gates;

        public BitwiseAndGate(int iSize)
            : base(iSize)
        {
            And_gates = new AndGate[iSize];
            int i = 0;
            while (i < Size)
            {
                And_gates[i] = new AndGate();
                And_gates[i].ConnectInput1(Input1[i]);
                And_gates[i].ConnectInput2(Input2[i]);
                Output[i].ConnectInput(And_gates[i].Output);
                i++;
            }
            
        }

        //an implementation of the ToString method is called, e.g. when we use Console.WriteLine(and)
        //this is very helpful during debugging
        public override string ToString()
        {
            return "And " + Input1 + ", " + Input2 + " -> " + Output;
        }

        public override bool TestGate()
        {
            for (int i = 0; i < Size; i++)
            {
                And_gates[i].Input1.Value = 0;
                And_gates[i].Input2.Value = 0;
                if (Output[i].Value != 0)
                    return false;
                And_gates[i].Input1.Value = 0;
                And_gates[i].Input2.Value = 1;
                if (Output[i].Value != 0)
                    return false;
                And_gates[i].Input1.Value = 1;
                And_gates[i].Input2.Value = 0;
                if (Output[i].Value != 0)
                    return false;
                And_gates[i].Input1.Value = 1;
                And_gates[i].Input2.Value = 1;
                if (Output[i].Value != 1)
                    return false;
            }
            return true;
           
        }
    }
}
