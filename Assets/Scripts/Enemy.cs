using UnityEngine;
using UnityEngine.Serialization;

namespace ZeuskGames
{
    public class Enemy : CellObject
    { 
        [SerializeField] private int health = 3;
        private int _currentHealth;

        private void Awake()
       {
          TurnManager.OnTick += TurnHappened;
       }

       private void OnDestroy()
       {
           TurnManager.OnTick -= TurnHappened;
       }

       public override void Init(Vector2Int coord)
       {
          base.Init(coord);
          _currentHealth = health;
       }

       public override bool PlayerWantsToEnter()
       {
           _currentHealth -= 1;

           if (_currentHealth <= 0)
           {
              Destroy(gameObject);
           }

           return false;
       }

       bool MoveTo(Vector2Int coord)
       {
           var board = GameManager.Instance.GetBoardManager();
           var targetCell =  board.GetCellData(coord);

          if (targetCell == null
              || !targetCell.passable
              || targetCell.containedObject != null)
          {
              return false;
          }
        
          //remove enemy from current cell
          var currentCell = board.GetCellData(_Cell);
          currentCell.containedObject = null;
        
          //add it to the next cell
          targetCell.containedObject = this;
          _Cell = coord;
          transform.position = board.CellToWorld(coord);

          return true;
       }

       void TurnHappened()
       {
          //We added a public property that return the player current cell!
          var playerController = GameManager.Instance.GetPlayerController();
          var playerCell = playerController.GetCell();
          int xDist = playerCell.x - _Cell.x;
          int yDist = playerCell.y - _Cell.y;

          int absXDist = Mathf.Abs(xDist);
          int absYDist = Mathf.Abs(yDist);

          if ((xDist == 0 && absYDist == 1)
              || (yDist == 0 && absXDist == 1))
          {
              //we are adjacent to the player, attack!
              GameManager.Instance.ChangeFood(-3);
          }
          else
          {
              if (absXDist > absYDist)
              {
                  if (!TryMoveInX(xDist))
                  {
                      //if our move was not successful (so no move and not attack)
                      //we try to move along Y
                      TryMoveInY(yDist);
                  }
              }
              else
              {
                  if (!TryMoveInY(yDist))
                  {
                      TryMoveInX(xDist);
                  }
              }
          }
       }

       bool TryMoveInX(int xDist)
       {
          //try to get closer in x
         
          //player to our right
          if (xDist > 0)
          {
              return MoveTo(_Cell + Vector2Int.right);
          }
        
          //player to our left
          return MoveTo(_Cell + Vector2Int.left);
       }

       bool TryMoveInY(int yDist)
       {
          //try to get closer in y
         
          //player on top
          if (yDist > 0)
          {
              return MoveTo(_Cell + Vector2Int.up);
          }

          //player below
          return MoveTo(_Cell + Vector2Int.down);
       }
    }
}
