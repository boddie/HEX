using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.NetworkLayout
{
    class Net_Flag : INet_Block
    {
        double intensity;
        protected override void Start()
        {
            base.Start();
            if (photonView.isMine)
            {
                intensity = (double)Mat.Health / 500;
            }
        }
        protected override void NetInit()
        {
            base.NetInit();
            intensity = (double)Mat.Health / 500;
        }
        protected override void Update()
        {
            base.Update();
            if (photonView.isMine)
            {
                if ((manager.CurrentPlayer.Position - transform.position).magnitude < 100.0f)
                {
                    manager.giveEffect(manager.CurrentPlayer.Alias, manager.CurrentPlayer.Alias, new Buffs.FlagBuff(intensity).Serialize(), intensity);
                }
               
            }
            //manager.CurrentPlayer.BuffManager.AddBuff(
        }
    }
}
