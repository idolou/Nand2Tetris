using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This class implements an adder, receving as input two n bit numbers, and outputing the sum of the two numbers
    class MultiBitAdder : Gate
    {
        //Word size - number of bits in each input
        public int Size { get; private set; }

        public WireSet Input1 { get; private set; }
        public WireSet Input2 { get; private set; }
        public WireSet Output { get; private set; }
        //An overflow bit for the summation computation
        public Wire Overflow { get; private set; }
        private FullAdder[] fullAdders;
        public FullAdder starter { get; private set; }
        public FullAdder Finisher { get; private set; }




        public MultiBitAdder(int iSize)
        {
            Size = iSize;
            Input1 = new WireSet(Size);
            Input2 = new WireSet(Size);
            Output = new WireSet(Size);
            fullAdders = new FullAdder[Size];
            Overflow = new Wire();
            
            starter = new FullAdder();
            Finisher = new FullAdder();
            
            fullAdders[0] = starter;
            starter.CarryInput.Value = 0;
            starter.ConnectInput1(Input1[0]);
            starter.ConnectInput2(Input2[0]);
            Output[0].ConnectInput(starter.Output);
            for (int i = 1; i < iSize-1; i++)
            {
                fullAdders[i] = new FullAdder();
                fullAdders[i].ConnectInput1(Input1[i]);
                fullAdders[i].ConnectInput2(Input2[i]);
                fullAdders[i].CarryInput.ConnectInput(fullAdders[i-1].CarryOutput);
                Output[i].ConnectInput(fullAdders[i].Output);
            }
            
            // FullAdder finisher = new FullAdder();
            fullAdders[iSize-1] = Finisher;
            Finisher.ConnectInput1(Input1[iSize-1]);
            Finisher.ConnectInput2(Input2[iSize-1]);
            Finisher.CarryInput.ConnectInput(fullAdders[iSize - 2].CarryOutput);
            Output[iSize-1].ConnectInput(Finisher.Output);
            Overflow = new Wire();
            Overflow.ConnectInput(Finisher.CarryOutput);

        }

        public override string ToString()
        {
            return Input1 + "(" + Input1.GetValue() + ")" + " + " + Input2 + "(" + Input2.GetValue() + ")" + " = " + Output + "(" + Output.GetValue() + ")";
        }

        public void ConnectInput1(WireSet wInput)
        {
            Input1.ConnectInput(wInput);
        }
        public void ConnectInput2(WireSet wInput)
        {
            Input2.ConnectInput(wInput);
        }


        public override bool TestGate()
        {
            for (int i = 0; i < Size; i++)
            {
                Input1[i].Value = 0;
            }

            for (int i = 0; i < Size; i++)
            {
                Input2[i].Value = 0;
            }

            for (int i = 0; i < Size; i++)
            {
                if (Output[i].Value != 0)
                    return false;
            }
            return true;
            //throw new NotImplementedException();
        }
    }
}
