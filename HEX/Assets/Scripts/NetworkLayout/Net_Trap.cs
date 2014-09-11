using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.CitadelBuilder;

namespace Assets.NetworkLayout
{
    class Net_Trap : INet_Block
    {
        float resetCounter;
        const float timer = 30; //cd in s
        public Net_Trap()
            : base()
        {
            resetCounter = timer;
        }
        void OnCollisionEnter(Collision collision)
        {
            Debug.Log("Trap Collision Registered");
            if (photonView.isMine)
            {
                if (resetCounter >= timer && collision.gameObject.tag == "NetPlayer" && !collision.gameObject.GetPhotonView().isMine)
                {
                    #region
                    if (base.Mat.GetType() == typeof(AirMaterial))
                    {

                    }
                    else if (base.Mat.GetType() == typeof(SteelMaterial))
                    {

                    }
                    else if (base.Mat.GetType() == typeof(FireMaterial))
                    {

                    }
                    else if (base.Mat.GetType() == typeof(ObsidianMaterial))
                    {

                    }
                    else if (base.Mat.GetType() == typeof(StoneMaterial))
                    {

                    }
                    else if (base.Mat.GetType() == typeof(WaterMaterial))
                    {

                    }
                    else if (base.Mat.GetType() == typeof(WoodMaterial))
                    {

                    }
                    else if (base.Mat.GetType() == typeof(ElecMaterial))
                    {

                    }
                    else if (base.Mat.GetType() == typeof(LearnerMaterial))
                    {

                    }
                    else if (base.Mat.GetType() == typeof(LinkerMaterial))
                    {

                    }
                    else if (base.Mat.GetType() == typeof(MirrorMaterial))
                    {

                    }
                    else if (base.Mat.GetType() == typeof(IceMaterial))
                    {

                    }
                    else if (base.Mat.GetType() == typeof(AltarMaterial))
                    {

                    }
                    else
                    {
                        throw new Exception("Add this type to this queer ass pretend switch statement dickfuck");
                    }
                    #endregion
                    //Debug.Log(collision.gameObject.GetPhotonView().owner.name + ability.Damage.ToString());
                    manager.giveDamage(manager.CurrentPlayer.Alias, collision.gameObject.GetPhotonView().owner.name, base.Mat.Health / 2);
                    resetCounter = 0;
                }
            }
        }
        protected override void Update()
        {
            resetCounter += Time.deltaTime;
            if (!photonView.isMine && PhotonNetwork.room != null)
            {
                gameObject.renderer.enabled = false;
            }
        }
    }
}
