public interface ISoldier : IUnit
{ 
    public int Damage { get; set; }
    public void Move();
    public void Attack();
}
