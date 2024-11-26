using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using UnityEngine;
using UnityEngine.UI;

namespace GameDefine 
{
    public enum GameType
    {
        Trail = 1,
        Animal = 2,
        Color = 3,
        Job = 4,
        Friend = 5,
        Romance = 6,
        Child = 7,
        ChildEasy = 8,
        Zombie = 9,
        Island = 10,
        War = 11,
    }

    public enum AnimType
    {
        Death1 = 1,
        Death2 = 2,
        Death3 = 3,
        Run = 4,
        Shoot = 5,
        Win = 6,
        Fallen = 7,
        Idle = 8,
        Ground = 9,
    }

    public enum LanguageType
    {
        zh = 0,
        ja = 1,
        en = 2,
        ko = 3,
    }

    public enum AttractType
    {
        Honey = 0,
        Ball  = 1,
    }

    public enum AnimalTag
    {
    }

    public enum GameAllType
    {
        Selection = 0
    }

    public static class GameConst
    {
        public const int MaxVitality = 5;
        public const int RecoveryTime = 270;
    }
}
