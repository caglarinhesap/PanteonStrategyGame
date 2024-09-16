using System.Collections.Generic;
using UnityEngine;

namespace Interfaces
{
    public interface IUnit
    {
        public int Health { get; set; }
        public List<Vector2> OccupiedSquares { get; set; }
        public List<Vector2> SurroundingSquares { get; set; }
        public void TakeDamage(int damage);
    }
}