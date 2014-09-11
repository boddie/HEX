using System;
using UnityEngine;

public interface INetworkController
{
    void giveCitadelDamage(string player, int viewID, int damage, string attackingPlayer, global::Assets.Elements.IElement element);
    void giveDamage(string source, string destination, int damage);
    void giveEffect(string actor, string target, int effectIdentifier, double sp);
    void respawned();
    void sendAltarHealthUpdate(int newHealth);
}

