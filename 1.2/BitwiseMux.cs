using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //A bitwise gate takes as input WireSets containing n wires, and computes a bitwise function - z_i=f(x_i)
    class BitwiseMux : BitwiseTwoInputGate
    {
        public Wire ControlInput { get; private set; }

        private MuxGate[] Mux_gates;

        public BitwiseMux(int iSize)
            : base(iSize)
        {
            ControlInput = new Wire();
            Mux_gates = new MuxGate[iSize];
            int i = 0;
            while (i < iSize)
            {
                Mux_gates[i] = new MuxGate();
                Mux_gates[i].ConnectInput1(Input1[i]);
                Mux_gates[i].ConnectInput2(Input2[i]);
                Output[i].ConnectInput(Mux_gates[i].Output);
                Mux_gates[i].ConnectControl(ControlInput);
                i++;
            }
        }

        public void ConnectControl(Wire wControl)
        {
            ControlInput.ConnectInput(wControl);
        }



        public override string ToString()
        {
            return "Mux " + Input1 + "," + Input2 + ",C" + ControlInput.Value + " -> " + Output;
        }




        public override bool TestGate()
        {
            ControlInput.Value = 0;
            for (int i = 0; i < Size; i++)
            {
                Mux_gates[i].Input1.Value = 0;
                Mux_gates[i].Input2.Value = 0;
                if(Output[i].Value != 0)
                    return false;
                Mux_gates[i].Input1.Value = 1;
                Mux_gates[i].Input2.Value = 0;
                if(Output[i].Value != 1)
                    return false;
                Mux_gates[i].Input1.Value = 0;
                Mux_gates[i].Input2.Value = 1;
                if(Output[i].Value != 0)
                    return false;
                Mux_gates[i].Input1.Value = 1;
                Mux_gates[i].Input2.Value = 1;
                if(Output[i].Value != 1)
                    return false;
            }

            ControlInput.Value = 1;
            for (int i = 0; i < Size; i++)
            {
                Mux_gates[i].Input1.Value = 0;
                Mux_gates[i].Input2.Value = 0;
                if(Output[i].Value != 0)
                    return false;
                Mux_gates[i].Input1.Value = 1;
                Mux_gates[i].Input2.Value = 0;
                if(Output[i].Value != 0)
                    return false;
                Mux_gates[i].Input1.Value = 0;
                Mux_gates[i].Input2.Value = 1;
                if(Output[i].Value != 1)
                    return false;
                Mux_gates[i].Input1.Value = 1;
                Mux_gates[i].Input2.Value = 1;
                if(Output[i].Value != 1)
                    return false;
            }
            return true;
        }
    }
}
