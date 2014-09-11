
using UnityEngine;

public class AbilityPair
{
    public BaseTapAbility TapAbility
    {
        get;
        private set;
    }

    public BaseSwipeAbility SwipeAbility
    {
        get;
        private set;
    }
    public static Vector2 IconDims { get; private set; }
    static AbilityPair()
    {
        IconDims = new Vector2(Screen.width * (3f/32f), Screen.height * (7f/40f));
    }
    public AbilityPair(BaseTapAbility tapAbility, BaseSwipeAbility swipeAbility, string name, string desc, string filepath)
    {
        TapAbility = tapAbility;
        SwipeAbility = swipeAbility;
        Texture = new InterfaceTexture(filepath, new Rect(0, 0, IconDims.x, IconDims.y));
        Description = desc;
        Name = name;
    }
    public readonly InterfaceTexture Texture;
    public readonly string Description;
    public readonly string Name;
    public void Update()
    {
        SwipeAbility.Update();
        TapAbility.Update();
    }
}

