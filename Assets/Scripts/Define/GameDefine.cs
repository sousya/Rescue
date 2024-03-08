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
        public static int totalLeveNum = 6;
    }
}
