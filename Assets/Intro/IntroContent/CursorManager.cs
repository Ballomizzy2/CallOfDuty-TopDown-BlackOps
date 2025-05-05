using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D zombieCursor;
    public Vector2 hotspot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    void Start()
    {
        Cursor.SetCursor(zombieCursor, hotspot, cursorMode);
        DontDestroyOnLoad(gameObject);
    }
}
