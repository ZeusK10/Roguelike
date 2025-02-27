using UnityEngine;
using UnityEngine.InputSystem;

namespace ZeuskGames
{
    public class PlayerController : MonoBehaviour
    {
        private BoardManager _boardManager;
        private Vector2Int _cellPotition;
        private bool _isGameOver;

        private void Update()
        {
            if (_isGameOver)
            {
                if (Keyboard.current.enterKey.wasPressedThisFrame)
                {
                    GameManager.Instance.StartNewGame();
                    _isGameOver = false;
                }
                return;
            }
            Vector2Int newCellTarget = _cellPotition;
            bool hasMoved = false;

            if(Keyboard.current.upArrowKey.wasPressedThisFrame)
            {
                newCellTarget.y += 1;
                hasMoved = true;
            }
            else if(Keyboard.current.downArrowKey.wasPressedThisFrame)
            {
                newCellTarget.y -= 1;
                hasMoved = true;
            }
            else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            {
                newCellTarget.x += 1;
                hasMoved = true;
            }
            else if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            {
                newCellTarget.x -= 1;
                hasMoved = true;
            }

            if(hasMoved)
            {
                CellData cellData = _boardManager.GetCellData(newCellTarget);

                if(cellData != null && cellData.passable)
                {
                    GameManager.Instance.UpdateTick();
                    
                    if (cellData.containedObject == null)
                    {
                        MoveTo(newCellTarget);
                    }
                    else if(cellData.containedObject.PlayerWantsToEnter())
                    {
                        MoveTo(newCellTarget);
                        //Call PlayerEntered AFTER moving the player! Otherwise not in cell yet
                        cellData.containedObject.PlayerEntered();
                    }
                }
            }
        }

        public void GameOver()
        {
            _isGameOver = true;
        }

        private void MoveTo(Vector2Int cell)
        {
            _cellPotition = cell;
            transform.position = _boardManager.CellToWorld(cell);
        }
        
        public void Spawn(BoardManager boardManager, Vector2Int cell)
        {
            _boardManager = boardManager;
            MoveTo(cell);
        }
    }
}
