using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ZeuskGames
{
    public class PlayerController : MonoBehaviour
    {
        private BoardManager _boardManager;
        private Vector2Int _cellPotition;
        private bool _isGameOver;
        [SerializeField] private float moveSpeed = 5f;
        private bool _isMoving;
        private Vector3 moveTarget;
        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public Vector2Int GetCell()
        {
            return _cellPotition;
        }

        public void Init()
        {
            _isMoving = false;
            _isGameOver = false;
        }
        
        private void Update()
        {
            if (_isGameOver)
            {
                if (Keyboard.current.enterKey.wasPressedThisFrame)
                {
                    GameManager.Instance.StartNewGame();
                }
                return;
            }
            
            if (_isMoving)
            {
                transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);

                if (transform.position == moveTarget)
                {
                    _isMoving = false;
                    _animator.SetBool("Moving",_isMoving);
                    var cellData = _boardManager.GetCellData(_cellPotition);
                    if(cellData.containedObject != null)
                        cellData.containedObject.PlayerEntered();
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
                    
                    if (cellData.containedObject == null || cellData.containedObject.PlayerWantsToEnter())
                    {
                        MoveTo(newCellTarget,false);
                    }
                    else
                    {
                        _animator.SetTrigger("Attack");
                    }
                }
            }
        }

        public void GameOver()
        {
            _isGameOver = true;
        }

        private void MoveTo(Vector2Int cell, bool isImmidiate)
        {
            _cellPotition = cell;
            if (isImmidiate)
            {
                _isMoving = false;
                transform.position = _boardManager.CellToWorld(cell);
            }
            else
            {
                _isMoving = true;
                moveTarget = _boardManager.CellToWorld(cell);
            }
            _animator.SetBool("Moving",_isMoving);
        }
        
        public void Spawn(BoardManager boardManager, Vector2Int cell)
        {
            _boardManager = boardManager;
            _cellPotition = cell;
            MoveTo(cell,true);
        }
    }
}
