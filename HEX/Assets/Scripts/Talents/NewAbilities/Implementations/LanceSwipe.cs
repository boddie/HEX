using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Elements;
using Assets.Buffs;

class LanceSwipe : MissileSwipeAbility
{
    protected override void UseCritical(SwipeAbilityUseContext context)
    {
        Debug.Log("Go Lancelot");
        base.UseCritical(context);
    }
    public override List<PEffect> CollisionEffects
    {
        get { return null; }
    }
    public override AbilityTarget AbilityTarget
    {
        get { return AbilityTarget.Others; }
    }
    public override IAbilityBehavior CreateAbilityBehavior()
    {
        return new LanceAbilityBehavior(Damage, Range, Speed);
    }
    protected override int Damage
    {
        get { return 75; }
    }
    public override Assets.Elements.IElement Element
    {
        get { return Assets.Elements.Metal.Get; }
    }
    public override string PrefabName
    {
        get { return "LanceSwipe"; }
    }
    protected override float Range
    {
        get { return 500; }
    }
    protected override float SpawnPointOffsetDistance
    {
        get { return 10; }
    }
    protected override float SpawnPointVerticalOffset
    {
        get { return 3.5f; }
    }
    protected override float Speed
    {
        get { return 200; }
    }
    public override float Cooldown
    {
        get { return 10.0f; }
    }
}
class LanceAbilityBehavior : DefaultMissileBehavior
{
    float timer;
    float timeSoFar = 0;
    public LanceAbilityBehavior(int damage, float range, float speed)
        : base(speed, range, damage)
    {
        timer = 2;
    }
    public override void Update()
    {
        if (timeSoFar < timer)
        {
            timeSoFar += Time.deltaTime;
        }
        else
        {
            base.Update();
        }
    }
}

