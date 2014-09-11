using UnityEngine;
using Assets.Elements;
using Assets.Buffs;
using System.Collections.Generic;
using System;

public class DefaultMissileBehavior : BaseAbilityBehavior
{
    private float _speed;
    private float _range;
    protected int _damage;
    protected double _spellpower;
    protected Vector3 _castLoc;
    protected List<PEffect> _effects;

    private float _distanceTraveled;

    public DefaultMissileBehavior(float speed, float range, int damage, List<PEffect> effects = null)
    {
        _speed = speed;
        _range = range;
        _spellpower = PersistentData.Instance.CurrentPlayer.SpellPower;
        _damage = (int) Math.Round(damage * _spellpower);
        _effects = effects ?? new List<PEffect>();
    }

    public override void OnCollisionEnter(Collision collision)
    {
        if (PhotonView.isMine)
        {
            if (collision.gameObject.tag != "Ability")
            {
                Debug.Log("Destroying a GameObject because it collided with : " + collision.gameObject.tag);
                PhotonNetwork.Destroy(GameObject);
            }
            if (collision.gameObject.tag == "NetPlayer")
            {
                foreach (PEffect p in _effects)
                {
                    NetworkController.giveEffect(PhotonView.owner.name, collision.gameObject.GetPhotonView().owner.name, p.Serialize(), _spellpower);
                }
                NetworkController.giveDamage(PhotonView.owner.name, collision.gameObject.GetPhotonView().owner.name, _damage);
            }
            if ((collision.gameObject.tag == "Block" || collision.gameObject.tag == "Altar") && !collision.gameObject.GetPhotonView().isMine)
            {
                NetworkController.giveCitadelDamage(collision.gameObject.GetPhotonView().owner.name, collision.gameObject.GetPhotonView().viewID, (int)Math.Round(_damage * _spellpower), GameObject.GetPhotonView().owner.name, Fire.Get);
            }
        }
    }
    public override void Update()
    {
        GameObject.transform.Translate(GameObject.transform.forward * Time.deltaTime * _speed, Space.World);

        if (PhotonView.isMine)
        {
            _distanceTraveled += Time.deltaTime * _speed;
            if (_distanceTraveled >= _range)
            {
                PhotonNetwork.Destroy(GameObject);
            }
        }
    }
    public override void OnTriggerEnter(Collider other)
    {
    }
}
public class SwipeMissileBehavior : DefaultMissileBehavior
{
    public SwipeMissileBehavior(float speed, float range, int damage)
        : base(speed, range, damage)
    {
    }
    public override void OnCollisionEnter(Collision collision)
    {
        if (PhotonView.isMine)
        {
            if (collision.gameObject.tag != "Ability")
            {
                Debug.Log("Destroying a GameObject because it collided with : " + collision.gameObject.tag);
                PhotonNetwork.Destroy(GameObject);
            }
            if (collision.gameObject.tag == "NetPlayer")
            {
                foreach (PEffect p in _effects)
                {
                    NetworkController.giveEffect(PhotonView.owner.name, collision.gameObject.GetPhotonView().owner.name, p.Serialize(), _spellpower);
                }
                NetworkController.giveDamage(PhotonView.owner.name, collision.gameObject.GetPhotonView().owner.name, _damage);
            }
        }
    }
}