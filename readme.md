# Async C Sharp
## System.Threading.Tasks.Task
The Task class, and related classes, are used for reasoning about tasks in progress.

A Task object represents a workload that has been started, each task is awaited with `await` for its result. If a method contains
an `await`, the method signature must have the `async` keyword in it as the method contains asynchronous operations.

## Async Return Types
Async methods will usually return a `Task` or `Task<TResult>`. The async method calling another async method should have an `await`
statement in its caller.

Specify `Task<TResult>` when the method return type specifies an operand of type `TResult`.

Specify `Task` when the method does not specify a return statement or has a return statement without an operand.

Event handlers will often have a return type of `void`, which means they can't be awaited. The caller can not catch exceptions
either.

C# 8.0 onwards allows the `IAsyncEnumerable<T>` for async methods returning async streams.

### Generalised Return Types
Async methods, starting with C# 7.0, can return any type with an accessible `GetAwaiter` method.

`Task` and `Task<TResult>` are reference types, which can affect performance due to memory allocation in performance-critical apps.
Generalised return types allow lightweight value types to be returned instead of reference types, this helps avoid additional
memory allocations.

.NET provides `System.Threading.Tasks.ValueTask<TResult>` for generalised return types. Add the `System.Threading.Tasks.Extensions`
NuGet package to the project to use this class.

Example taken from https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/async-return-types
```
using System;
using System.Threading.Tasks;

class Program
{
    static readonly Random s_rnd = new Random();

    static async Task Main() =>
        Console.WriteLine($"You rolled {await GetDiceRollAsync()}");

    static async ValueTask<int> GetDiceRollAsync()
    {
        Console.WriteLine("Shaking dice...");

        int roll1 = await RollAsync();
        int roll2 = await RollAsync();

        return roll1 + roll2;
    }

    static async ValueTask<int> RollAsync()
    {
        await Task.Delay(500);

        int diceRoll = s_rnd.Next(1, 7);
        return diceRoll;
    }
}
// Example output:
//    Shaking dice...
//    You rolled 8
```

### Enumerable example
```
static async IAsyncEnumerable<string> ReadWordsFromStreamAsync()
{
    string data =
        @"This is a line of text.
          Here is the second line of text.
          And there is one more for good measure.
          Wait, that was the penultimate line.";

    using var readStream = new StringReader(data);

    string line = await readStream.ReadLineAsync();
    while (line != null)
    {
        foreach (string word in line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            yield return word;
        }

        line = await readStream.ReadLineAsync();
    }
}
```