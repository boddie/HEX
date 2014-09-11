using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Buffs;
using Assets.Elements;


class LightningBoltSwipe : BaseSwipeAbility
{
    const float range = 100;
    public override AbilityTarget AbilityTarget
    {
        get { return AbilityTarget.Others; }
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
        return new LightningBoltSwipeBehavior();
    }
    public override IElement Element
    {
        get { return Electricity.Get; }
    }
    public override string PrefabName
    {
        get { return "Lightning"; }
    }
    protected override void UseCritical(SwipeAbilityUseContext abilityContext)
    {
        Vector3 startSwipe = abilityContext.SwipeGesture.StartWorldPoint;
        Vector3 endSwipe = abilityContext.SwipeGesture.EndWorldPoint;

        RaycastHit rch;
        HashSet<String> hitPlayers = new HashSet<string>();
        hitPlayers.Add(PersistentData.Instance.CurrentPlayer.Name);
            
        Vector3 startLoc = abilityContext.CasterPosition;
        Vector3 endPos = startLoc - (startSwipe - endSwipe); //this is to have the direction be the same as the swipe.

        Debug.Log(startLoc);
        Debug.Log(endPos - startLoc);

        
        Vector3 dir = (endPos - startLoc).normalized;
        Vector3 origin = startLoc + dir*5;
        string playerName = "";
        if (Physics.Raycast(new Ray(origin, dir), out rch, range))
        {            
            endPos = rch.collider.gameObject.transform.position;
            if (rch.collider.gameObject.GetPhotonView() != null && rch.collider.gameObject.tag == "NetPlayer")
            {
                playerName = rch.collider.gameObject.GetPhotonView().owner.name;
                Debug.Log("Hit " + playerName);
            }
            while ((startLoc - endPos).magnitude < range)
            {
                GameObject g = NetworkInstantiate(startLoc + new Vector3(0.0f, 3.0f, 0.0f), Quaternion.AngleAxis(90.0f, new Vector3(0.0f, 0.0f, 1.0f)), new NetworkInstantiationData(AbilityDatabase.Instance.GetIdentifier(this), abilityContext.CastingPlayer.SpellPower));
                var b = g.GetComponent<Lightning>();
                b.TargetPosition = endPos;
                if (playerName != "")
                {
                    //we want to do damage and add them to the list of hit targets
                    hitPlayers.Add(playerName);
                    PersistentData.Instance.giveDamage(PersistentData.Instance.CurrentPlayer.Name, playerName, 30);
                }
                else
                {
                    break;
                }
                playerName = "";
                startLoc = endPos;
                float closest = float.MaxValue;
                foreach (var player in PersistentData.Instance.Opponents)
                {
                    if ((player.Value.Position - origin).magnitude < closest && !hitPlayers.Contains(player.Key))
                    {
                        endPos = player.Value.Position;
                        playerName = player.Key;
                    }
                }
            }
        }
        else
        {
            Debug.Log("We hit phooey");
        }
       // NetworkInstantiate(abilityContext.CasterPosition, Quaternion.FromToRotation(startPosition, endPosition), new NetworkInstantiationData(AbilityDatabase.Instance.GetIdentifier(this), abilityContext.CastingPlayer.SpellPower));
        
    }
}
static class ExtensionMethods
{
    public static void AddRange<E>(this ICollection<E> sub, IEnumerable<E> list) { foreach (E entry in list) { sub.Add(entry); } }
}

class LightningBoltSwipeBehavior : BaseAbilityBehavior
{
    Lightning b;

    float timer = 0.5f;
    float currentTime = 0.0f;
    protected override void OnInitialize()
    {     
        b = GameObject.GetComponent<Lightning>();
        Debug.Log("Initializing Lightning");
        Debug.Log("Target position is " + b.TargetPosition);
        Debug.Log("Starting position is " + b.transform.position);
        base.OnInitialize();
    }
    public override void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > timer)
        {
            Debug.Log("Destroying lightning");
            PhotonNetwork.Destroy(GameObject);
        }
    }
}

