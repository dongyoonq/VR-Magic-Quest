using UnityEngine;

public class BookUI : MonoBehaviour
{
    public Player player;

    private void OnEnable()
    {
        player = FindAnyObjectByType<Player>();
    }
}