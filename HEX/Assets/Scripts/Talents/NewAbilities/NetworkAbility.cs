using UnityEngine;

public class NetworkAbility : Photon.MonoBehaviour
{
    private IAbilityBehavior _ability;

    void Awake()
    {
        NetworkInstantiationData data = NetworkInstantiationData.GetFromBytes((byte[])photonView.instantiationData[0]);
        int abilityType = data.AbilityIdentifier;
        _ability = AbilityDatabase.Instance.GetAbility(abilityType).CreateAbilityBehavior();

        _ability.Initialize(this);
    }

    void Update()
    {
        _ability.Update();
    }

    void OnCollisionEnter(Collision collision)
    {
        _ability.OnCollisionEnter(collision);
    }

    void OnTriggerEnter(Collider collider)
    {
        _ability.OnTriggerEnter(collider);
    }

    void OnTriggerExit(Collider collider)
    {
        _ability.OnTriggerExit(collider);
    }
}

