public interface IUnit
{
    int Health { get; set; }
    int Damage { get; set; }
    void Move();
    void Attack();
}
