using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBuilding : MonoBehaviour, IBuilding
{
    public int Health { get; set; }

    public List<Vector2> OccupiedSquares { get; set; }

    protected BaseBuilding(int health)
    {
        Health = health;
        OccupiedSquares = new List<Vector2>();
    }

    public virtual void TakeDamage(int damage)
    {
        Health -= damage;
    }
}
