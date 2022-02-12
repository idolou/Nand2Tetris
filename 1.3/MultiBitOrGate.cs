using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //Multibit gates take as input k bits, and compute a function over all bits - z=f(x_0,x_1,...,x_k)

    class MultiBitOrGate : MultiBitGate
    {
        private OrGate[] m_OrGates;

        public MultiBitOrGate(int iInputCount)
            : base(iInputCount)
        {
            m_OrGates = new OrGate[iInputCount-1];
            OrGate start_g = new OrGate();
            m_OrGates[0] = start_g;
            start_g.ConnectInput1(m_wsInput[0]);
            start_g.ConnectInput2(m_wsInput[1]);

            int i = 1;
            while (i < (iInputCount - 1))
            {
                m_OrGates[i] = new OrGate();
                m_OrGates[i].ConnectInput1(m_OrGates[i-1].Output);
                m_OrGates[i].ConnectInput2(m_wsInput[i+1]);
                i++;
            }
            
            Wire f_output = m_OrGates[iInputCount - 2].Output;
            Output = f_output;
        }
        
        public override string ToString()
        {
            return "MultibitOrGate" + m_wsInput  + " -> " + Output;
        }
        
        public override bool TestGate()
        {
            for (int i = 0; i < m_wsInput.Size; i++)
            {
                m_wsInput[i].Value = 0;
            }
            if (Output.Value != 0){return false;}
            
            for (int i = 0; i < m_wsInput.Size; i++)
            {
                m_wsInput[i].Value = 1;
            }
            if (Output.Value != 1){return false;}
            
            for (int i = 0; i < m_wsInput.Size; i++)
            {
                m_wsInput[i].Value = 0;
            }
            m_wsInput[0].Value = 1;
            if (Output.Value != 1){return false;}
            
            for (int i = 0; i < m_wsInput.Size; i++)
            {
                m_wsInput[i].Value = 1;
            }
            m_wsInput[0].Value = 0;
            if (Output.Value != 1){return false;}
                
        return true;    
        }
        
    }
}
