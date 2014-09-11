using UnityEngine;

public interface IAbilityBehavior
{
    void Initialize(NetworkAbility networkAbility);

    void OnCollisionEnter(Collision collision);
    void OnTriggerEnter(Collider collider);
    void OnTriggerExit(Collider collider);

    void Update();
}
