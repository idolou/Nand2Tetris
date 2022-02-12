using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //A two input bitwise gate takes as input two WireSets containing n wires, and computes a bitwise function - z_i=f(x_i,y_i)
    class BitwiseOrGate : BitwiseTwoInputGate
    {
        private OrGate[] Or_gates;

        public BitwiseOrGate(int iSize)
            : base(iSize)
        {
            Or_gates = new OrGate[iSize];
            int i = 0;
            while (i < Size)
            {
                Or_gates[i] = new OrGate();
                Or_gates[i].ConnectInput1(Input1[i]);
                Or_gates[i].ConnectInput2(Input2[i]);
                Output[i].ConnectInput(Or_gates[i].Output);
                i++;
            }
            
        }

        //an implementation of the ToString method is called, e.g. when we use Console.WriteLine(or)
        //this is very helpful during debugging
        public override string ToString()
        {
            return "Or " + Input1 + ", " + Input2 + " -> " + Output;
        }

        public override bool TestGate()
        {
            for (int i = 0; i < Size; i++)
            {
                Or_gates[i].Input1.Value = 0;
                Or_gates[i].Input2.Value = 0;
                if(Output[i].Value != 0)
                    return false;
                Or_gates[i].Input1.Value = 0;
                Or_gates[i].Input2.Value = 1;
                if(Output[i].Value != 1)
                    return false;
                Or_gates[i].Input1.Value = 1;
                Or_gates[i].Input2.Value = 0;
                if(Output[i].Value != 1)
                    return false;
                Or_gates[i].Input1.Value = 1;
                Or_gates[i].Input2.Value = 1;
                if(Output[i].Value != 1)
                    return false;
            }
            return true;
        }
    }
}
