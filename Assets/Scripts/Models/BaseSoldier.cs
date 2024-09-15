using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSoldier : MonoBehaviour, ISoldier
{
    public int Health { get; set; }
    public int Damage { get; set; }
    public List<PathNode> path { get; set; }
    public List<Vector2> OccupiedSquares { get; set; }

    protected BaseSoldier(int health, int damage)
    {
        Health = health;
        Damage = damage;
        OccupiedSquares = new List<Vector2>();
    }

    public virtual void SetPath(Vector2 currentLocation, Vector2 targetLocation)
    {
        path = GameManager.Instance.mapController.SetTargetPosition((int)currentLocation.x, (int)currentLocation.y, (int)targetLocation.x, (int)targetLocation.y);

        if (path != null && path.Count > 0)
        {
            Move();
        }
    }

    public virtual void Move()
    {
        if (path.Count > 0)
        {
            Vector3 worldPosition = GameManager.Instance.mapController.mapView.GetWorldPositionFromGrid(new Vector2(path[0].x, path[0].y));

            transform.DOMove(worldPosition, 0.5f).SetEase(Ease.Linear).OnComplete(() => {
                GameManager.Instance.mapController.mapModel.GetPathfinding().GetNode((int)OccupiedSquares[0].x, (int)OccupiedSquares[0].y).SetIsWalkable(true);
                OccupiedSquares.RemoveAt(0);
                OccupiedSquares.Add(new Vector2(path[0].x, path[0].y));
                GameManager.Instance.mapController.mapModel.GetPathfinding().GetNode(path[0].x, path[0].y).SetIsWalkable(false);
                path.RemoveAt(0);
                Move();
            });
        }
    }

    public abstract void Attack();

    public virtual void TakeDamage(int damage)
    {
        Health -= damage;
    }
}
