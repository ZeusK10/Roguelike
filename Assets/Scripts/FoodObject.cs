using UnityEngine;

namespace ZeuskGames
{
    public class FoodObject : CellObject
    {
        public override void PlayerEntered()
        {
            GameManager.Instance.ChangeFood(foodValue);
            Destroy(gameObject);
        }
    }
}
