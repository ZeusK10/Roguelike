using System;
using UnityEngine;
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
        private int _foodAmount = 100;

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
            boardManager.Init();
            _foodLabel = uiDocument.rootVisualElement.Q<Label>("FoodLabel");
            _foodLabel.text = "Food: " + _foodAmount;
            playerController.Spawn(boardManager,new Vector2Int(1,1));
            TurnManager.OnTick += OnTurnHappen;
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
        }
    }
}
