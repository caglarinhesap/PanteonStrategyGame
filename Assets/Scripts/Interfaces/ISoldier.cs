using System.Collections.Generic;
using UnityEngine;

public interface ISoldier : IUnit
{ 
    public int Damage { get; set; }
    public List<PathNode> path { get; set; }
    public void SetPath(Vector2 currentLocation, Vector2 targetLocation);
    public void Move()
    public void Attack();
}
