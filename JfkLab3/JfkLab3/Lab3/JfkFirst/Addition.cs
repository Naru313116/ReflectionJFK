using JfkCallable;

namespace JfkFirst
{
    [Description("Metoda zwraca sume dwóch liczb. Podaj dwa parametry typu double oddzielone ; " +
                    "oraz parametry muszą być z przedziału [1, 10]", Copyright = "Rafał Szczepański")]
    public class Addition : ICallable
    {
        public double Call(double a, double b) => a + b;
    }
}
