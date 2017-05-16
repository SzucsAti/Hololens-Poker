using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HoloToolkit.Unity
{

    public static class GameSingleton
    {
        //protected GameSingleton() { } // guarantee this will be always a singleton only - can't use the constructor!

        public static int players = 2;
    }
}
