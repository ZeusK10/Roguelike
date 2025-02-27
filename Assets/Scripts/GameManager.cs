using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

namespace ZeuskGames
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        [SerializeField] private BoardManager boardManager;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private UIDocument uiDocument;
        private Label _foodLabel;
        private TurnManager _turnManager;
        private const int MAX_FOOD_AMOUNT = 10;
        private int _foodAmount;
        private int _currentLevel = 0;
        private VisualElement _gameOverPanel;
        private Label _gameOverMessage;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            _turnManager = new TurnManager();
            TurnManager.OnTick += OnTurnHappen;
            
            
            _foodLabel = uiDocument.rootVisualElement.Q<Label>("FoodLabel");

            _gameOverPanel = uiDocument.rootVisualElement.Q<VisualElement>("GameOverPanel");
            _gameOverMessage = _gameOverPanel.Q<Label>("GameOverMessage");
            StartNewGame();
        }

        public void StartNewGame()
        {
            _currentLevel = 0;
            NewLevel();
            _foodAmount = MAX_FOOD_AMOUNT;
            _foodLabel.text = "Food: " + _foodAmount;
            _gameOverPanel.style.visibility = Visibility.Hidden;
        }
        
        public void NewLevel()
        {
            boardManager.Clean();
            boardManager.Init();
            playerController.Spawn(boardManager, new Vector2Int(1,1));

            _currentLevel++;
        }

        public void SetCellTile(Vector2Int cellIndex, Tile tile)
        {
            boardManager.SetCellTile(cellIndex,tile);
        }

        public Tile GetCellTile(Vector2Int cell)
        {
            return boardManager.GetCellTile(cell);
        }

        private void OnDestroy()
        {
            TurnManager.OnTick -= OnTurnHappen;
        }

        public void UpdateTick()
        {
            _turnManager.Tick();
        }

        private void OnTurnHappen()
        {
            ChangeFood(-1);
        }

        public void ChangeFood(int amount)
        {
            _foodAmount += amount;
            _foodLabel.text = "Food: " + _foodAmount;
            if (_foodAmount <= 0)
            {
                playerController.GameOver();
                _gameOverPanel.style.visibility = Visibility.Visible;
                _gameOverMessage.text = "Game Over!\n\nYou traveled through " + _currentLevel + " levels\n\nPress Enter to Restart!";

            }
        }
    }
}
