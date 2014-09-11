using UnityEngine;

public abstract class MissileAbility : BaseTapAbility
{
    protected abstract int Damage { get; }
    protected abstract float Speed { get; }
    protected abstract float Range { get; }
    protected abstract float SpawnPointOffsetDistance { get; }
    protected abstract float SpawnPointVerticalOffset { get; }

    public override IAbilityBehavior CreateAbilityBehavior()
    {
        return new DefaultMissileBehavior(Speed, Range, Damage);
    }

    protected override void UseCritical(TapAbilityUseContext context)
    {
        Vector3 tapPoint = context.TapGesture.TapWorldPoint;
        tapPoint.y = 0;
        Vector3 casterPosition = context.CasterPosition;
        casterPosition.y = 0;
        Vector3 direction = (tapPoint - casterPosition).normalized;

        Quaternion rotation = Quaternion.LookRotation(direction);
        Vector3 position = context.CasterPosition + direction * SpawnPointOffsetDistance + Vector3.up * SpawnPointVerticalOffset;

        NetworkInstantiate(position, rotation, new NetworkInstantiationData(AbilityDatabase.Instance.GetIdentifier(this), context.CastingPlayer.SpellPower));
    }
}
public abstract class MissileSwipeAbility : BaseSwipeAbility
{
    protected abstract int Damage { get; }
    protected abstract float Speed { get; }
    protected abstract float Range { get; }
    protected abstract float SpawnPointOffsetDistance { get; }
    protected abstract float SpawnPointVerticalOffset { get; }

    public override IAbilityBehavior CreateAbilityBehavior()
    {
        return new SwipeMissileBehavior(Speed, Range, Damage);
    }

    protected override void UseCritical(SwipeAbilityUseContext context)
    {
        Vector3 tapPoint = context.SwipeGesture.EndWorldPoint;
        tapPoint.y = 0;
        Vector3 casterPosition = context.SwipeGesture.StartWorldPoint;
        casterPosition.y = 0;
        Vector3 direction = (tapPoint - casterPosition).normalized;

        Quaternion rotation = Quaternion.LookRotation(direction);
        Vector3 position = casterPosition + direction * SpawnPointOffsetDistance + Vector3.up * SpawnPointVerticalOffset;
        position.y += context.CasterPosition.y;

        NetworkInstantiate(position, rotation, new NetworkInstantiationData(AbilityDatabase.Instance.GetIdentifier(this), context.CastingPlayer.SpellPower));
        Debug.Log("instantiation Complete");
    }
}