using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Buffs;
using UnityEngine;

class PBAoEBehavior : BaseAbilityBehavior
{
    private int _damage;
    private List<PEffect> _effects;
    private double _casterSP;
    private double _duration;
    private double _timer;
    public PBAoEBehavior(int damage, double duration, double spellpower, List<PEffect> effects = null)
    {
        _damage = (int) Math.Round(damage * spellpower);
        _duration = duration;
        _effects = effects;
        _casterSP = spellpower;
        _timer = 0;
    }
    public override void Update()
    {
        if (PhotonView.isMine)
        {
            //basically, a pbaoe ability fires it's shit and cleans up
            _timer += Time.deltaTime;
            if (_timer > _duration)
            {
                PhotonNetwork.Destroy(GameObject);
            }
        }
    }
    public override void OnCollisionEnter(Collision collision)
    {
        //if it ain't u sucka
        if (PhotonView.owner.name != collision.gameObject.GetPhotonView().name)
        {
            foreach (var effect in _effects)
            {
                NetworkController.giveEffect(PhotonView.owner.name, collision.gameObject.GetPhotonView().owner.name, effect.Serialize(), _casterSP);
            }
            NetworkController.giveDamage(PhotonView.owner.name, collision.gameObject.GetPhotonView().name, _damage);
        }
    }

}
abstract class PBAoEAbility : BaseTapAbility
{
    protected override void UseCritical(TapAbilityUseContext abilityContext)
    {
        NetworkInstantiate(abilityContext.CasterPosition, Quaternion.identity, new NetworkInstantiationData(AbilityDatabase.Instance.GetIdentifier(this), abilityContext.CastingPlayer.SpellPower));
    }
    public override AbilityTarget AbilityTarget
    {
        get { return AbilityTarget.Others; }
    }
    
}
