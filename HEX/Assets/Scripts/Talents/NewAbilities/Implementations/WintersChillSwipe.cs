using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Buffs;
using Assets.Elements;


class WintersChillSwipe : BaseSwipeAbility
{
    protected override void UseCritical(SwipeAbilityUseContext abilityContext)
    {
        Vector3 startLoc = abilityContext.SwipeGesture.StartWorldPoint;
        Vector3 endLoc = abilityContext.SwipeGesture.EndWorldPoint;
        Quaternion rotation = Quaternion.LookRotation(endLoc - startLoc);
        NetworkInstantiate(startLoc, rotation, new NetworkInstantiationData(AbilityDatabase.Instance.GetIdentifier(this), abilityContext.CastingPlayer.SpellPower));
    }
    public override AbilityTarget AbilityTarget
    {
        get { return AbilityTarget.All; }
    }
    public override List<PEffect> CollisionEffects
    {
        get { return new List<PEffect>() { new Frozen() }; }
    }
    public override string PrefabName
    {
        get { return "WintersChillSwipe"; }
    }
    public override float Cooldown
    {
        get { return 20.0f; }
    }
    public override IElement Element
    {
        get { return Ice.Get; }
    }
    public override IAbilityBehavior CreateAbilityBehavior()
    {
        return new WintersChillBehavior();
    }
}
class WintersChillBehavior : BaseAbilityBehavior
{
    const float scaling = 25;
    double timer = 2;
    double curTime = 0;
    public override void Update()
    {
        curTime += Time.deltaTime;
        if (curTime > timer)
        {
            PhotonNetwork.Destroy(GameObject);
        }
        //does nothing but dies after it blows it's load
    }
    public override void OnCollisionEnter(Collision collision)
    {
        //basically I want to look at the rotation of the gameobject, and then use that to project the vector it will knock them back along
        if (collision.gameObject.tag == "NetPlayer")
        {
            NetworkController.giveEffect(PhotonView.owner.name, collision.gameObject.GetPhotonView().owner.name, new Frozen().Serialize(), 0.0);
        }
    }
}

