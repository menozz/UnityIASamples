using System;

public enum GameState
{
    None,
    SelectionStarted,
    Animating
}

[Flags]
public enum BonusType
{
    None,
    DestroyWholeRowColumn
}


public static class BonusTypeUtilities
{
    /// <summary>
    /// Helper method to check for specific bonus type
    /// </summary>
    /// <param name="bt"></param>
    /// <returns></returns>
    public static bool ContainsDestroyWholeRowColumn(BonusType bt)
    {
        return (bt & BonusType.DestroyWholeRowColumn)
               == BonusType.DestroyWholeRowColumn;
    }
}
