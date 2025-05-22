using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WholeApplication.Solid
{
    public class Greeting
    {
        public string getgreeting(string language)
        {
            if (language == "english")
                return "hello";
            else if (language == "spanish")
                return "hola";

            else
                return "hi";
        }

        public interface IGreeting
        {
            string SayHello();
        }

        
        public class EnglishGreeting : IGreeting
        {
            public string SayHello() => "Hello";
        }

        public class SpanishGreeting : IGreeting
        {
            public string SayHello() => "Hola";
        }

        public class FrenchGreeting : IGreeting
        {
            public string SayHello() => "Bonjour";
        }


        public class Greeter
        {
            public void Greet(IGreeting greeting)
            {
                Console.WriteLine(greeting.SayHello());
            }
        }
    }


}
