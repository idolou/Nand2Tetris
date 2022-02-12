using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    class Program
    {
        static void Main(string[] args)
        {
            //This is an example of a testing code that you should run for all the gates that you create

            //Create an And gate 
            AndGate and = new AndGate();
            Console.WriteLine(and + "");
            //Test that the unit testing works properly
            if (!and.TestGate())
                Console.WriteLine("bugbug");
            
            //Create an Or gate 
            OrGate or = new OrGate();
            Console.WriteLine(or + "");
            //Test that the unit testing works properly
            if (!or.TestGate())
                Console.WriteLine("bugbug");
            
            //Create Xor gate
            XorGate xor = new XorGate();
            Console.WriteLine(xor + "");
            //Test that the unit testing works properly
            if (!xor.TestGate())
                Console.WriteLine("bugbug");
            
            //Create Mux gate
            MuxGate mux = new MuxGate();
            Console.WriteLine(mux + "");
            if (!mux.TestGate())
                Console.WriteLine("bugbug");
            
            //Create DeMux gate
            Demux demux = new Demux();
            Console.WriteLine(demux + "");
            if (!demux.TestGate())
                Console.WriteLine("bugbug");
            
            
            //************ PART 2
            //MultiBitAndGate
            MultiBitAndGate  multibitand = new MultiBitAndGate(3);
            Console.WriteLine(multibitand + "");
            if (!multibitand.TestGate())
                Console.WriteLine("bugbug");
            
            
            //MultiBitOrGate
            MultiBitOrGate multibitor = new MultiBitOrGate(3);
            Console.WriteLine(multibitor + "");
            if(!multibitor.TestGate())
                Console.WriteLine("bugbug");

            //************ PART 3
            //BitwiseAndGate
            BitwiseAndGate bitwiseand = new BitwiseAndGate(2);
            Console.WriteLine(bitwiseand + "");
            if(!bitwiseand.TestGate())
                Console.WriteLine("bugbug");
            
            //BitwiseNotGate
            BitwiseNotGate bitwisenot = new BitwiseNotGate(4);
            Console.WriteLine(bitwisenot + "");
            if(!bitwisenot.TestGate())
                Console.WriteLine("bugbug");
            
            //BitwiseOrGate
            BitwiseOrGate bitwiseor = new BitwiseOrGate(8);
            Console.WriteLine(bitwiseor + "");
            if (!bitwiseor.TestGate())
                Console.WriteLine("bugbug");
            
            //BitwiseMux
            BitwiseMux bitwisemux = new BitwiseMux(8);
            Console.WriteLine(bitwisemux + "");
            if (!bitwisemux.TestGate())
                Console.WriteLine("bugbug");
            
            //BitwiseDemux
            BitwiseDemux bitwisedemux = new BitwiseDemux(4);
            Console.WriteLine(bitwisedemux + "");
            if (!bitwisedemux.TestGate())
                Console.WriteLine("bugbug");



            


            
            //Now we ruin the nand gates that are used in all other gates. The gate should not work properly after this.
            NAndGate.Corrupt = true;
            if (and.TestGate())
                Console.WriteLine("bugbug");



            BitwiseMultiwayMux m = new BitwiseMultiwayMux(7, 3);


            Console.WriteLine("done");
            Console.ReadLine();
        }
    }
}
