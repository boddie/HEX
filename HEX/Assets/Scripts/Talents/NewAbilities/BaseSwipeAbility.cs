using UnityEngine;

public class SwipeAbilityUseContext : BaseAbilityUseContext
{
    public SwipeGesture SwipeGesture
    {
        get;
        private set;
    }

    public SwipeAbilityUseContext(SwipeGesture swipeGesture, Player castingPlayer, Vector3 casterPosition)
        : base(castingPlayer, casterPosition)
    {
        SwipeGesture = swipeGesture;
    }
}

public abstract class BaseSwipeAbility : BaseAbility
{
    public void Use(SwipeAbilityUseContext abilityContext)
    {
        if (Cooldown < timer)
        {
            UseCritical(abilityContext);
            timer = 0.0f;
        }
    }
    protected abstract void UseCritical(SwipeAbilityUseContext abilityContext);
}

