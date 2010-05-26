using System;
using System.Collections.Generic;
using System.Threading;

namespace DeadApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Demo1();
        }

        private static void Demo1()
        {
            Thread t1 = new Thread(FastDeadlock.Thread1);
            Thread t2 = new Thread(FastDeadlock.Thread2);
            Thread t3 = new Thread(FastDeadlock.Thread3);

            t1.Start();
            t2.Start();
            t3.Start();

            t1.Join();
            t2.Join();
            t3.Join();
        }

        private static void Demo2()
        {
            Random rnd = new Random();
            Account a1 = new Account(1234.12);
            Account a2 = new Account(56789.56);

            Thread t1 = new Thread(delegate()
            {
                while (true)
                {
                    double funds = rnd.NextDouble() * 100;
                    Account.Transfer(a1, a2, funds);
                    Console.WriteLine(string.Format("Transferred {0} from A1({1}) to A2({2})", funds,  a1.Balance, a2.Balance));
                    Thread.Sleep((int)(rnd.NextDouble() * 10));
                }
            });

            Thread t2 = new Thread(delegate()
            {
                while (true)
                {
                    double funds = rnd.NextDouble() * 100;
                    Account.Transfer(a2, a1, funds);
                    Console.WriteLine(string.Format("Transferred {0} from A2({1}) to A1({2})", funds, a1.Balance, a2.Balance));
                    Thread.Sleep((int)(rnd.NextDouble() * 10));
                }
            });

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();
        }

        private static void Demo3()
        {
            int numThreads = 7;
            object synch = new object();
            List<Multi> multis = new List<Multi>();

            Monitor.Enter(synch);

            for (int i = 0; i < numThreads; i++)
            {
                Multi m = new Multi(synch, i + 1);
                multis.Add(m);
                m.Start();
            }

            Thread.Sleep(2000);

            Monitor.Exit(synch);

            for (int i = 0; i < numThreads; i++)
            {
                multis[i].Thread.Join();
            }
        }
    }

    ///////////////////////////
    // Demo 1
    ///////////////////////////

    class FastDeadlock
    {
        private static object syncA = new object();
        private static object syncB = new object();
        private static object syncC = new object();

        public static void Thread1()
        {
            lock (syncA)
            {
                Console.WriteLine("T1 has A, waits for B");
                Thread.Sleep(200);
                lock (syncB)
                {
                    Console.WriteLine("T1 has A and B - performing action");
                }
            }
        }

        public static void Thread2()
        {
            lock (syncB)
            {
                Console.WriteLine("T2 has B, waits for C");
                Thread.Sleep(200);
                lock (syncC)
                {
                    Console.WriteLine("T2 has B and C - performing action");
                }
            }
        }

        public static void Thread3()
        {
            lock (syncC)
            {
                Console.WriteLine("T3 has C, waits for A");
                Thread.Sleep(200);
                lock (syncA)
                {
                    Console.WriteLine("T3 has C and A - performing action");
                }
            }
        }
    }

    ///////////////////////////
    // Demo 2
    ///////////////////////////

    class Account
    {
        private double m_Balance;

        public Account(double balance)
        {
            m_Balance = balance;
        }

        public double Balance
        {
            get { return m_Balance; }
        }

        public static void Transfer(Account source, Account destination, double funds)
        {
            lock (source)
            {
                lock (destination)
                {
                    source.m_Balance -= funds;
                    destination.m_Balance += funds;
                }
            }
        }
    }
    
    ///////////////////////////
    // Demo 3
    ///////////////////////////

    class Multi
    {
        private object m_Synch;
        private Thread m_Thread;
        private int m_Num;

        public Multi(object synch, int num)
        {
            m_Synch = synch;
            m_Num = num;
        }

        public Thread Thread
        {
            get { return m_Thread; }
        }

        public void Start()
        {
            m_Thread = new Thread(new ThreadStart(Run));
            m_Thread.Start();
        }

        public void Run()
        {
            Thread.Sleep(m_Num * 100);
            Console.WriteLine("Before Wait :" + m_Num);

            Monitor.Enter(m_Synch);
            Thread.Sleep(1000);

            Console.WriteLine("After Wait :" + m_Num);
            Monitor.Exit(m_Synch);
            
        }
    }

    public class CustomSemaphore
    {
        private volatile int semVal;
        //private Queue<Thread> m_Threads = new Queue<Thread>();
        private Stack<Thread> m_Threads = new Stack<Thread>();

        public CustomSemaphore(int semVal)
        {
            if (semVal < 0) this.semVal = 0;
            else this.semVal = semVal;
        }

        public void WaitOne()
        {
            Monitor.Enter(this);
            Thread t = Thread.CurrentThread;

            while (semVal == 0)
            {
                //m_Threads.Enqueue(t);
                m_Threads.Push(t);
                Monitor.Exit(this);
                t.Suspend();
                Monitor.Enter(this);
            }
            semVal--;

            Monitor.Exit(this);
        }

        public void Release()
        {
            lock (this)
            {
                semVal++;

                if (m_Threads.Count > 0)
                {
                    m_Threads.Pop().Resume();
                    //m_Threads.Dequeue().Resume();
                }
            }
        }
    }
}
