using System.Collections.Generic;
using UnityEngine;

public interface IUnit
{
    public int Health { get; set; }
    public List<Vector2> OccupiedSquares { get; set; }
    public void TakeDamage(int damage);
}
