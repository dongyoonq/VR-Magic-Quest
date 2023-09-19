using UnityEngine;

public class BookUI : MonoBehaviour
{
    public Player player;
    public BookCanvasUI bookCanvasUI;

    private void OnEnable()
    {
        player = FindAnyObjectByType<Player>();
        bookCanvasUI = FindAnyObjectByType<BookCanvasUI>();
    }
}