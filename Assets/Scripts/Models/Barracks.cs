using UnityEngine;

namespace Models
{
    public class Barracks : BaseBuilding
    {
        public Barracks() : base(100) { }
        public Vector2 SpawnPoint { get; set; }
    }
}