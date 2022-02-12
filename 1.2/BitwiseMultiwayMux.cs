using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This class implements a mux with k input, each input with n wires. The output also has n wires.

    class BitwiseMultiwayMux : Gate
    {
        //Word size - number of bits in each output
        public int Size { get; private set; }

        //The number of control bits needed for k outputs
        public int ControlBits { get; private set; }

        public WireSet Output { get; private set; }
        public WireSet Control { get; private set; }
        public WireSet[] Inputs { get; private set; }

        private BitwiseMux[] muxGates;
        

        public BitwiseMultiwayMux(int iSize, int cControlBits)
        {
            Size = iSize;
            Output = new WireSet(Size);
            Control = new WireSet(cControlBits);
            Inputs = new WireSet[(int)Math.Pow(2, cControlBits)];
            
            for (int i = 0; i < Inputs.Length; i++)
            {
                Inputs[i] = new WireSet(Size);
                
            }

            // creates empty mux gates in the mux_gates array in the size of bits(Size)
            int inpLen = Inputs.Length;
            muxGates = new BitwiseMux[inpLen - 1];
            
            for (int i = 0; i < inpLen - 1; i++)
            {
                muxGates[i] = new BitwiseMux(Size);
            }
            
            //will connect all the inputs to the "first level of binary tree muxes"
            //will connect the first control to all first level muxes
            for (int i = 0; i < (inpLen/2)+1; i+=2)
            {
                muxGates[i/2].ConnectInput1(Inputs[i]);
                muxGates[i/2].ConnectInput2(Inputs[i + 1]);
                muxGates[i/2].ConnectControl(Control[0]);

            }
            //to do -fix
            
            //now we will connect all the outputs of first level mux gates to the next level
            //will connect the control bit accordingly
            int m = 1;
            int Siter = 0;
            int curr = Inputs.Length;
            int stopI =  Inputs.Length/4;
            while (m < Control.Size)
            {
                
                for (int n = 0 ; n < stopI ; n+=1)
                {
                    
                    muxGates[curr/2].ConnectInput1(muxGates[Siter].Output);
                    muxGates[curr / 2].ConnectInput2(muxGates[Siter + 1].Output);
                    muxGates[curr / 2].ConnectControl(Control[m]);
                    curr += 2;
                    Siter += 2;
                    
                }
                stopI = stopI / 2;
                m++;
            }
            
            //connect the last mux gate output to the class output
            WireSet outP = muxGates[^1].Output;
            Output.ConnectInput(outP);
        }


        public void ConnectInput(int i, WireSet wsInput)
        {
            Inputs[i].ConnectInput(wsInput);
        }
        public void ConnectControl(WireSet wsControl)
        {
            Control.ConnectInput(wsControl);
        }



        public override bool TestGate()
        {
            //check if the last input is the result
            for (int i = 0; i < Control.Size; i++)
            {
                Control[i].Value = 1;
            }

            for (int i = 0; i < Size; i++)
            {
                Inputs[^1][i].Value = 1;
            }
            
            for (int i = 0; i < Size; i++)
            {
                Inputs[0][i].Value = 0;
            }

            for (int i = 0; i < Size; i++)
            {
                if (Output[i].Value != 1)
                {return false;}
            
            }
            
            //check if the first input is the result
            for (int i = 0; i < Control.Size; i++)
            {
                Control[i].Value = 0;
            }

            for (int i = 0; i < Size; i++)
            {
                Inputs[^1][i].Value = 1;
            }
            
            for (int i = 0; i < Size; i++)
            {
                Inputs[0][i].Value = 0;
            }

            for (int i = 0; i < Size; i++)
            {
                if (Output[i].Value != 0)
                {return false;}
            
            }
            return true;
        }
    }
    }

