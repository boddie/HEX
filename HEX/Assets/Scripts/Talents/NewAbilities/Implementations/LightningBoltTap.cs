using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Buffs;
using Assets.Elements;

class LightningBoltTap : BaseTapAbility
{
    public override AbilityTarget AbilityTarget
    {
        get { return AbilityTarget.Others; }
    }
    public override List<PEffect> CollisionEffects
    {
        get { return null; ; }
    }
    public override IElement Element
    {
        get { return Electricity.Get; }
    }
    public override IAbilityBehavior CreateAbilityBehavior()
    {
        return new LightningBoltTapBehavior();
    }
    public override string PrefabName
    {
        get { return "Lightning"; }
    }
    protected override void UseCritical(TapAbilityUseContext abilityContext)
    {
        NetworkInstantiate(abilityContext.TapGesture.TapWorldPoint + new Vector3(0, 30, 0), Quaternion.AngleAxis(90.0f, Vector3.forward), new NetworkInstantiationData(AbilityDatabase.Instance.GetIdentifier(this), abilityContext.CastingPlayer.SpellPower));
    }
}
class LightningBoltTapBehavior : BaseAbilityBehavior
{
    const int damage = 5;
    Lightning b;
    float timeSoFar = 0.0f;
    float timer;
    float duration = 1.0f;
    float durTime = 0.0f;
    protected override void OnInitialize()
    {
        base.OnInitialize();
        b = GameObject.GetComponent<Lightning>();
        b.TargetPosition = GameObject.transform.localPosition;
        timer = 2.0f;

    }
    public override void Update()
    {
        if (timeSoFar < timer)
        {
            timeSoFar += Time.deltaTime;
        }
        else
        {
            durTime += Time.deltaTime;
            if (durTime > duration)
            {
                PhotonNetwork.Destroy(GameObject);
                return;
            }
            RaycastHit rch;
            if (Physics.Raycast(new Ray(b.transform.localPosition, new Vector3(0, -1, 0)), out rch, distance: 40))
            {
                b.TargetPosition = rch.point;
                if (PhotonView.isMine)
                {
                    foreach (var opponent in PersistentData.Instance.Opponents)
                    {
                        if (rch.collider.gameObject.GetPhotonView() != null && rch.collider.gameObject.GetPhotonView().owner.name == opponent.Key)
                        {
                            //whoop direct hit
                            if (rch.collider.gameObject.tag == "NetPlayer")
                            {
                                NetworkController.giveDamage(PhotonView.owner.name, opponent.Key, damage * 2);
                            }
                            else if (rch.collider.gameObject.tag == "Block") 
                            {
                                NetworkController.giveCitadelDamage(PhotonView.owner.name, rch.collider.gameObject.GetPhotonView().viewID, damage * 2, GameObject.GetPhotonView().name, Electricity.Get); 
                            }
                            b.TargetPosition = opponent.Value.Position;
                        }
                        else if ((opponent.Value.Position - b.TargetPosition).magnitude < 15)
                        {
                            if (rch.collider.gameObject.tag == "NetPlayer")
                            {
                                NetworkController.giveDamage(PhotonView.owner.name, opponent.Key, damage);
                            }
                            else if (rch.collider.gameObject.tag == "Block")
                            {
                                NetworkController.giveCitadelDamage(PhotonView.owner.name, rch.collider.gameObject.GetPhotonView().viewID, damage, GameObject.GetPhotonView().name, Electricity.Get);
                            }
                            b.TargetPosition = opponent.Value.Position;
                        }
                    }
                }
            }
            else
            {
                throw new Exception("The lightning bolt hit nothing. Literally nothing. Ass");
            }
        }
        
    }
}

