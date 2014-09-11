using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Buffs;

class NWSwipe : BaseSwipeAbility
{
    public override List<PEffect> CollisionEffects
    {
        get { return null; }
    }
    public override AbilityTarget AbilityTarget
    {
        get { return AbilityTarget.All; }
    }
    public override float Cooldown
    {
        get { return 5.0f; }
    }
    public override Assets.Elements.IElement Element
    {
        get { return Assets.Elements.Nature.Get; }
    }
    public override IAbilityBehavior CreateAbilityBehavior()
    {
        return new FallingObjectBehavior(50, 50.0f, 200.0f);
    }
    public override string PrefabName
    {
        get { return "NWSwipe"; }
    }
    protected override void UseCritical(SwipeAbilityUseContext abilityContext)
    {
        //make 6 spikes along the swipe trajectory
        int numSpikes = 6;
        Vector3 startloc = abilityContext.SwipeGesture.StartWorldPoint;
        Vector3 endloc = abilityContext.SwipeGesture.EndWorldPoint;
        Vector3 traj = startloc - endloc;
        Debug.Log(startloc);
        Debug.Log(endloc);
        Debug.Log(traj);
        Vector3 step = traj / numSpikes;
        Debug.Log(step);
        Vector3 instantLoc = new Vector3(startloc.x, startloc.y + 10, startloc.z);
        Vector3 yOffset = new Vector3(0, 10, 0);
        for (int i = 0; i < numSpikes; i++)
        {
            NetworkInstantiate(instantLoc, Quaternion.Euler(90.0f, 0, 0), new NetworkInstantiationData(AbilityDatabase.Instance.GetIdentifier(this), abilityContext.CastingPlayer.SpellPower));
            instantLoc -= step;
            instantLoc += yOffset;
        }
    }
}
class FallingObjectBehavior : BaseAbilityBehavior
{
    private int _damage;
    private float _speed;
    private float _height;
    private float _distanceTraveled;
    double _spellpower;
    public FallingObjectBehavior(int damage, float speed, float height)
    {
        _spellpower = PersistentData.Instance.CurrentPlayer.SpellPower;
        Debug.Log(_spellpower);
        Debug.Log(damage);
        _damage = (int) Math.Round(_spellpower * damage);
        Debug.Log(_damage);
        _speed = speed;
        _height = height;
    }
    public override void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (PhotonView.isMine)
        {
            if (collision.gameObject.tag != "Ability")
            {
                PhotonNetwork.Destroy(GameObject);
            }
            if (collision.gameObject.tag == "NetPlayer")
            {
                NetworkController.giveDamage(PhotonView.owner.name, collision.gameObject.GetPhotonView().owner.name, _damage);
            }
            //if ((collision.gameObject.tag == "Block" || collision.gameObject.tag == "Altar") && !collision.gameObject.GetPhotonView().isMine)
            //{
            //    NetworkController.giveCitadelDamage(collision.gameObject.GetPhotonView().owner.name, collision.gameObject.GetPhotonView().viewID, _damage, GameObject.GetPhotonView().owner.name, Assets.Elements.Nature.Get);
            //}
        }
    }
    protected override void OnInitialize()
    {
        base.OnInitialize();
    }
    public override void OnTriggerEnter(UnityEngine.Collider other)
    {
        base.OnTriggerEnter(other);
    }
    public override void OnTriggerExit(UnityEngine.Collider other)
    {
        base.OnTriggerExit(other);
    }
    public override void Update()
    {
        GameObject.transform.Translate(GameObject.transform.forward * Time.deltaTime * _speed, Space.World);

        if (PhotonView.isMine)
        {
            _distanceTraveled += Time.deltaTime * _speed;
            if (_distanceTraveled >= _height)
            {
                PhotonNetwork.Destroy(GameObject);
            }
        }
    }
}

