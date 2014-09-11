using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Elements;
using Assets.Buffs;
using UnityEngine;

class SteelyourselfSwipe : BaseSwipeAbility
{
    public override AbilityTarget AbilityTarget
    {
        get { return AbilityTarget.All; }
    }
    public override List<PEffect> CollisionEffects
    {
        get { return null; }
    }
    public override float Cooldown
    {
        get { return 10.0f; }
    }
    public override IAbilityBehavior CreateAbilityBehavior()
    {
        return new SteelyourselfSwipeBehavior();
    }
    public override IElement Element
    {
        get { return Metal.Get; }
    }
    public override string PrefabName
    {
        get { return "SteelYourselfSwipe"; }
    }
    protected override void UseCritical(SwipeAbilityUseContext abilityContext)
    {
        NetworkInstantiate((abilityContext.SwipeGesture.StartWorldPoint + abilityContext.SwipeGesture.EndWorldPoint) / 2, Quaternion.identity, new NetworkInstantiationData(AbilityDatabase.Instance.GetIdentifier(this), abilityContext.CastingPlayer.SpellPower));
    }
}
class SteelyourselfSwipeBehavior : BaseWallBehavior
{
    public SteelyourselfSwipeBehavior() : base(10)
    {
    }
    public override void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (PhotonView.isMine)
        {
            if (collision.gameObject.tag == "NetPlayer")
            {
                //i.e. is it yours
                if (PhotonView.owner.name == collision.gameObject.GetPhotonView().name)
                {
                    NetworkController.giveEffect(PhotonView.owner.name, PhotonView.owner.name, new Steel().Serialize(), 0.0); //they aren't using sp / positions in any way so ya
                }
                else
                {
                    NetworkController.giveEffect(PhotonView.owner.name, collision.gameObject.GetPhotonView().name, new Soft().Serialize(), 0.0);
                }
            }
        }
    }
}
