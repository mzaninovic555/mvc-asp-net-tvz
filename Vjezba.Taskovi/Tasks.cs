using System;

// Task t1 = Task.Run(() =>
// {
//     Console.WriteLine("T1 Sleeping started");
//     Thread.Sleep(1000);
//     Console.WriteLine("T1 Sleeping completed");
// });

// Task t2 = Task.Run(() =>
// {
//     Console.WriteLine("T2 Sleeping started");
//     Thread.Sleep(1500);
//     Console.WriteLine("T2 Sleeping completed");
// });

// Task.WaitAll(t1, t2);

Console.WriteLine("----------------------------------------");

static async Task SleepF1()
{
    Console.WriteLine("SleepF1 called");
     SleepF2();
    await Task.Delay(1000);
    Console.WriteLine("SleepF1 finished waiting 1000 ms");
}

static async Task SleepF2()
{
    Console.WriteLine("SleepF2 called");
    await Task.Delay(1500);
    Console.WriteLine("SleepF2 finished waiting 1500 ms");
}

Task taskMethod = SleepF1();
taskMethod.Wait();
