using JfkCallable;
using System;

namespace JfkFirst
{
    [Description("Metoda zwraca długośc łuku okręgu, a to kąt alfa, b to długość promienia podany w cm. Wynik zwracany jest w [cm^2]. " +
                    "Podaj dwa parametry typu double oddzielone ; oraz parametry muszą być z przedziału [1, 10]",
                    Copyright = "Rafał Szczepański")]
    public class BowLength : ICallable
    {
        public double Call(double a, double b) => (a * (2 * Math.PI * b)) / 360;
    }
}
