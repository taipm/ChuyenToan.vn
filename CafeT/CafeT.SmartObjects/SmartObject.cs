using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.SmartObjects
{
    public static class Methods
    {
        public static void Inform(string parameter)
        {
            Console.WriteLine("Inform:parameter={0}", parameter);
        }
    }

    public class MathObject
    {
        public int Add(int a, int b) { return a + b; }
        public int Sub(int a, int b) { return a - b; }
    }

    public class SmartObject : ISmartObject
    {
        public int Mul(int a, int b)
        {
            return a * b;
        }

        public void Excute(string method)
        {
            throw new NotImplementedException();
        }

        public void SelectMethod()
        {
            throw new NotImplementedException();
        }
        public void Run()
        {
            // Name of the method we want to call.
            string name = "Inform";

            // Call it with each of these parameters.
            string[] parameters = { "Sam", "Perls" };

            // Get MethodInfo.
            Type type = typeof(Methods);
            MethodInfo info = type.GetMethod(name);

            // Loop over parameters.
            foreach (string parameter in parameters)
            {
                info.Invoke(null, new object[] { parameter });
            }
        }
    }
}
