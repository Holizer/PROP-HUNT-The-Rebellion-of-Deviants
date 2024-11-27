using Random = System.Random;

public static class SeededRandomGenerator
{
    private static Random rng;

    public static void SetSeed(int seed)
    {
        rng = new Random(seed);
    }

    public static int GetRandomIndex(int range, int seed)
    {
        if (rng == null)
        {
            throw new System.InvalidOperationException("Seed has not been set.");
        }
        return rng.Next(0, range);
    }
}
