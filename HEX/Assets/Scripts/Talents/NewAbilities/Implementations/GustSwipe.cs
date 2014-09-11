using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Elements;
using Assets.Buffs;
using UnityEngine;

class GustSwipe : BaseSwipeAbility
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
        get { return "GustSwipe"; }
    }
    public override IElement Element
    {
        get { return Wind.Get; }
    }
    public override IAbilityBehavior CreateAbilityBehavior()
    {
        return new GustAbilityBehavior();
    }
    public override float Cooldown
    {
        get { return 5.0f; }
    }
    public override List<PEffect> CollisionEffects
    {
        get { return new List<PEffect>() { }; }
    }
    public override AbilityTarget AbilityTarget
    {
        get { return AbilityTarget.Others; }
    }
}
class GustAbilityBehavior : BaseAbilityBehavior
{
    const float scaling = 25;
    double timer = .5;
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
            Vector3 fire = -1 * base.GameObject.transform.forward;
            fire *= scaling;
            collision.gameObject.transform.Translate(fire);
        }

    }
}

