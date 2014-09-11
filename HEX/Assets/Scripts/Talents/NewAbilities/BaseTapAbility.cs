using UnityEngine;

public class TapGesture
{
    public Vector3 TapScreenPoint
    {
        get;
        private set;
    }

    public Vector3 TapWorldPoint
    {
        get;
        private set;
    }

    public TapGesture(Vector3 screenPoint, Vector3 worldPoint)
    {
        TapScreenPoint = screenPoint;
        TapWorldPoint = worldPoint;
    }
}

public class SwipeGesture
{
    public Vector3 StartScreenPoint
    {
        get;
        private set;
    }

    public Vector3 EndScreenPoint
    {
        get;
        private set;
    }

    public Vector3 StartWorldPoint
    {
        get;
        private set;
    }

    public Vector3 EndWorldPoint
    {
        get;
        private set;
    }

    public SwipeGesture(Vector3 startingPoint, Vector3 endPoint, Vector3 startWorldPoint, Vector3 endWorldPoint)
    {
        StartScreenPoint = StartScreenPoint;
        EndScreenPoint = endPoint;
        StartWorldPoint = startWorldPoint;
        EndWorldPoint = endWorldPoint;
    }
}

public abstract class BaseAbilityUseContext
{
    public Player CastingPlayer
    {
        get;
        private set;
    }

    public Vector3 CasterPosition
    {
        get;
        private set;
    }

    public BaseAbilityUseContext(Player castingPlayer, Vector3 casterPosition)
    {
        CastingPlayer = castingPlayer;
        CasterPosition = casterPosition;
    }
}

public class TapAbilityUseContext : BaseAbilityUseContext
{
    public TapGesture TapGesture
    {
        get;
        private set;
    }

    public TapAbilityUseContext(TapGesture tapGesture, Player castingPlayer, Vector3 casterPosition)
        : base(castingPlayer, casterPosition)
    {
        TapGesture = tapGesture;
    }
}

public abstract class BaseTapAbility : BaseAbility
{  
    public void Use(TapAbilityUseContext abilityContext)
    {
        if (Cooldown < timer)
        {
            UseCritical(abilityContext);
            timer = 0.0f;
        }      
    }
    

    protected abstract void UseCritical(TapAbilityUseContext abilityContext);
    public override sealed float Cooldown
    {
        get { return 0; }
    }
}

