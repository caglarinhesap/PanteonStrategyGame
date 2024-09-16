using System.Collections.Generic;
using UnityEngine;

public interface ISoldier : IUnit
{ 
    public int Damage { get; set; }
    public GameObject Target { get; set; }
    public Vector2 CurrentPosition { get; set; }
    public Vector2 TargetPosition { get; set; }
    public List<PathNode> path { get; set; }
    public void SetPath(Vector2 currentLocation, Vector2 targetLocation);
    public void Move();
    public void ReachTarget(GameObject target);
    public void Attack();
}
