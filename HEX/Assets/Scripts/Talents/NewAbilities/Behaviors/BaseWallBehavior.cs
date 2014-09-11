using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

abstract class BaseWallBehavior : BaseAbilityBehavior
{
    private double _duration;
    private double curTime;
    public BaseWallBehavior(double duration)
    {
        _duration = duration;
        curTime = 0.0;
    }
    public abstract override void OnCollisionEnter(UnityEngine.Collision collision);
    public override void Update()
    {
        if (PhotonView.isMine)
        {
            curTime += Time.deltaTime;
            if (curTime > _duration)
            {
                PhotonNetwork.Destroy(GameObject);
            }
        }
    }
}

