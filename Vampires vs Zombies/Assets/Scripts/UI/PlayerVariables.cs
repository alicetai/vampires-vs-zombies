using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerVariables
{
    public static CharacterType PlayerType { get; set; } = CharacterType.Zombie;
    public static Difficulty PlayerDifficulty { get; set;} = Difficulty.Easy;
    public static bool isGameFinished {get; set;} = false;
    public static bool wonGame {get; set;} = true;
    public static bool isDead {get; set;} = false;
    public static float mouseSensitivity {get; set;} = 40f;
    public static bool isZombie {get; set;} = true;
    public static float gameVolume {get; set;} = 0.3f;
}
