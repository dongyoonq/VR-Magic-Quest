using UnityEngine;

public class TestMonster : MonoBehaviour, IHitable
{
    [SerializeField] int hp;

    public void TakeDamaged(int damage)
    {
        hp -= damage;
    }
}
