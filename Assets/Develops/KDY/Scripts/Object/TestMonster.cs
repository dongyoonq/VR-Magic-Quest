using UnityEngine;

public class TestMonster : MonoBehaviour, IHittable
{
    [SerializeField] int hp;

    public void TakeDamaged(int damage)
    {
        hp -= damage;
    }
}
