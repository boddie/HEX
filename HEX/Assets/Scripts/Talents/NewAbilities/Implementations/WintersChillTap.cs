using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Elements;
using Assets.Buffs;
using UnityEngine;

class WintersChillTap : BaseTapAbility
{
    Vector3 casterLoc;
    Vector3 targetPosition;
    protected override void UseCritical(TapAbilityUseContext abilityContext)
    {
        Debug.Log("Ice Beam Use");
        var manager = GameObject.Find("Persistence").GetComponent<PersistentData>();
        casterLoc = abilityContext.CasterPosition;
        targetPosition = abilityContext.CasterPosition;
        if (!manager.CurrentPlayer.BuffManager.HasPEffect(new IceBeamUse()))
        {
            obj = NetworkInstantiate(abilityContext.CasterPosition + new Vector3(0, 2.5f, 0), Quaternion.identity, new NetworkInstantiationData(AbilityDatabase.Instance.GetIdentifier(this), abilityContext.CastingPlayer.SpellPower));
        }
        else
        {
            PhotonNetwork.Destroy(obj);
        }
        manager.giveEffect(manager.CurrentPlayer.Alias, manager.CurrentPlayer.Alias, new IceBeamUse().Serialize(), abilityContext.CastingPlayer.SpellPower);
    }
    public override string PrefabName
    {
        get { return "Beam"; }
    }
    public override IElement Element
    {
        get { return Ice.Get; }
    }
    public override IAbilityBehavior CreateAbilityBehavior()
    {
        Debug.Log("Registered caster location is: " + casterLoc);
        return new BeamBehavior(casterLoc, targetPosition);
    }
    public override List<PEffect> CollisionEffects
    {
        get { return new List<PEffect>() { new Chilled() }; }
    }
    public override AbilityTarget AbilityTarget
    {
        get { return AbilityTarget.Others; }
    }
}
class BeamBehavior : BaseAbilityBehavior
{
    Camera cam;
    Beam b;
    public BeamBehavior(Vector3 casterLoc, Vector3 initLoc)
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    protected override void OnInitialize()
    {
        base.OnInitialize();
        b = GameObject.GetComponent<Beam>();
        
    }
    const double duration = 5;
    double timer = 0;
    float range = 100;
    public override void Update()
    {
        if (PhotonView.isMine)
        {
            timer += Time.deltaTime;
            if (timer > duration)
            {
                PhotonNetwork.Destroy(GameObject);
            }
            Vector3 loc = MathHelper.GetWorldFromMousePos(cam, Input.mousePosition);
            loc = new Vector3(loc.x, b.transform.localPosition.y + 2.5f, loc.z);

            Vector3 dir = (loc - b.transform.localPosition).normalized;
            if ((loc - b.transform.localPosition).magnitude > range)
            {
                loc = b.transform.localPosition + range * dir;
            }
            b.TargetPosition = loc;
            PersistentData.Instance.SyncIceBeamPosition(PhotonView.owner.name, loc);

            float length = (loc - b.transform.localPosition).magnitude;

            foreach (var opponent in PersistentData.Instance.Opponents)
            {
                RaycastHit rch;
                if (Physics.Raycast(new Ray(b.transform.localPosition, dir), out rch, distance: length))
                {
                    if (rch.collider.gameObject.GetPhotonView() != null && rch.collider.gameObject.GetPhotonView().owner.name == opponent.Key && rch.collider.gameObject.tag == "NetPlayer")
                    {
                        b.TargetPosition = rch.transform.position + new Vector3(0, 2.5f, 0);
                        NetworkController.giveEffect(base.PhotonView.owner.name, opponent.Key, new Chilled().Serialize(), PersistentData.Instance.CurrentPlayer.SpellPower);
                    }
                }
            }
        }
        else
        {
            b.TargetPosition = PersistentData.Instance.Opponents[PhotonView.owner.name].IceBeamInfo;
        }
    }
}

