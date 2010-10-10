using System;
using System.Collections.Generic;
using VisionCode.Framework.Helpers.Concurrent;
using VisionCode.Framework.Helpers.Disk;

namespace ManagedThreadPoolDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //PersistorTest();
            //return;

            ManagedThreadPool pool = new ManagedThreadPool();
            JobExecutor executor = new JobExecutor();

            pool.WorkCompleted += pool_WorkCompleted;
            executor.JobCompleted += executor_JobCompleted;
            pool.QueueUserWorkItem(new MyClass(1));

            pool.QueueUserWorkItem(new MyClass(2));
            //            System.Threading.Thread.Sleep(1000);
            pool.QueueUserWorkItem(new MyClass(3));

            pool.QueueUserWorkItem<int>(Performer, 4);
            pool.QueueUserWorkItem<int>(Performer, 5);
            pool.QueueUserWorkItem(new MyClass(6));


            MyClass mc11 = new MyClass(10);
            MyClass mc12 = new MyClass(20);
            MyClass mc13 = new MyClass(30);
            Job job = new Job();
            job.Tasks.Add(mc13);
            job.Tasks.Add(mc12);
            job.Tasks.Add(mc11);

            MyClass mc21 = new MyClass(40);
            MyClass mc22 = new MyClass(50);
            MyClass mc23 = new MyClass(60);
            Job job2 = new Job();
            job2.Tasks.Add(mc23);
            job2.Tasks.Add(mc22);
            job2.Tasks.Add(mc21);

            executor.EnqueueJob(job);
            executor.EnqueueJob(job2);


            Console.WriteLine("Waiting...");
            //System.Threading.Thread.Sleep(1000);
            pool.AddWorkers(1);
            Console.WriteLine("Workers: " + pool.WorkersCount);
            //pool.Stop();
            Console.ReadKey();
        }

        static void executor_JobCompleted(object sender, JobCompletedEventArgs e)
        {
            Console.WriteLine("JOB COMPLETED");
            Console.WriteLine("ResultsCount=" + e.Results.Length);
            foreach (string s in e.Results)
            {
                Console.WriteLine(s);
            }
            Console.WriteLine("JOB END");
        }

        static void pool_WorkCompleted(object sender, WorkCompletedEventArgs e)
        {
            if (e.Task is MyClass)
            {
                string val = (string)e.Result;
                Console.WriteLine("Work complete: " + val);
            }
        }

        static void Performer(int x)
        {
            Console.WriteLine(x + " Performer Calculating...");
            System.Threading.Thread.Sleep(3000);
            Console.WriteLine(x + " Performer OK");
        }

        static void PersistorTest()
        {
            using (DiskPersistor persistor = new DiskPersistor("data.dta"))
            {
                MyClass c = new MyClass(1235);
                persistor.Save(c);
                c = new MyClass(1234);
                persistor.Save(c);
                c = null;
                //c = persistor.Load<MyClass>(0);
                //Console.WriteLine(c.ID);
                //Console.WriteLine(c.Run());

                IList<MyClass> classes = persistor.LoadAll<MyClass>();
                foreach (MyClass cx in classes)
                {
                    Console.WriteLine(cx.ID);
                    Console.WriteLine(cx.Run());
                }
            }
        }
    }

    [Serializable]
    class MyClass : Task
    {
        int x;

        public MyClass(int v)
        {
            x = v;
        }

        public override object Run()
        {
            Console.WriteLine(x + " Calculating...");
            System.Threading.Thread.Sleep(2500);
            Console.WriteLine(x + " OK");
            return "RetVal=" + x;
        }
    }
}
