using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //A bitwise gate takes as input WireSets containing n wires, and computes a bitwise function - z_i=f(x_i)
    class BitwiseDemux : Gate
    {
        public int Size { get; private set; }
        public WireSet Output1 { get; private set; }
        public WireSet Output2 { get; private set; }
        public WireSet Input { get; private set; }
        public Wire Control { get; private set; }

        private Demux[] Demux_gates;

        public BitwiseDemux(int iSize)
        {
            Size = iSize;
            Control = new Wire();
            Input = new WireSet(Size);
            Output1 = new WireSet(Size);
            Output2 = new WireSet(Size);

            Demux_gates = new Demux[Size];
            int i = 0;
            while (i < Size)
            {
                Demux_gates[i] = new Demux();
                Demux_gates[i].ConnectControl(Control);
                Demux_gates[i].ConnectInput(Input[i]);
                Output1[i].ConnectInput(Demux_gates[i].Output1);
                Output2[i].ConnectInput(Demux_gates[i].Output2);
                i++;
            }
        }

        public void ConnectControl(Wire wControl)
        {
            Control.ConnectInput(wControl);
        }
        public void ConnectInput(WireSet wsInput)
        {
            Input.ConnectInput(wsInput);
        }

        public override string ToString()
        {
            return "Demux" + Input + "," + "c" + Control.Value + " ->" + Output1 + "," + Output2;
        }

        public override bool TestGate()
        {
            Control.Value = 0;
            for (int i = 0; i < Size; i++)
            {
                Input[i].Value = 0;
                if ( Output1[i].Value != 0 && Output2[i].Value != 0)
                    return false;
                Input[i].Value = 1;
                if ( Output1[i].Value != 1 && Output2[i].Value != 0)
                    return false;
            }
            Control.Value = 1;
            for (int i = 0; i < Size; i++)
            {
                Input[i].Value = 0;
                if ( Output1[i].Value != 0 && Output2[i].Value != 0)
                    return false;
                Input[i].Value = 1;
                if ( Output1[i].Value != 0 && Output2[i].Value != 1)
                    return false;
            }
            return true;
        }
    }
}
