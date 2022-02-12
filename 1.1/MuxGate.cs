using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Components
{
    //A mux has 2 inputs. There is a single output and a control bit, selecting which of the 2 inpust should be directed to the output.
    class MuxGate : TwoInputGate
    {
        public Wire ControlInput { get; private set; }
        private NotGate m_gNot;
        private AndGate m_gAnd1;
        private AndGate m_gAnd2;
        private OrGate m_gOr;

        public MuxGate()
        {
            ControlInput = new Wire();
            m_gNot = new NotGate();
            m_gAnd1 = new AndGate();
            m_gAnd2 = new AndGate();
            m_gOr =new OrGate();
            
            m_gAnd1.ConnectInput2(m_gNot.Output);
            m_gOr.ConnectInput1(m_gAnd1.Output);
            m_gOr.ConnectInput2(m_gAnd2.Output);
            m_gAnd2.ConnectInput1(ControlInput);
            m_gNot.ConnectInput(ControlInput);

            Input1 = m_gAnd1.Input1;
            Input2 = m_gAnd2.Input2;
            
            Output = m_gOr.Output;

        }

        public void ConnectControl(Wire wControl)
        {
            ControlInput.ConnectInput(wControl);
        }


        public override string ToString()
        {
            return "Mux " + Input1.Value + "," + Input2.Value + ",C" + ControlInput.Value + " -> " + Output.Value;
        }



        public override bool TestGate()
        {
            ControlInput.Value = 0;
            Input1.Value = 0;
            Input2.Value = 0;
            if (Output.Value != 0)
                return false;
            Input1.Value = 0;
            Input2.Value = 1;
            if (Output.Value != 0)
                return false;
            Input1.Value = 1;
            Input2.Value = 0;
            if (Output.Value != 1)
                return false;
            Input1.Value = 1;
            Input2.Value = 1;
            if (Output.Value != 1)
                return false;
            ControlInput.Value = 1;
            Input1.Value = 0;
            Input2.Value = 0;
            if (Output.Value != 0)
                return false;
            Input1.Value = 0;
            Input2.Value = 1;
            if (Output.Value != 1)
                return false;
            Input1.Value = 1;
            Input2.Value = 0;
            if (Output.Value != 0)
                return false;
            Input1.Value = 1;
            Input2.Value = 1;
            if (Output.Value != 1)
                return false;
            return true;
            
        }
    }
}
