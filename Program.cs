using System;
using System.Threading.Tasks;

namespace CSharp_Async_Example
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await AsyncBreakfast.MakeBreakfast();
        }
    }
}
