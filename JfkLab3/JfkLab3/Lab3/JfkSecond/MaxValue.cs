using JfkCallable;
using System;

namespace JfkSecond
{
    [Description("Metoda zwraca większą z dwóch podanych wartości. Podaj dwa parametry typu double oddzielone ; " +
                    "oraz parametry muszą być z przedziału [1, 10]", Copyright = "Rafał Szczepański")]
    public class MaxValue : ICallable
    {
        public double Call(double a, double b) => Math.Max(a, b);
    }
}
