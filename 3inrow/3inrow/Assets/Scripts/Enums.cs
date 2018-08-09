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
