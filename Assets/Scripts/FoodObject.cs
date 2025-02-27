using UnityEngine;

namespace ZeuskGames
{
    public class FoodObject : CellObject
    {
        public int foodValue;
        public override void PlayerEntered()
        {
            GameManager.Instance.ChangeFood(foodValue);
            Destroy(gameObject);
        }
    }
}
