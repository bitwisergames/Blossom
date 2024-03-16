namespace Blossom.scripts.interfaces;

public interface IFlower : IDamaging
{
    /// <summary>Ignore damaging aspects if true</summary>
    bool Passive { get; }

    /// <summary>The amount of pollen this gives per stage</summary>
    /// <remarks>Should be a 4 long int array</remarks>
    int[] Income { get; }

    /// <summary>How many pollinations is needed for upgrading</summary>
    /// <remarks>Should be a 3 long int array</remarks>
    int[] Upgrade { get; }

    /// <summary>-1 to 3, what stage the flower is at</summary>
    /// <remarks>-1 is ground mound</remarks>
    int Stage { get; }

    /// <summary>The amount of pollen this has</summary>
    int Pollen { get; }

    /// <summary>Has this been pollinated this round</summary>
    bool Pollinated { get; }
}