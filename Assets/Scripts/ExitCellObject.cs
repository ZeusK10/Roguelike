using UnityEngine;
using UnityEngine.Tilemaps;

namespace ZeuskGames
{
    public class ExitCellObject : CellObject
    {
        [SerializeField] private Tile endTile;

        public override void Init(Vector2Int coord)
        {
            base.Init(coord);
            GameManager.Instance.SetCellTile(coord,endTile);
        }

        public override void PlayerEntered()
        {
            GameManager.Instance.NewLevel();
        }
    }
}
