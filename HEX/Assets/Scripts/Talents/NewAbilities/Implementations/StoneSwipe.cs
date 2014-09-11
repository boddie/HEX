using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Buffs;

class StoneSwipe : BaseSwipeAbility
{
    protected override void UseCritical(SwipeAbilityUseContext abilityContext)
    {
        Vector3 startLoc = abilityContext.SwipeGesture.StartWorldPoint;
        Vector3 endLoc = abilityContext.SwipeGesture.EndWorldPoint;
        Quaternion rotation = Quaternion.LookRotation(endLoc - startLoc) * Quaternion.AngleAxis(90.0f, Vector3.up);
        NetworkInstantiate(startLoc, rotation, new NetworkInstantiationData(AbilityDatabase.Instance.GetIdentifier(this), abilityContext.CastingPlayer.SpellPower));
    }
    public override string PrefabName
    {
        get { return "StoneSwipe"; }
    }
    public override float Cooldown
    {
        get { return 0.0f; }
    }
    public override Assets.Elements.IElement Element
    {
        get { return Assets.Elements.Earth.Get; }
    }
    public override IAbilityBehavior CreateAbilityBehavior()
    {
        return new StoneWallBehavior();
    }
    public override List<PEffect> CollisionEffects
    {
        get { return null; }
    }
    public override AbilityTarget AbilityTarget
    {
        get { return AbilityTarget.All; }
    }
}
class StoneWallBehavior : BaseWallBehavior
{
    public StoneWallBehavior()
        : base(10)
    {
    }
    public override void OnCollisionEnter(UnityEngine.Collision collision)
    {
        //pretty much is just a rigidbody
    }
}

