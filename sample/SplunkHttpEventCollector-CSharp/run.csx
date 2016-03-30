using System;

public static void Run(TimerInfo input, out object splunkEvent)
{
    splunkEvent = new
    {
        message = "Hello from a C# Function"
    };
}