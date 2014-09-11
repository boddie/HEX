using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class SpeedOfWindSwipe : BaseSwipeBuffAbility
{
    public const int JumpDistance = 50;
    protected override void UseCritical(SwipeAbilityUseContext abilityContext)
    {
        PersistentData p = GameObject.Find("Persistence").GetComponent<PersistentData>();
        Vector3 direction = (abilityContext.SwipeGesture.EndWorldPoint - abilityContext.SwipeGesture.StartWorldPoint).normalized;
        direction *= JumpDistance;
        p.CurrentPlayer.ChangePositionTo = p.CurrentPlayer.Position + direction;
    }
    public override Assets.Elements.IElement Element
    {
        get { return Assets.Elements.Wind.Get; }
    }
    public override float Cooldown
    {
        get { return 10.0f; }
    }
}

