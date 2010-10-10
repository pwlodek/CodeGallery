using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace DedlockedWindowsApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
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

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Working...", "UI Thread");
            Console.WriteLine("UI Thread : Working...");
        }
    }

    class FastDeadlock
    {
        private static object syncA = new object();
        private static object syncB = new object();
        private static object syncC = new object();

        public static void Thread1()
        {
            lock (syncA)
            {
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
                Thread.Sleep(200);
                lock (syncA)
                {
                    Console.WriteLine("T3 has C and A - performing action");
                }
            }
        }
    }
}