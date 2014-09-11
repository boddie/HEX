using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;
using System;
using Assets.Buffs;
using Assets.Elements;

public class NetworkInstantiationData
{
    public int AbilityIdentifier
    {
        get;
        set;
    }

    public double SpellPower
    {
        get;
        set;
    }

    public NetworkInstantiationData(int abilityIdentifier, double spellPower)
    {
        AbilityIdentifier = abilityIdentifier;
        SpellPower = spellPower;
    }

    public byte[] ConvertToBytes()
    {
        List<byte> bytes = new List<byte>(12);
        bytes.AddRange(BitConverter.GetBytes(AbilityIdentifier));
        bytes.AddRange(BitConverter.GetBytes(SpellPower));
        return bytes.ToArray();
    }

    public static NetworkInstantiationData GetFromBytes(byte[] data)
    {
        if (data.Length != 12)
        {
            Debug.LogError("Expecting 12 bytes for NetworkInstantiationData, but received " + data.Length);
            return null;
        }

        return new NetworkInstantiationData(BitConverter.ToInt32(data, 0), BitConverter.ToDouble(data, 4));
    }
}

public abstract class BaseAbility : IAbility
{
    public abstract IAbilityBehavior CreateAbilityBehavior();

    public abstract float Cooldown
    {
        get;
    }

    public abstract string PrefabName
    {
        get;
    }

    public abstract AbilityTarget AbilityTarget
    {
        get;
    }

    public abstract List<PEffect> CollisionEffects
    {
        get;
    }

    public abstract IElement Element
    {
        get;
    }
    protected double timer;
    protected GameObject obj;
    public BaseAbility()
    {
        timer = Cooldown;
    }
    public void Update()
    {
        timer += Time.deltaTime;
    }

    protected GameObject NetworkInstantiate(Vector3 position, Quaternion rotation, NetworkInstantiationData data)
    {
        return PhotonNetwork.Instantiate(PrefabName, position, rotation, 0, new object[] { data.ConvertToBytes() });
    }
}

