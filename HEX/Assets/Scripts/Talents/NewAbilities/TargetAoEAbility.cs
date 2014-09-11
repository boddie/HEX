using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

// Alright... AoEAbility:

// The AbilityBehavior is executed on the NetworkAbility.
// So, it's executed on each machine. Randomness is kinda out of the question unless it's pregenerated...which could work.

public class DefaultTargetAoeAbilityBehavior : BaseAbilityBehavior
{
    private float _radius;
    private int _damage;
    private float _duration;

    private float _creationTime;

    public DefaultTargetAoeAbilityBehavior(float radius, int damage, float duration)
    {
        _damage = (int) Math.Round(damage * PersistentData.Instance.CurrentPlayer.SpellPower);
        _radius = radius;
        _duration = duration;
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        _creationTime = Time.time;
        foreach (Collider collider in Physics.OverlapSphere(GameObject.transform.position, _radius))
        {
            if (collider.tag == "NetPlayer")
            {
                NetworkController.giveDamage(PhotonView.owner.name, collider.gameObject.GetPhotonView().name, _damage);
            }
        }
    }

    public override void OnCollisionEnter(Collision collision)
    {
    }

    public override void Update()
    {
        if (Time.time - _creationTime > _duration)
        {
            PhotonNetwork.Destroy(GameObject);
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
    }
}

public abstract class TargetAoEAbility : BaseSwipeAbility
{
    protected abstract float Radius
    {
        get;
    }

    protected abstract int Damage
    {
        get;
    }

    protected abstract float Duration
    {
        get;
    }

    public override IAbilityBehavior CreateAbilityBehavior()
    {
        return new DefaultTargetAoeAbilityBehavior(Radius, Damage, Duration);
    }

    protected override void UseCritical(SwipeAbilityUseContext abilityContext)
    {
        Vector3 location = (abilityContext.SwipeGesture.StartWorldPoint + abilityContext.SwipeGesture.EndWorldPoint) / 2;

        NetworkInstantiate(location, Quaternion.identity, new NetworkInstantiationData(AbilityDatabase.Instance.GetIdentifier(this), abilityContext.CastingPlayer.SpellPower));
    }
}

