using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_Async_Example
{
    class AsyncBreakfast
    {
        /*
         * Makes a breakfast asynchronously.
         */
        /// <summary>
        ///  Makes a breakfast asynchronously.
        /// </summary>
        /// <returns>
        ///  A Task representing whether MakeBreakfast is complete or not.
        /// </returns>
        public static async Task MakeBreakfast()
        {
            Coffee cup = PourCoffee();
            Console.WriteLine($"Coffee has been poured and is {(cup.IsFresh ? "fresh" : "not fresh")}");

            var eggsTask = FryEggsAsync(2);
            var baconTask = FryBaconAsync(3);
            var toastTask = MakeToastWithButterAndJamAsync(2);

            var breakfastTasks = new List<Task> { eggsTask, baconTask, toastTask };

            while (breakfastTasks.Count > 0)
            {
                // When any task from breakfastTags is finished, the finished task is returned
                Task finishedTask = await Task.WhenAny(breakfastTasks);

                if (finishedTask == eggsTask) Console.WriteLine("Eggs are finished");
                else if (finishedTask == baconTask) Console.WriteLine("Bacon is finished");
                else if (finishedTask == toastTask) Console.WriteLine("Toast has toasted");

                breakfastTasks.Remove(finishedTask);
            }

            Juice oj = PourOJ();
            Console.WriteLine($"{oj.Name} is ready");
            Console.WriteLine("Breakfast is ready");
        }

        private static Juice PourOJ()
        {
            Juice juice = new Juice();
            juice.Name = "Orange Juice";
            return juice;
        }

        private static async Task<Toast> MakeToastWithButterAndJamAsync(int number)
        {
            var toast = await ToastBreadAsync(number);
            toast = ApplyButter(toast);
            toast = ApplyJam(toast);

            return toast;
        }

        private static Toast ApplyJam(Toast toast)
        {
            Console.WriteLine("Jamming up the toast");
            toast.AddTopping("Jam");
            return toast;
        }

        private static Toast ApplyButter(Toast toast)
        {
            Console.WriteLine("Buttering up the toast");
            toast.AddTopping("Butter");
            return toast;
        }

        private static async Task<Toast> ToastBreadAsync(int slices)
        {
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("Putting bread slice into toaster");
            }
            Console.WriteLine("Toasting");
            await Task.Delay(3000);
            Console.WriteLine("Removing toast from toaster");

            return new Toast();
        }

        private static async Task<Bacon> FryBaconAsync(int slices)
        {
            Console.WriteLine($"putting {slices} slices of bacon in the pan");
            Console.WriteLine("cooking first side of bacon...");
            await Task.Delay(3000);

            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("flipping a slice of bacon");
            }

            Console.WriteLine("cooking the second side of the bacon...");
            await Task.Delay(3000);
            Console.WriteLine("Putting bacon on a plate");

            return new Bacon();
        }

        private static async Task<Egg> FryEggsAsync(int eggs)
        {
            Console.WriteLine("Warming the egg pan...");
            await Task.Delay(3000);
            Console.WriteLine($"cracking {eggs} eggs");
            Console.WriteLine("cooking the eggs ...");
            await Task.Delay(3000);
            Console.WriteLine("Put eggs on plate");

            return new Egg();
        }

        private static Coffee PourCoffee()
        {
            Coffee coffee = new Coffee();
            coffee.IsFresh = true;
            return coffee;
        }
    }

    internal class Juice
    {
        public string Name { get; set; }
    }

    internal class Egg
    {
    }

    internal class Bacon
    {
    }

    internal class Toast
    {
        public List<string> Toppings { get; set; }

        public void AddTopping(string topping)
        {
            if (Toppings == null) Toppings = new List<string>();
            Toppings.Add(topping);
        }
    }

    internal class Coffee
    {
        public bool IsFresh { get; set; }
    }
}
