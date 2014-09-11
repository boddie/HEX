using UnityEngine;

public abstract class BaseAbilityBehavior : IAbilityBehavior
{
    protected PhotonView PhotonView
    {
        get;
        private set;
    }

    protected GameObject GameObject
    {
        get;
        private set;
    }

    protected INetworkController NetworkController
    {
        get;
        private set;
    }

    protected virtual void OnInitialize() { }

    public void Initialize(NetworkAbility networkAbility)
    {
        NetworkController = PersistentData.Instance;
        GameObject = networkAbility.gameObject;
        PhotonView = networkAbility.photonView;
        OnInitialize();
    }

    public virtual void OnCollisionEnter(UnityEngine.Collision collision) { }

    public virtual void OnTriggerEnter(UnityEngine.Collider other) { }

    public virtual void OnTriggerExit(Collider other) { }

    public abstract void Update();
}

