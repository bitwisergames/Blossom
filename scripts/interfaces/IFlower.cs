namespace Blossom.scripts.interfaces;

public interface IFlower : IDamaging
{
    /// <summary>The damage done per stage</summary>
    /// <remarks>Should be a 4 long int array</remarks>
    int[] ScaledDamage { get; }

    /// <summary>Ignore damaging aspects if true</summary>
    bool Passive { get; }

    /// <summary>The amount of pollen this gives per stage</summary>
    /// <remarks>Should be a 4 long int array</remarks>
    int[] Income { get; }

    /// <summary>How many pollinations is needed for upgrading</summary>
    /// <remarks>Should be a 3 long int array</remarks>
    int[] UpgradeCost { get; }

    /// <summary>-1 to 3, what stage the flower is at</summary>
    /// <remarks>-1 is ground mound</remarks>
    int Stage { get; }

    /// <summary>The amount of pollen this has for upgrading</summary>
    int Pollen { get; }

    /// <summary>Has this been pollinated this round</summary>
    bool Pollinated { get; }

    /// <summary>Attempt to pollinate the flower</summary>
    /// <returns>false if the flower has already been pollinated</returns>
    bool Pollinate();
}