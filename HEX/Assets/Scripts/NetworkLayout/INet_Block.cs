using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.CitadelBuilder;
using UnityEngine;

namespace Assets.NetworkLayout
{
    public abstract class INet_Block : Photon.MonoBehaviour
    {
        public CitadelBuilder.CMaterial Mat { get; set; }
        protected PersistentData manager;
        public bool NetworkInit { get; set; }
        private bool _healthInit;

        protected virtual void Start()
        {
            manager = GameObject.Find("Persistence").GetComponent<PersistentData>();
            NetworkInit = false;
            _healthInit = false;
        }
        protected virtual void NetInit()
        {          
            NetworkInit = true;
            Mat = CitadelBuilder.CMaterial.Deserialize((byte[])photonView.instantiationData.FirstOrDefault());
            Debug.Log("Got data!");
            if (Mat.DiffuseColor != null) this.gameObject.renderer.material.color = Mat.DiffuseColor;
        }
        protected virtual void Update()
        {
            if (PhotonNetwork.room != null && !photonView.isMine && !NetworkInit)
            {
                NetInit();
            }
            if (photonView.isMine)
            {
                if (!_healthInit)
                {
                    _healthInit = true;
                    Mat.Init();
                }
                if (manager.Blocks[this.photonView.viewID].Damage != 0)
                {
                    Mat.PerformOnHitEffect(manager.Blocks[this.photonView.viewID].AttackingPlayer, manager.Blocks[this.photonView.viewID].Element, manager.Blocks[this.photonView.viewID].Damage);
                    manager.Blocks[this.photonView.viewID].Damage = 0;
                }
                if (Mat.Dead())
                {
                    Debug.Log("Destruction incoming");
                    PhotonNetwork.Destroy(this.gameObject);
                }
            }
        }
    }
}
