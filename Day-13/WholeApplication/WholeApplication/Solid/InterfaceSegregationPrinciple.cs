using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WholeApplication.Solid
{
    //public interface IWorker
    //{
    //    void Work();
    //    void Eat();
    //}

    //public class Robot : IWorker
    //{
    //    public void Work()
    //    {
    //        Console.WriteLine("Robot is working.");
    //    }

    //    public void Eat()
    //    {
    //        throw new NotImplementedException("Robot doesn't eat.");
    //    }
    //}

    public interface IWorkable
    {
        void Work();
    }

    public interface IEatable
    {
        void Eat();
    }

    public class Human : IWorkable, IEatable
    {
        public void Work()
        {
            Console.WriteLine("Human is working.");
        }

        public void Eat()
        {
            Console.WriteLine("Human is eating.");
        }
    }

    public class Robot : IWorkable
    {
        public void Work()
        {
            Console.WriteLine("Robot is working.");
        }
    }

}
