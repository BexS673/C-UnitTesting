namespace Tracker.Global
{
    //for global constant initialised variables. Cannot instantiate new objects in static classes.
    public static class Types
    {
        public const double V = 1e-24;
        public const double KV = 31.25;

        public static readonly double[,] ZeroMatrix = new double[2, 2] { { 0.0, 0.0 }, { 0.0, 0.0 } };
        public const double Max = 32000.0;



    }

}
