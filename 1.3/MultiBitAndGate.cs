using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //Multibit gates take as input k bits, and compute a function over all bits - z=f(x_0,x_1,...,x_k)
    class MultiBitAndGate : MultiBitGate
    {
        private AndGate[] m_Andgates;
        
        public MultiBitAndGate(int iInputCount)
            : base(iInputCount)
        {
            m_Andgates = new AndGate[iInputCount - 1]; //we have (number of inputs bits -1) and gates
            //we will connect the first 2 bits to the first And gate
            //then we will connect the rest of the inputs to the others and gates
            AndGate start_g = new AndGate();
            m_Andgates[0] = start_g;
            start_g.ConnectInput1(m_wsInput[0]);
            start_g.ConnectInput2(m_wsInput[1]);
            
 
            int i = 1;
            while (i < (iInputCount-1))
            {
                m_Andgates[i] = new AndGate();
                m_Andgates[i].ConnectInput1(m_Andgates[i - 1].Output);
                m_Andgates[i].ConnectInput2(m_wsInput[i + 1]);
                i++;
            }
            
            
            Wire f_output = m_Andgates[iInputCount - 2].Output;
            Output = f_output;
        }

        public override string ToString()
        {
            return "MultibitAndGate" + m_wsInput  + " -> " + Output;
        }

        public override bool TestGate()
        {
            int i = 0;
            while (i < m_wsInput.Size)
            {
                m_wsInput[i].Value = 0;
                i++;
            }  
            
            if(Output.Value != 0){
                    return false;
            }
            
            int j = 0;
            while (j < m_wsInput.Size)
            {
                m_wsInput[j].Value = 1;
                j++; }
            if(Output.Value != 1){
                    return false;
            }
            return true;
            
            
        }
    }
}
