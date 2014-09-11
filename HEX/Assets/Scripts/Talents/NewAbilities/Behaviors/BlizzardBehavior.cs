using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class BlizzardBehavior : BaseAbilityBehavior
{
    private int _damage;
    private float _radius;
    private float _duration;
    private float _creationTime;
    private float _lastTick;

    private List<PhotonView> _playersToDamage = new List<PhotonView>();

    public BlizzardBehavior(int damagePerTick, float radius, float duration)
    {
        _damage = (int)Math.Round(damagePerTick * PersistentData.Instance.CurrentPlayer.SpellPower);
        _radius = radius;
        _duration = duration;
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();

        _creationTime = Time.time;
    }

    public override void OnTriggerEnter(Collider other)
    {

        Debug.Log("ENTERED BLIZZARD");

        if (PhotonView.isMine)
        {
            if (other.tag == "NetPlayer")
            {
                _playersToDamage.Add(other.gameObject.GetPhotonView());
            }
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        if (PhotonView.isMine)
        {
            if (other.tag == "NetPlayer")
            {
                _playersToDamage.Remove(other.gameObject.GetPhotonView());
            }
        }
    }

    public override void Update()
    {
        if (PhotonView.isMine)
        {
            if (Time.time - _creationTime > _duration)
            {
                PhotonNetwork.Destroy(GameObject);
            }

            if (Time.time - _lastTick > 0.5f)
            {
                _lastTick = Time.time;
                foreach (PhotonView other in _playersToDamage)
                {
                    float distance = Vector3.Distance(other.gameObject.transform.position, GameObject.transform.position);

                    // Quadratic dropoff by distance
                    distance = Mathf.Clamp(_radius - distance, 0, _radius) / _radius;
                    distance *= distance;
                    int damage = (int)(0.5f * _damage * distance);
                    Debug.Log("Giving blizz dmg");
                    NetworkController.giveDamage(PhotonView.owner.name, other.owner.name, damage);
                }
            }
        }
    }
}
