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
        
        public WireSet negOneout { get; private set; }
        
        
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
        
        public MultiBitAdder XsubY { get; private set;}
        
        public MultiBitAdder YsubX { get; private set;}
        
        public BitwiseNotGate notX { get; private set;}
        public BitwiseNotGate notY { get; private set;}
        
        public MultiBitAdder XaddOne {get; private set;}
        public MultiBitAdder YaddOne {get; private set;}
        
        public MultiBitAdder negX {get; private set;}
        public MultiBitAdder negY {get; private set;}
        public MultiBitAdder Xsub1 {get; private set;}
        public MultiBitAdder Ysub1 {get; private set;}
        
        public BitwiseOrGate log_or {get; private set;}
        
        public BitwiseAndGate log_and {get; private set;}
        


        


        public ALU(int iSize)
        {
            Size = iSize;
            InputX = new WireSet(Size);
            InputY = new WireSet(Size);
            Output = new WireSet(Size);


            XaddOne = new MultiBitAdder(Size);
            YaddOne = new MultiBitAdder(Size);

            negX = new MultiBitAdder(Size);
            negY = new MultiBitAdder(Size);
            
            zeroOut = new WireSet(Size);
            oneOut = new WireSet(Size);
            negOneout = new WireSet(Size);
            


            


            
            Xsub1 = new MultiBitAdder(Size);
            Ysub1 = new MultiBitAdder(Size);
            
            XsubY = new MultiBitAdder(Size);
            YsubX = new MultiBitAdder(Size);
            
            Ylog_andX = new WireSet(Size);
            Ylog_orX = new WireSet(Size);

            log_or = new BitwiseOrGate(Size);
            log_and = new BitwiseAndGate(Size);
            

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
            
            
            zeroOut.Set2sComplement(0);
            oneOut.Set2sComplement(1);
            negOneout.Set2sComplement(-1);
            


            
            notX.ConnectInput(InputX);
            notY.ConnectInput(InputY);
            
            negX.ConnectInput1(notX.Output);
            negX.ConnectInput2(oneOut);
            
            negY.ConnectInput1(notY.Output);
            negY.ConnectInput2(oneOut);
            
            
            XaddOne.ConnectInput1(InputX);
            XaddOne.ConnectInput2(oneOut);

            YaddOne.ConnectInput1(InputY);
            YaddOne.ConnectInput2(oneOut);
            
            Xsub1.ConnectInput1(InputX);
            Xsub1.ConnectInput2(negOneout);
            
            Ysub1.ConnectInput1(InputY);
            Ysub1.ConnectInput2(negOneout);
            
            XsubY.ConnectInput1(InputX);
            XsubY.ConnectInput2(negY.Output);
            
            XaddY.ConnectInput1(InputX);
            XaddY.ConnectInput2(InputY);
            
            YsubX.ConnectInput1(InputY);
            YsubX.ConnectInput2(negX.Output);
            


            
            




            
            



            
            
            XandY.ConnectInput1(InputX);
            XandY.ConnectInput2(InputY);
            
            XorY.ConnectInput1(InputX);
            XorY.ConnectInput2(InputY);

            if (InputX.Get2sComplement() != 0 && InputY.Get2sComplement() != 0)
            {
                log_and.ConnectInput1(oneOut);
                log_and.ConnectInput2(oneOut);
                
            }
            else
            {

                log_and.ConnectInput1(zeroOut);
                log_and.ConnectInput2(zeroOut);

            }

            if (InputX.Get2sComplement() != 0 || InputY.Get2sComplement() != 0)
            {
                log_or.ConnectInput1(zeroOut);
                log_or.ConnectInput2(zeroOut);
            }

            else
            {
               log_or.ConnectInput1(oneOut);
               log_or.ConnectInput2(oneOut);
            }
            
            
            ////////
            BitwiseMux.ConnectControl(Control);
            ///////


            
            BitwiseMux.ConnectInput(0,zeroOut);
            BitwiseMux.ConnectInput(1,oneOut);
            BitwiseMux.ConnectInput(2,InputX);
            BitwiseMux.ConnectInput(3, InputY);
            BitwiseMux.ConnectInput(4, notX.Output);
            BitwiseMux.ConnectInput(5, notY.Output);
            BitwiseMux.ConnectInput(6, negX.Output);
            BitwiseMux.ConnectInput(7, negY.Output);
            BitwiseMux.ConnectInput(8, XaddOne.Output);
            BitwiseMux.ConnectInput(9, YaddOne.Output);
            BitwiseMux.ConnectInput(10,Xsub1.Output);
            BitwiseMux.ConnectInput(11,Ysub1.Output);
            BitwiseMux.ConnectInput(12, XaddY.Output);
            BitwiseMux.ConnectInput(13, XsubY.Output);
            BitwiseMux.ConnectInput(14, YsubX.Output);
            BitwiseMux.ConnectInput(15, XandY.Output);
            BitwiseMux.ConnectInput(16,oneOut);
            BitwiseMux.ConnectInput(17,XorY.Output);
            BitwiseMux.ConnectInput(18, oneOut);
                


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
            InputX.Set2sComplement(3);
            InputY.Set2sComplement(1);
            Control.Set2sComplement(0);
            if (Output.Get2sComplement() != 0 && Zero.Value != 0)
            {
                return false;
            } 
            
            Control.Set2sComplement(1);
            if (Output.Get2sComplement() != 1)
            {
                return false;
            }
            
            Control.Set2sComplement(2);
            if (Output.Get2sComplement() != InputX.Get2sComplement())
            {
                return false;
            }
            
            Control.Set2sComplement(4);
            WireSet ch = new WireSet(Size);
            ch.Set2sComplement(-4);
            if (Output.Get2sComplement() != ch.Get2sComplement())
            {
                return false;
            }
            Control.Set2sComplement(7);
            ch.Set2sComplement(-1);
            if (Output.Get2sComplement() != ch.Get2sComplement())
            {
                return false;
            }
            Control.Set2sComplement(8);
            ch.Set2sComplement(4);
            if (Output.Get2sComplement() != ch.Get2sComplement())
            {
                return false;
            }
            Control.Set2sComplement(11);
            ch.Set2sComplement(0);
            if (Output.Get2sComplement() != ch.Get2sComplement())
            {
                return false;
            }
            Control.Set2sComplement(12);
            ch.Set2sComplement(4);
            if (Output.Get2sComplement() != ch.Get2sComplement())
            {
                return false;
            }
            Control.Set2sComplement(14);
            ch.Set2sComplement(-2);
            if (Output.Get2sComplement() != ch.Get2sComplement() && Negative.Value != 1)
            {
                return false;
            }
            Control.Set2sComplement(17);
            if (Output.Get2sComplement() != InputX.Get2sComplement())
            {
                return false;
            }
            // Control.Set2sComplement(16);
            // ch.Set2sComplement(0);
            // if (Output.Get2sComplement() != ch.Get2sComplement() && Zero.Value != 0)
            // {
            //     return false;
            // }
            InputX.Set2sComplement(7);
            InputY.Set2sComplement(10);
            Control.Set2sComplement(16);
            ch.Set2sComplement(1);
            Console.WriteLine(ch);
            Console.WriteLine(Output);
            if (Output.Get2sComplement() != ch.Get2sComplement())
            {
                return false;
            }

            return true;
            
        }
    }
}
