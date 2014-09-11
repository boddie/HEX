using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Buffs;
using Assets.Elements;
using UnityEngine;

class DrownSwipe : MissileSwipeAbility
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
        get { return 0; }
    }
    public override IAbilityBehavior CreateAbilityBehavior()
    {
        return new DrownSwipeBehavior(Speed, Range, Damage);
    }
    public override IElement Element
    {
        get { return Water.Get; }
    }
    public override string PrefabName
    {
        get { return "DrownSwipe"; }
    }
    protected override int Damage
    {
        get { return 30; }
    }
    protected override float Range
    {
        get { return 50.0f; }
    }
    protected override float SpawnPointOffsetDistance
    {
        get { return 3.0f; ; }
    }
    protected override float SpawnPointVerticalOffset
    {
        get { return 0.0f; }
    }
    protected override float Speed
    {
        get { return 50.0f; }
    }
}
class DrownSwipeBehavior : SwipeMissileBehavior
{
    public DrownSwipeBehavior(float speed, float range, int damage)
        : base(speed, range, damage)
    {
    }
    protected override void OnInitialize()
    {
        base.OnInitialize();
        //GameObject.transform.Rotate(new Vector3(0, 90.0f, 0.0f));
    }
    public override void OnCollisionEnter(Collision collision)
    {
        if (PhotonView.isMine)
        {
            if (collision.gameObject.tag != "Ability" && collision.gameObject.tag  != "NetPlayer" && collision.gameObject.tag != "Block")
            {
                Debug.Log("Destroying a GameObject because it collided with : " + collision.gameObject.tag);
                PhotonNetwork.Destroy(GameObject);
            }
            if (collision.gameObject.tag == "NetPlayer")
            {
                try
                {
                    if (collision.gameObject.GetPhotonView().owner.name != PersistentData.Instance.CurrentPlayer.Alias && PersistentData.Instance.Opponents[collision.gameObject.GetPhotonView().owner.name].Buffs.ContainsKey(typeof(Drowning)))
                    {
                        NetworkController.giveDamage(PhotonView.owner.name, collision.gameObject.GetPhotonView().owner.name, _damage * 2);
                    }
                    else if (collision.gameObject.GetPhotonView().owner.name == PersistentData.Instance.CurrentPlayer.Alias && PersistentData.Instance.CurrentPlayer.BuffManager.HasPEffect(new Drowning(0.0)))
                    {
                        NetworkController.giveDamage(PhotonView.owner.name, collision.gameObject.GetPhotonView().owner.name, _damage * 2);
                    }
                    else
                    {
                        NetworkController.giveDamage(PhotonView.owner.name, collision.gameObject.GetPhotonView().owner.name, _damage);
                    }
                }
                catch (Exception e)
                {
                }
            }
        }
    }
}

