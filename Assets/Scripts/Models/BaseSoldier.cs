using DG.Tweening;
using Interfaces;
using Managers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Models
{
    public abstract class BaseSoldier : MonoBehaviour, ISoldier
    {
        public int Health { get; set; }
        public int Damage { get; set; }
        public GameObject Target { get; set; }
        public Vector2 CurrentPosition { get; set; }
        public Vector2 TargetPosition { get; set; }
        public List<PathNode> path { get; set; }
        public List<Vector2> OccupiedSquares { get; set; }
        public List<Vector2> SurroundingSquares { get; set; }

        protected BaseSoldier(int health, int damage)
        {
            Health = health;
            Damage = damage;
            OccupiedSquares = new List<Vector2>();
            SurroundingSquares = new List<Vector2>();
        }

        public virtual void SetPath(Vector2 currentLocation, Vector2 targetLocation)
        {
            CurrentPosition = currentLocation;
            TargetPosition = targetLocation;

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

                transform.DOKill();
                transform.DOMove(worldPosition, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    GameManager.Instance.mapController.mapModel.GetPathfinding().GetNode((int)OccupiedSquares[0].x, (int)OccupiedSquares[0].y).SetIsWalkable(true);
                    OccupiedSquares.RemoveAt(0);
                    OccupiedSquares.Add(new Vector2(path[0].x, path[0].y));

                    SurroundingSquares.Clear();
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (i == 1 && j == 1)
                            {
                                continue;
                            }

                            SurroundingSquares.Add(new Vector2(path[0].x + i - 1, path[0].y + j - 1)); //Get surrounding squares.
                        }
                    }

                    if (path.Count > 1)
                    {
                        CurrentPosition = new Vector2(path[1].x, path[1].y);
                    }

                    GameManager.Instance.mapController.mapModel.GetPathfinding().GetNode(path[0].x, path[0].y).SetIsWalkable(false);
                    path.RemoveAt(0);

                    if (path.Count > 0)
                    {
                        SetPath(CurrentPosition, TargetPosition); //Set path again after one move. If path is blocked it will find new path.
                    }
                    else
                    {
                        if (Target != null && Target.activeSelf)
                        {
                            ReachTarget(Target);
                        }
                    }
                });
            }
        }

        public virtual void ReachTarget(GameObject targetUnit)
        {
            Target = targetUnit;

            bool isNearTarget = false;
            foreach (Vector2 surroundingSquare in targetUnit.GetComponent<IUnit>().SurroundingSquares)
            {
                if (surroundingSquare == CurrentPosition)
                {
                    isNearTarget = true;
                    break;
                }
            }

            if (isNearTarget)
            {
                Attack();
            }
            else
            {
                if (targetUnit.GetComponent<BaseBuilding>() != null) //It can't move. Reach and attack.
                {
                    List<List<PathNode>> paths = new List<List<PathNode>>();

                    foreach (Vector2 surroundingSquare in targetUnit.GetComponent<BaseBuilding>().SurroundingSquares)
                    {
                        paths.Add(GameManager.Instance.mapController.SetTargetPosition((int)CurrentPosition.x, (int)CurrentPosition.y, (int)surroundingSquare.x, (int)surroundingSquare.y));
                    }

                    path = GetSmallestPath(paths);

                    if (path != null && path.Count > 0)
                    {
                        TargetPosition = new Vector2(path[path.Count - 1].x, path[path.Count - 1].y);
                        Move();
                    }
                }
                else if (targetUnit.GetComponent<BaseSoldier>() != null) //Target is a soldier. Check position changes.
                {
                    List<List<PathNode>> paths = new List<List<PathNode>>();

                    foreach (Vector2 surroundingSquare in targetUnit.GetComponent<BaseSoldier>().SurroundingSquares)
                    {
                        paths.Add(GameManager.Instance.mapController.SetTargetPosition((int)CurrentPosition.x, (int)CurrentPosition.y, (int)surroundingSquare.x, (int)surroundingSquare.y));
                    }

                    path = GetSmallestPath(paths);

                    if (path != null && path.Count > 0)
                    {
                        TargetPosition = new Vector2(path[path.Count - 1].x, path[path.Count - 1].y);
                        Move();
                    }
                }
            }
        }

        public virtual void Attack()
        {
            if (Target.activeSelf)
            {
                transform.DOKill();
                Target.transform.DOKill();
                transform.DOShakePosition(0.5f, 5f, 10, 90, false, true);
                Target.transform.DOShakePosition(0.5f, 5f, 10, 90, false, true);
                Target.GetComponent<IUnit>().TakeDamage(Damage);
                DOVirtual.DelayedCall(0.5f, () => Attack());
            }
            else //Target destroyed. Stop attacking.
            {
                //Target = null;
            }
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

        private List<PathNode> GetSmallestPath(List<List<PathNode>> paths)
        {
            List<PathNode> smallestPath = paths
                .Where(path => path != null && path.Count > 0) // Null ve boþ path'leri filtrele
                .OrderBy(path => path.Count)
                .FirstOrDefault();

            return smallestPath;
        }
    }
}