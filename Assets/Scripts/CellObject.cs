using UnityEngine;

namespace ZeuskGames
{
    public class CellObject : MonoBehaviour
    {
        protected Vector2Int _Cell;

        public virtual void Init(Vector2Int cell)
        {
            _Cell = cell;
        }
        public virtual void PlayerEntered()
        {
            
        }
        
        public virtual bool PlayerWantsToEnter()
        {
            return true;
        }
    }
}
