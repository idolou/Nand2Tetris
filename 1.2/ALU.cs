using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This class is used to implement the ALU
    class ALU : Gate
    {
        //The word size = number of bit in the input and output
        public int Size { get; private set; }

        //Input and output n bit numbers
        //inputs
        public WireSet InputX { get; private set; }
        public WireSet InputY { get; private set; }
        public WireSet oneOut { get; private set; }
        public WireSet zeroOut { get; private set; }
        
        public WireSet negX { get; private set; }
        public WireSet negY { get; private set; }
        
        public WireSet Xadd1 { get; private set; }
        
        public WireSet Yadd1 { get; private set; }
        public WireSet Xsub1 { get; private set; }
        
        public WireSet Ysub1 { get; private set; }
        
        public WireSet XsubY { get; private set; }
        
        public WireSet YsubX { get; private set; }
        public WireSet Ylog_andX { get; private set; }
        public WireSet Ylog_orX { get; private set; }

        
        public WireSet Control { get; private set; }

        //outputs
        public WireSet Output { get; private set; }
        public Wire Zero { get; private set; }
        
        public Wire Negative { get; private set; }

        public BitwiseAndGate XandY { get; private set;}
        
        public BitwiseOrGate XorY { get; private set;}
        public BitwiseMultiwayMux BitwiseMux { get; private set;}
        
        public BitwiseOrGate MultiOr { get; private set;}
        
        public MultiBitAdder XaddY { get; private set;}
        public BitwiseNotGate notX { get; private set;}
        public BitwiseNotGate notY { get; private set;}
        


        public ALU(int iSize)
        {
            Size = iSize;
            InputX = new WireSet(Size);
            InputY = new WireSet(Size);
            negX = new WireSet(Size);
            negY = new WireSet(Size);
            Output = new WireSet(Size);
            
            zeroOut = new WireSet(Size);
            oneOut = new WireSet(Size);
            
            Xadd1 = new WireSet(Size);
            Yadd1 = new WireSet(Size);
            
            Xsub1 = new WireSet(Size);
            Ysub1 = new WireSet(Size);
            
            XsubY = new WireSet(Size);
            YsubX = new WireSet(Size);
            
            Ylog_andX = new WireSet(Size);
            Ylog_orX = new WireSet(Size);
            

            Control = new WireSet(5);
            Zero = new Wire();
            Negative = new Wire();
            XandY = new BitwiseAndGate(Size);
            XorY = new BitwiseOrGate(Size);
            BitwiseMux = new BitwiseMultiwayMux(Size, 5);
            MultiOr = new BitwiseOrGate(Size);
            XaddY = new MultiBitAdder(Size);
            notY = new BitwiseNotGate(Size);
            notX = new BitwiseNotGate(Size);
            
            // WireSet zeroOut = new WireSet(Size);
            // for (int i = 0; i < Size; i++)
            // {
            //     zeroOut[i].Value = 0;
            // }
            //
            // WireSet oneOut = new WireSet(Size);
            // oneOut[0].Value = 1;
            // for (int i = 1; i < Size; i++)
            // {
            //     oneOut[i].Value = 0;
            // }
            
            notX.ConnectInput(InputX);
            notY.ConnectInput(InputY);
            
            negY.Set2sComplement((-1)*InputY.Get2sComplement());
            negX.Set2sComplement((-1)*InputX.Get2sComplement());

            
            Xadd1.Set2sComplement((1+InputX.Get2sComplement()));
            Yadd1.Set2sComplement(1 + InputY.Get2sComplement());
            
            Xsub1.Set2sComplement(InputX.Get2sComplement()-1);
            Ysub1.Set2sComplement(InputY.Get2sComplement() - 1);
            
            XaddY.ConnectInput1(InputX);
            XaddY.ConnectInput2(InputY);

            XsubY.Set2sComplement(InputX.Get2sComplement() - InputY.Get2sComplement());
            YsubX.Set2sComplement(InputY.Get2sComplement()-InputX.Get2sComplement());
            
            XandY.ConnectInput1(InputX);
            XandY.ConnectInput2(InputY);
            
            XorY.ConnectInput1(InputX);
            XorY.ConnectInput2(InputY);

            if (InputX.Get2sComplement() != 0 && InputY.Get2sComplement() != 0)
            {
                Ylog_andX.ConnectInput(XandY.Output);
            }
            else
            {
                Ylog_andX.ConnectInput(zeroOut);
            }

            if (InputX.Get2sComplement() != 0 || InputY.Get2sComplement() != 0)
            {
                Ylog_orX.ConnectInput(XorY.Output);
            }

            else
            {
                Ylog_orX.ConnectInput(zeroOut);
            }
            
            
            ////////
            BitwiseMux.ConnectControl(Control);
            ///////

            zeroOut.Set2sComplement(0);
            oneOut.Set2sComplement(1);
            
            BitwiseMux.ConnectInput(0,zeroOut);
            BitwiseMux.ConnectInput(1,oneOut);
            BitwiseMux.ConnectInput(2,InputX);
            BitwiseMux.ConnectInput(3, InputY);
            BitwiseMux.ConnectInput(4, notX.Output);
            BitwiseMux.ConnectInput(5, notY.Output);
            BitwiseMux.ConnectInput(6, negX);
            BitwiseMux.ConnectInput(7, negY);
            BitwiseMux.ConnectInput(8, Xadd1);
            BitwiseMux.ConnectInput(9, Yadd1);
            BitwiseMux.ConnectInput(10,Xsub1);
            BitwiseMux.ConnectInput(11,Ysub1);
            BitwiseMux.ConnectInput(12, XaddY.Output);
            BitwiseMux.ConnectInput(13, XsubY);
            BitwiseMux.ConnectInput(14, YsubX);
            BitwiseMux.ConnectInput(15, XandY.Output);
            BitwiseMux.ConnectInput(16,Ylog_andX);
            BitwiseMux.ConnectInput(17,XorY.Output);
            BitwiseMux.ConnectInput(18, Ylog_orX);
                


            Output.ConnectInput(BitwiseMux.Output);




            if (Output.Get2sComplement() == 0)
            {
                Zero.Value = 1;

            }
            else
            {
                Zero.Value = 0;}

            if (Output.Get2sComplement() < 0)
            {
                Negative.Value = 1;
            }
            else
            {
                Negative.Value = 0;
            }
            
            











            //Create and connect all the internal components

        }

        public override bool TestGate()
        {
            Control[0].Value = 1;
            for (int i = 1; i < Size; i++)
            {
                Control[i].Value = 0;
            }

            if (Output[0].Value != 1)
            {
                return false;
            }

            for (int i = 1; i < Size; i++)
            {
                if (Output[i].Value != 0)
                {
                    return false;
                }

            }
            return true;
            //throw new NotImplementedException();
        }
    }
}
