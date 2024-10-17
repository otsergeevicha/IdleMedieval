using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private bool enableCursor;
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    
    void Start()
    {
        if (enableCursor)
        {
            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
            Cursor.visible = true;
        }
    }
}
