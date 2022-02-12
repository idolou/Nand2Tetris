using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //A demux has 2 outputs. There is a single input and a control bit, selecting whether the input should be directed to the first or second output.
    class Demux : Gate
    {
        public Wire Output1 { get; private set; }
        public Wire Output2 { get; private set; }
        public Wire Input { get; private set; }
        public Wire Control { get; private set; }

        private NotGate m_gNot;
        private AndGate m_gAnd1;
        private AndGate m_gAnd2;
        
        
        public Demux()
        {
            Output1 = new Wire();
            Output2 = new Wire();
            Input = new Wire();
            Control = new Wire();
            m_gNot = new NotGate();
            m_gAnd1 = new AndGate();
            m_gAnd2 = new AndGate();
            
            m_gAnd1.ConnectInput1(m_gNot.Output);
            m_gAnd1.ConnectInput2(Input);
            m_gNot.ConnectInput(Control);
            m_gAnd2.ConnectInput1(Control);
            m_gAnd2.ConnectInput2(Input);

            Output1 = m_gAnd1.Output;
            Output2 = m_gAnd2.Output;

        }

        public void ConnectControl(Wire wControl)
        {
            Control.ConnectInput(wControl);
        }
        public void ConnectInput(Wire wInput)
        {
            Input.ConnectInput(wInput);
        }

        public override string ToString()
        {
            return "Demux " + Input.Value + "," + "C" + Control.Value + " -> " + Output1.Value + ","  + Output2.Value;
        }

        public override bool TestGate()
        {
            Control.Value = 0;
            Input.Value = 0;
            if (Output1.Value != 0 && Output2.Value != 0)
                return false;
            Input.Value = 1;
            if (Output1.Value != 1 && Output2.Value != 0)
                return false;
            Control.Value = 1;
            if (Output1.Value != 0 && Output2.Value != 0)
                return false;
            Input.Value = 1;
            if (Output1.Value != 0 && Output2.Value != 1)
                return false;
            return true;
        }
    }
}
