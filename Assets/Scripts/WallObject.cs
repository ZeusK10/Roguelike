using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ZeuskGames
{
    public class WallObject : CellObject
    {
        [SerializeField] private List<Tile> obstacleTile;
        [SerializeField] private int maxHealth = 3;

        private int _healthPoint;
        private Tile _originalTile;

        public override void Init(Vector2Int cell)
        {
            base.Init(cell);
            _healthPoint = maxHealth;
            _originalTile = GameManager.Instance.GetCellTile(cell);
            GameManager.Instance.SetCellTile(cell, obstacleTile[_healthPoint-1]);
        }

        public override bool PlayerWantsToEnter()
        {
            _healthPoint -= 1;
            if (_healthPoint > 0)
            {
                GameManager.Instance.SetCellTile(_Cell, obstacleTile[_healthPoint-1]);
                return false;
            }

            GameManager.Instance.SetCellTile(_Cell, _originalTile);
            Destroy(gameObject);
            return true;
        }
    }
}
