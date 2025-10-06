using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursorNomal;
    [SerializeField] private Texture2D cursorShoot;
    [SerializeField] private Texture2D cursorReload;
    private Vector2 hotspot = new Vector2(16, 48);
    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(cursorNomal,hotspot,CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.SetCursor(cursorShoot, hotspot, CursorMode.Auto);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(cursorNomal, hotspot, CursorMode.Auto);
        }
        if(Input.GetMouseButtonDown(1))
        {
            Cursor.SetCursor(cursorReload, hotspot, CursorMode.Auto);
        }
        else if(Input.GetMouseButtonUp(1)) 
        {
            Cursor.SetCursor(cursorNomal, hotspot, CursorMode.Auto);
        }
    }
}
