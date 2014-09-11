using UnityEngine;
using System.Collections;
using Assets.NetworkLayout;

public class Net_Altar : INet_Block
{
    int previousHealth = -1;
    protected override void Update()
    {
        base.Update();
        if (photonView.isMine)
        {
            if (previousHealth != base.Mat.CurrentHealth && !manager.DEBUG)
            {
                if (base.Mat.CurrentHealth <= 0)
                {
                    base.manager.altarDown();
                }
                base.manager.sendAltarHealthUpdate(base.Mat.CurrentHealth);
            }
            previousHealth = base.Mat.CurrentHealth;
        }

    }
}
