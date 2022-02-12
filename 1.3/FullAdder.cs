using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This class implements a FullAdder, taking as input 3 bits - 2 numbers and a carry, and computing the result in the output, and the carry out.
    class FullAdder : TwoInputGate
    {
        public Wire CarryInput { get; private set; }
        public Wire CarryOutput { get; private set; }

        private HalfAdder half_add1;
        private HalfAdder half_add2;
        private OrGate m_gOr;

         
        



        public FullAdder()
        {
            CarryInput = new Wire();
            CarryOutput = new Wire();
            half_add1 = new HalfAdder();
            half_add2 = new HalfAdder();
            m_gOr = new OrGate();

            // CarryInput.Value = 0;
            // CarryOutput.Value = 0;
            
            half_add2.ConnectInput1(half_add1.Output);
            half_add2.ConnectInput2(CarryInput);
            m_gOr.ConnectInput1(half_add1.CarryOutput);
            m_gOr.ConnectInput2(half_add2.CarryOutput);
            CarryOutput = m_gOr.Output;

            Input1 = half_add1.Input1;
            Input2 = half_add1.Input2;
            Output = half_add2.Output;




        }


        public override string ToString()
        {
            return Input1.Value + "+" + Input2.Value + " (C" + CarryInput.Value + ") = " + Output.Value + " (C" + CarryOutput.Value + ")";
        }

        public override bool TestGate()
        {
            CarryInput.Value = 0;
            Input1.Value = 0;
            Input2.Value = 0;
            if (Output.Value != 0 && CarryOutput.Value != 0) return false;
            Input1.Value = 0;
            Input2.Value = 1;
            if (Output.Value != 1 && CarryOutput.Value != 0) return false;
            Input1.Value = 1;
            Input2.Value = 0;
            if (Output.Value != 1 && CarryOutput.Value != 0) return false;
            Input1.Value = 1;
            Input2.Value = 1;
            if (Output.Value != 0 && CarryOutput.Value != 1) return false;
            
            CarryInput.Value = 1;
            Input1.Value = 0;
            Input2.Value = 0;
            if (Output.Value != 1 && CarryOutput.Value != 0) return false;
            Input1.Value = 0;
            Input2.Value = 1;
            if (Output.Value != 0 && CarryOutput.Value != 1) return false;
            Input1.Value = 1;
            Input2.Value = 0;
            if (Output.Value != 0 && CarryOutput.Value != 1) return false;
            Input1.Value = 1;
            Input2.Value = 1;
            if (Output.Value != 1 && CarryOutput.Value != 1) return false;
            return true;
            //throw new NotImplementedException();
        }
    }
}
