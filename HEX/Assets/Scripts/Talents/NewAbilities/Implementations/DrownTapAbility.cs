using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Elements;
using Assets.Buffs;
using UnityEngine;

class DrownTap : BaseTapAbility
{
    public override AbilityTarget AbilityTarget
    {
        get { return AbilityTarget.Others; }
    }
    public override List<PEffect> CollisionEffects
    {
        get { return null; }
    }
    public override IElement Element
    {
        get { return Water.Get; }
    }
    public override IAbilityBehavior CreateAbilityBehavior()
    {
        return new DrownTapAbilityBehavior();
    }
    public override string PrefabName
    {
        get { return ""; }
    }
    protected override void UseCritical(TapAbilityUseContext abilityContext)
    {
        Vector3 wp = abilityContext.TapGesture.TapWorldPoint;
        foreach (var player in PersistentData.Instance.Opponents)
        {
            if ((player.Value.Position - wp).magnitude < 10)
            {
                PersistentData.Instance.giveEffect(abilityContext.CastingPlayer.Alias, player.Key, new Drowning(abilityContext.CastingPlayer.SpellPower).Serialize(), abilityContext.CastingPlayer.SpellPower);
            }
        }
    }
}
class DrownTapAbilityBehavior : BaseAbilityBehavior
{
    //public override void OnCollisionEnter(UnityEngine.Collision collision)
    //{
    //    base.OnCollisionEnter(collision);
    //}
    //protected override void OnInitialize()
    //{
    //    base.OnInitialize();
    //}
    //public override void OnTriggerEnter(UnityEngine.Collider other)
    //{
    //    base.OnTriggerEnter(other);
    //}
    //public override void OnTriggerExit(UnityEngine.Collider other)
    //{
    //    base.OnTriggerExit(other);
    //}
    public override void Update()
    {
        //throw new NotImplementedException();
    }
}

