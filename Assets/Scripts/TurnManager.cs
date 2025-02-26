using System;
using UnityEngine;

namespace ZeuskGames
{
    public class TurnManager
    {
        public static Action OnTick;
        private int _turnCount;

        public TurnManager()
        {
            _turnCount = 1;
        }

        public void Tick()
        {
            OnTick?.Invoke();
            _turnCount++;
        }
    }
}
