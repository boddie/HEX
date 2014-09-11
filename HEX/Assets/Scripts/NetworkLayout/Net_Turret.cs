using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.NetworkLayout
{
    class Net_Turret : INet_Block
    {
        const float cooldown = 3; //cooldown in s
        float curTime = 3;
        TurretAbility t;
        protected override void Start()
        {
            base.Start();
            if (photonView.isMine)
            {
                t = new TurretAbility(base.Mat);
            }
        }
        protected override void Update()
        {
            base.Update();
            if (photonView.isMine)
            {
                t.Update();
                Vector3? closest = null;
                float distance = 100;
                foreach (var opponent in manager.Opponents)
                {
                    float newlength = (transform.position - opponent.Value.Position).magnitude;
                    if (distance > newlength)
                    {
                        distance = newlength;
                        closest = opponent.Value.Position;
                    }
                }
                if (distance < 100)
                {
                    Fire(closest);
                }
            }
            curTime += Time.deltaTime;
        }

        public void Fire(Vector3? atPosition)
        {
            if (atPosition == null) return;          
            if (curTime >= cooldown)
            {
                Debug.Log("Firing");
                var context = new TapAbilityUseContext(new TapGesture(Vector3.zero, atPosition.Value), manager.CurrentPlayer, transform.position);            
                t.Use(context);
                curTime = 0;
            }
        }
    }
}
