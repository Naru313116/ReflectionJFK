using JfkCallable;
using System;

namespace JfkSecond
{
    [Description("Metoda zwraca potęgę liczby, której podstawą jest a, potęgę zaś definiuje wartość w b. " +
                    "Podaj dwa parametry typu double oddzielone ; oraz parametry muszą być z przedziału [1, 10]",
                    Copyright = "Rafał Szczepański")]
    public class Exponentiation : ICallable
    {
        public double Call(double a, double b) => Math.Pow(a, b);
    }
}
