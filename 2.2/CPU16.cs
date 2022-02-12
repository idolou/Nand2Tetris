using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleComponents;

namespace Machine
{
    public class CPU16 
    {
         public const int J0 = 0, J1 = 1, J2 = 2, D0 = 3, D1 = 4, D2 = 5, C0 = 6, C1 = 7, C2 = 8, C3 = 9, C4 = 10, A = 11, X0 = 12, X1 = 13, X2 = 14, Type = 15;

        public int Size { get; private set; }

        public WireSet Instruction { get; private set; }
        public WireSet MemoryInput { get; private set; }
        public Wire Reset { get; private set; }

        public WireSet MemoryOutput { get; private set; }
        public Wire MemoryWrite { get; private set; }
        public WireSet MemoryAddress { get; private set; }
        public WireSet InstructionAddress { get; private set; }

        private ALU m_gALU;
        private Counter m_rPC;
        private MultiBitRegister m_rA, m_rD;
        private BitwiseMux m_gAMux, m_gMAMux;
        
        

        

        public CPU16()
        {
            Size =  16;

            Instruction = new WireSet(Size);
            MemoryInput = new WireSet(Size);
            MemoryOutput = new WireSet(Size);
            MemoryAddress = new WireSet(Size);
            InstructionAddress = new WireSet(Size);
            MemoryWrite = new Wire();
            Reset = new Wire();

            m_gALU = new ALU(Size);
            m_rPC = new Counter(Size);
            m_rA = new MultiBitRegister(Size);
            m_rD = new MultiBitRegister(Size);

            m_gAMux = new BitwiseMux(Size);
            m_gMAMux = new BitwiseMux(Size);

            m_gAMux.ConnectInput1(Instruction);
            m_gAMux.ConnectInput2(m_gALU.Output);

            m_rA.ConnectInput(m_gAMux.Output);

            m_gMAMux.ConnectInput1(m_rA.Output);
            m_gMAMux.ConnectInput2(MemoryInput);
            m_gALU.InputY.ConnectInput(m_gMAMux.Output);

            m_gALU.InputX.ConnectInput(m_rD.Output);

            m_rD.ConnectInput(m_gALU.Output);

            MemoryOutput.ConnectInput(m_gALU.Output);
            MemoryAddress.ConnectInput(m_rA.Output);

            InstructionAddress.ConnectInput(m_rPC.Output);
            m_rPC.ConnectInput(m_rA.Output);
            m_rPC.ConnectReset(Reset);

            ConnectControls();
        }

       //Add gates for control implementation here
       
       // or gate for register A
       private NotGate regA_not;
       private OrGate regA_Or;
       private AndGate regA_and;
       
       //********** ALU Zero and Neg
       private WireSet ALU_Zero;
       private WireSet ALU_Neg;
       
       //for Register D
       private AndGate And_regD;
       private AndGate Mamux_and;
       private AndGate Mwrite_and;
       
       //ALU and gates
       private AndGate c0;
       private AndGate c1;
       private AndGate c2;
       private AndGate c3;
       private AndGate c4;
       
       
       //jump inputs
       //do not jump
       private WireSet Zero;
       
       //uncondtional
       private WireSet One;
       
        //jump greater THEN
        private BitwiseOrGate or_JGT;
       private BitwiseNotGate not_JGT;


       //jump equal
       
       
       //jump greater equal
       private BitwiseNotGate not_JGE;
       
       //not equal
       
       private BitwiseNotGate not_JNE;
       
       //jump less equal then

       private BitwiseOrGate or_JLE;
       
       
       
       //jump mux

       private BitwiseMultiwayMux jumps;
       
       
       //and gate for pc Load

       private AndGate pcL_and;
       
       
       
       //and jump mux controls

       private AndGate j0_and;
       private AndGate j1_and;
       private AndGate j2_and;


       private BitwiseNotGate zero_not;
       private BitwiseNotGate one_not;
       
           
       





       private void ConnectControls()
        {
            //1. connect control of mux 1 (selects entrance to register A)
            m_gAMux.ConnectControl(Instruction[Type]);

            //2. connect control to mux 2 (selects A or M entrance to the ALU)
            Mamux_and = new AndGate();
            
            Mamux_and.ConnectInput1(Instruction[Type]);
            Mamux_and.ConnectInput2(Instruction[A]);
            m_gMAMux.ConnectControl(Mamux_and.Output);


            //3. consider all instruction bits only if C type instruction (MSB of instruction is 1)

            //4. connect ALU control bits
            c0 = new AndGate();
            c1 = new AndGate();
            c2 = new AndGate();
            c3 = new AndGate();
            c4 = new AndGate();
            
            c0.ConnectInput1(Instruction[Type]);
            c0.ConnectInput2(Instruction[C0]);
            
            c1.ConnectInput1(Instruction[Type]);
            c1.ConnectInput2(Instruction[C1]);
            
            c2.ConnectInput1(Instruction[Type]);
            c2.ConnectInput2(Instruction[C2]);
            
            c3.ConnectInput1(Instruction[Type]);
            c3.ConnectInput2(Instruction[C3]);
            
            c4.ConnectInput1(Instruction[Type]);
            c4.ConnectInput2(Instruction[C4]);

            m_gALU.Control[0].ConnectInput(c0.Output);
            m_gALU.Control[1].ConnectInput(c1.Output);
            m_gALU.Control[2].ConnectInput(c2.Output);
            m_gALU.Control[3].ConnectInput(c3.Output);
            m_gALU.Control[4].ConnectInput(c4.Output);


            ALU_Zero = new WireSet(1);
            ALU_Neg = new WireSet(1);


            //5. connect control to register D (very simple)
            And_regD = new AndGate();
            
            And_regD.ConnectInput1(Instruction[Type]);
            And_regD.ConnectInput2(Instruction[D1]);
            m_rD.Load.ConnectInput(And_regD.Output);
            

            //6. connect control to register A (a bit more complicated)
            regA_and = new AndGate();
            regA_not = new NotGate();
            regA_Or = new OrGate();
            
            
            regA_and.ConnectInput1(Instruction[Type]);
            regA_and.ConnectInput2(Instruction[D2]);
            
            regA_not.ConnectInput(Instruction[Type]);
            
            regA_Or.ConnectInput1(regA_not.Output);
            regA_Or.ConnectInput2(regA_and.Output);
            
            m_rA.Load.ConnectInput(regA_Or.Output);



            //7. connect control to MemoryWrite
            
            Mwrite_and = new AndGate();
            Mwrite_and.ConnectInput1(Instruction[Type]);
            Mwrite_and.ConnectInput2(Instruction[D0]);
            MemoryWrite.ConnectInput(Mwrite_and.Output);
            

            //8. create inputs for jump mux
            
            //turn alu zero and neg into wireset

            
            //input 0 and 7 - true/false
            
            zero_not = new BitwiseNotGate(1);
            one_not = new BitwiseNotGate(1);

            ALU_Neg[0].ConnectInput(m_gALU.Negative);
            ALU_Zero[0].ConnectInput(m_gALU.Zero);
            
            Zero = new WireSet(1);
            One = new WireSet(1);

            
            



            //jump greater THEN - input 1
            or_JGT = new BitwiseOrGate(1);
            not_JGT = new BitwiseNotGate(1);
            
            or_JGT.ConnectInput1(ALU_Zero);
            or_JGT.ConnectInput2(ALU_Neg);
            not_JGT.ConnectInput(or_JGT.Output);
            

            
            //jump greater equal - input 2
            not_JGE = new BitwiseNotGate(1);
            not_JGE.ConnectInput(ALU_Neg);
            
            //jump not equal - input 5
            not_JNE = new BitwiseNotGate(1);
            not_JNE.ConnectInput(ALU_Zero);
            
            //jump less equal - input 16
            or_JLE = new BitwiseOrGate(1);
            or_JLE.ConnectInput1(ALU_Zero);
            or_JLE.ConnectInput2(ALU_Neg);
            

            //9. connect jump mux (this is the most complicated part)
            jumps = new BitwiseMultiwayMux(1,3);
            
            //controls
            j0_and = new AndGate();
            j1_and = new AndGate();
            j2_and = new AndGate();

            j0_and.ConnectInput1(Instruction[Type]);
            j0_and.ConnectInput2(Instruction[J0]);

            j1_and.ConnectInput1(Instruction[Type]);
            j1_and.ConnectInput2(Instruction[J1]);
            
            j2_and.ConnectInput1(Instruction[Type]);
            j2_and.ConnectInput2(Instruction[J2]);

            
            
            jumps.Control[0].ConnectInput(Instruction[J0]);
            jumps.Control[1].ConnectInput(Instruction[J1]);
            jumps.Control[2].ConnectInput(Instruction[J2]);
            
            //dont jump
            jumps.ConnectInput(0, Zero);
            //jump greater then
            jumps.ConnectInput(1, not_JGT.Output);
            //jump equal
            jumps.ConnectInput(2, ALU_Zero);
            //jump greater equal
            jumps.ConnectInput(3,not_JGE.Output);
            //jump less then
            jumps.ConnectInput(4, ALU_Neg);
            //jump not equal
            jumps.ConnectInput(5, not_JNE.Output);
            // jump less equal
            jumps.ConnectInput(6, or_JLE.Output);
            //unconditional
            jumps.ConnectInput(7, One);

            
            ALU_Zero.SetValue(m_gALU.Zero.Value);
            ALU_Neg.SetValue((m_gALU.Negative.Value));

            Zero.SetValue(0);
            One.SetValue(1);
            
            //10. connect PC load control
            
            pcL_and = new AndGate();
            pcL_and.ConnectInput1(Instruction[Type]);
            pcL_and.ConnectInput2(jumps.Output[0]);
            
            m_rPC.Load.ConnectInput(pcL_and.Output);


        }
        

        public override string ToString()
        {
            return "A=" + m_rA + ", D=" + m_rD + ", PC=" + m_rPC + ",Ins=" + Instruction;
        }

        private string GetInstructionString()
        {
            if (Instruction[Type].Value == 0)
                return "@" + Instruction.GetValue();
            return Instruction[Type].Value + "XXX " +
               "a" + Instruction[A] + " " +
               "c" + Instruction[C4] + Instruction[C3] + Instruction[C2] + Instruction[C1] + Instruction[C0] + " " +
               "d" + Instruction[D2] + Instruction[D1] + Instruction[D0] + " " +
               "j" + Instruction[J2] + Instruction[J1] + Instruction[J0];
        }

        public void PrintState()
        {
            Console.WriteLine("CPU state:");
            Console.WriteLine("PC=" + m_rPC + "=" + m_rPC.Output.GetValue());
            Console.WriteLine("A=" + m_rA + "=" + m_rA.Output.GetValue());
            Console.WriteLine("D=" + m_rD + "=" + m_rD.Output.GetValue());
            Console.WriteLine("Ins=" + GetInstructionString());
            Console.WriteLine("ALU=" + m_gALU);
            Console.WriteLine("inM=" + MemoryInput);
            Console.WriteLine("outM=" + MemoryOutput);
            Console.WriteLine("addM=" + MemoryAddress);
        }
    }
}
