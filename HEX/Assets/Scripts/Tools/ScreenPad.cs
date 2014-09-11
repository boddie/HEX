using UnityEngine;
using System.Collections;

public enum StickDirection
{
    NONE,
    UP,
    DOWN,
    LEFT,
    RIGHT,
    UP_RIGHT,
    UP_LEFT,
    DOWN_RIGHT,
    DOWN_LEFT
}

public class ScreenPad
{
    private Texture screenJoyStick;
    private Texture screenTouchPad;
    private Rect joyStickRect;
    private Rect touchPadRect;

    private Vector2 mousePosition;
    private bool startFollowing = false;

    private Rect initialPosition;

    public StickDirection JoyDirection { get; private set; }

    private Vector2 joyCenter;
    private Vector2 none;
    private Vector2 left;
    private Vector2 right;
    private Vector2 top;
    private Vector2 bottom;
    private Vector2 topLeft;
    private Vector2 topRight;
    private Vector2 bottomLeft;
    private Vector2 bottomRight;

    public ScreenPad(Rect locationAndSize)
    {
        screenJoyStick = Resources.Load<Texture>("UI_Elements/screenJoyStick");
        screenTouchPad = Resources.Load<Texture>("UI_Elements/screenTouchPad");
        initialPosition = new Rect(locationAndSize.x + locationAndSize.width / 4, 
            locationAndSize.y + locationAndSize.height / 4, locationAndSize.width / 2, locationAndSize.height / 2);
        joyStickRect = initialPosition;
        touchPadRect = locationAndSize;

        JoyDirection = StickDirection.NONE;

        joyCenter = new Vector2(joyStickRect.x + joyStickRect.width / 2, joyStickRect.y + joyStickRect.height / 2);
        none = new Vector2(touchPadRect.x + touchPadRect.width / 2, touchPadRect.y + touchPadRect.height / 2);
        left = new Vector2(touchPadRect.x + joyStickRect.width / 2, touchPadRect.y + touchPadRect.height / 2);
        right = new Vector2((touchPadRect.x + touchPadRect.width) - joyStickRect.width / 2, touchPadRect.y + touchPadRect.height / 2);
        top = new Vector2(touchPadRect.x + touchPadRect.width / 2, touchPadRect.y + joyStickRect.height / 2);
        bottom = new Vector2(touchPadRect.x + touchPadRect.width / 2, (touchPadRect.y + touchPadRect.height) - joyStickRect.height / 2);
        topLeft = new Vector2(touchPadRect.x + joyStickRect.width / 2, touchPadRect.y + joyStickRect.height / 2);
        topRight = new Vector2((touchPadRect.x + touchPadRect.width) - joyStickRect.width / 2, touchPadRect.y + joyStickRect.height / 2);
        bottomLeft = new Vector2(touchPadRect.x + joyStickRect.width / 2, (touchPadRect.y + touchPadRect.height) - joyStickRect.height / 2);
        bottomRight = new Vector2((touchPadRect.x + touchPadRect.width) - joyStickRect.width / 2, (touchPadRect.y + touchPadRect.height) - joyStickRect.height / 2);
    }

	public void Update () 
    {
        if (!Input.GetMouseButton(0))
        {
            startFollowing = false;
        }

        mousePosition = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        if (Input.GetMouseButton(0) && joyStickRect.Contains(mousePosition) || startFollowing)
        {
            startFollowing = true;
            float joyX = Mathf.Clamp(mousePosition.x - joyStickRect.width / 2, touchPadRect.x, (touchPadRect.x + touchPadRect.width) - joyStickRect.width);
            float joyY = Mathf.Clamp(mousePosition.y - joyStickRect.height / 2, touchPadRect.y, (touchPadRect.y + touchPadRect.height) - joyStickRect.height);
            joyStickRect = new Rect(joyX, joyY, joyStickRect.width, joyStickRect.height);
            CalculateDirection();
        }
        else
        {
            joyStickRect = initialPosition;
            JoyDirection = StickDirection.NONE;
        }
	}

    public void OnGUI()
    {
        GUI.DrawTexture(touchPadRect, screenTouchPad, ScaleMode.StretchToFill);
        GUI.DrawTexture(joyStickRect, screenJoyStick, ScaleMode.StretchToFill);
    }

    public void CalculateDirection()
    {
        float minDistance = Vector2.Distance(mousePosition, none);
        JoyDirection = StickDirection.NONE;
        if (Vector2.Distance(mousePosition, left) < minDistance)
        {
            minDistance = Vector2.Distance(mousePosition, left);
            JoyDirection = StickDirection.LEFT;
        }
        if (Vector2.Distance(mousePosition, right) < minDistance)
        {
            minDistance = Vector2.Distance(mousePosition, right);
            JoyDirection = StickDirection.RIGHT;
        }
        if (Vector2.Distance(mousePosition, top) < minDistance)
        {
            minDistance = Vector2.Distance(mousePosition, top);
            JoyDirection = StickDirection.UP;
        }
        if (Vector2.Distance(mousePosition, bottom) < minDistance)
        {
            minDistance = Vector2.Distance(mousePosition, bottom);
            JoyDirection = StickDirection.DOWN;
        }
        if (Vector2.Distance(mousePosition, topLeft) < minDistance)
        {
            minDistance = Vector2.Distance(mousePosition, topLeft);
            JoyDirection = StickDirection.UP_LEFT;
        }
        if (Vector2.Distance(mousePosition, topRight) < minDistance)
        {
            minDistance = Vector2.Distance(mousePosition, topRight);
            JoyDirection = StickDirection.UP_RIGHT;
        }
        if (Vector2.Distance(mousePosition, bottomLeft) < minDistance)
        {
            minDistance = Vector2.Distance(mousePosition, bottomLeft);
            JoyDirection = StickDirection.DOWN_LEFT;
        }
        if (Vector2.Distance(mousePosition, bottomRight) < minDistance)
        {
            minDistance = Vector2.Distance(mousePosition, bottomRight);
            JoyDirection = StickDirection.DOWN_RIGHT;
        }
    }
}
