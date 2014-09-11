using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Assets.Elements;
using Assets.Buffs;

class LanceTap : MissileAbility
{
    protected override void UseCritical(TapAbilityUseContext context)
    {
        Vector3 tapPoint = context.TapGesture.TapWorldPoint;
        tapPoint.y = 0;
        Vector3 casterPosition = context.CasterPosition;
        casterPosition.y = 0;
        Vector3 direction = (tapPoint - casterPosition).normalized;

        int numSpikes = 6;


        //want 45 degrees in either direction of look rotation
        List<Quaternion> rotations = new List<Quaternion>();
        var quat = Quaternion.LookRotation(direction);
        for (int i = 0; i < numSpikes; i++)
        {
            //so basically multiply it by quats corresponding to quat(-45 + 15 * i) 
            rotations.Add(quat * Quaternion.AngleAxis(-45.0f + 15.0f * i, Vector3.up));
        }
        Vector3 position = context.CasterPosition + direction * SpawnPointOffsetDistance + Vector3.up * SpawnPointVerticalOffset;

        foreach (var rotation in rotations)
        {
            NetworkInstantiate(position, rotation, new NetworkInstantiationData(AbilityDatabase.Instance.GetIdentifier(this), context.CastingPlayer.SpellPower));
        }
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
        return base.CreateAbilityBehavior();
    }
    protected override int Damage
    {
        get { return 20; }
    }
    public override Assets.Elements.IElement Element
    {
        get { return Assets.Elements.Metal.Get; }
    }
    public override string PrefabName
    {
        get { return "LanceTap"; }
    }
    protected override float Range
    {
        get { return 25; }
    }
    protected override float SpawnPointVerticalOffset
    {
        get { return 3.5f; }
    }
    protected override float SpawnPointOffsetDistance
    {
        get { return 7; }
    }
    protected override float Speed
    {
        get { return 100; }
    }
}

