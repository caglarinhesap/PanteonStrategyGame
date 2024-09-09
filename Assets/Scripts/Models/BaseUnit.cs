public abstract class BaseUnit : IUnit
{
    public int Health { get; set; }
    public int Damage { get; set; }

    protected BaseUnit(int health, int damage)
    {
        Health = health;
        Damage = damage;
    }

    public abstract void Move();
    public abstract void Attack();
}
