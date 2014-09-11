using System.Collections.Generic;
using Assets.Buffs;

public enum AbilityTarget { All, Self, Others }

public interface IAbility
{
    float Cooldown { get; }
    AbilityTarget AbilityTarget { get; }
    List<PEffect> CollisionEffects { get; }
}
