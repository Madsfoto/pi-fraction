using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Globalization;


namespace Pi2
{
    class Program
    {
        static decimal n_decimal = 1;
        static decimal d_decimal = 1;
        static decimal divisionResult;
        static decimal best = 3;
        static decimal absResult;
        static string nd_from_file = "";
        static long totalIterations=0;
        
        static void writeBest(decimal best, long iterations)
        {
            totalIterations=totalIterations+iterations;
            using StreamWriter sw_Num_Denom = new("Num_Denom.txt", true);
            
            sw_Num_Denom.WriteLine(n_decimal + ";" + +d_decimal + ";" + divisionResult + ";" + best + ";" + totalIterations);
            sw_Num_Denom.Close();
        }
        
        static void read_N_D()
        {
            if(File.Exists("N_D.txt"))
            {
                using StreamReader sr = new StreamReader("N_D.txt");
                try
                {
                    nd_from_file = sr.ReadLine();
                    // n;d;best

                    string[] ND_file_Arr = nd_from_file.Split(";");
                    n_decimal = long.Parse(ND_file_Arr[0]);
                    d_decimal= long.Parse(ND_file_Arr[1]);
                    best = decimal.Parse(ND_file_Arr[2]);
                    totalIterations = long.Parse(ND_file_Arr[3]);

                   

                }
                catch 
                {

                }

                
                sr.Close();
            }
            else
            {
                using StreamWriter sr = new StreamWriter("N_D.txt",false);
            }
            
        }
        static void DoCalculate(long iterations)
        {
            for (long i = 0; i < iterations; i++)
            {
                divisionResult = n_decimal / d_decimal;
                absResult = Math.Abs(divisionResult - (decimal)Math.PI);
                if (absResult < best)
                {
                    best = absResult;
                    writeBest(best, i);
                }


                if (divisionResult < (decimal)Math.PI)
                {
                    n_decimal++;
                }
                if (divisionResult > (decimal)Math.PI)
                {
                    d_decimal++;
                }


            }

        }


        static void Main(string[] args)
        {
            // the idea:
            // stolen from https://www.wired.com/2011/03/what-is-the-best-fractional-representation-of-pi/
            /* Pi_est = n/d
             * 
            * First, I will start with n = d = 1.
            If n/d is less than pi, I will increase n by 1.
            If n/d is greater than pi, I will increase d by 1.
            Repeat the above until my computer complains.
             * 
             * Todo: bigint thingy for C#
             */

            Stopwatch sw = new Stopwatch();

            sw.Start();
            read_N_D();

            long CustIterations = 0;
            if (args.Length == 0)
            {
                Console.WriteLine("Default: 50 000 000 iterations, should take 10 seconds or so");
                CustIterations = 50000000;
                DoCalculate(CustIterations);
            }
            else if(args.Length == 1)
            {
                CustIterations = long.Parse(args[0]);
                DoCalculate(CustIterations);
            }


            using StreamWriter file_nd = new("N_D.txt", false);
            file_nd.WriteLine(n_decimal+";"+d_decimal+";"+best+";"+totalIterations); 
            file_nd.Close();




            sw.Stop();


            TimeSpan timeSpan = sw.Elapsed;

            Console.WriteLine("Total time taken: {0}h {1}m {2}s {3}ms", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
            Console.WriteLine("Total milliseconds: {0} ms",  timeSpan.TotalMilliseconds);
            Console.WriteLine("Ticks nanoseconds per iteration: {0} ns", timeSpan.TotalNanoseconds / CustIterations);










        }
    }
}
