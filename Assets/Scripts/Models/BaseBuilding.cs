using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBuilding : MonoBehaviour, IBuilding
{
    public int Health { get; set; }
    public List<Vector2> OccupiedSquares { get; set; }
    public List<Vector2> SurroundingSquares { get; set; }

    protected BaseBuilding(int health)
    {
        Health = health;
        OccupiedSquares = new List<Vector2>();
        SurroundingSquares = new List<Vector2>();
    }

    public virtual void TakeDamage(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            if (SelectionManager.Instance.selectedMapObject == this)
            {
                SelectionManager.Instance.Deselect();
            }

            foreach (Vector2 square in OccupiedSquares)
            {
                GameManager.Instance.mapController.mapModel.GetPathfinding().GetNode((int)square.x, (int)square.y).SetIsWalkable(true);
            }

            UnitManager.Instance.Units.Remove(gameObject);
            UnitFactory.Instance.DestroyUnit(gameObject);
        }
    }
}
