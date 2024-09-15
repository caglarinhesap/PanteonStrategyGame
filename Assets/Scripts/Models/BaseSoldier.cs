using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSoldier : MonoBehaviour, ISoldier
{
    public int Health { get; set; }
    public int Damage { get; set; }

    public List<Vector2> OccupiedSquares { get; set; }

    protected BaseSoldier(int health, int damage)
    {
        Health = health;
        Damage = damage;
        OccupiedSquares = new List<Vector2>();
    }

    public abstract void Move();
    public abstract void Attack();

    public virtual void TakeDamage(int damage)
    {
        Health -= damage;
    }
}
