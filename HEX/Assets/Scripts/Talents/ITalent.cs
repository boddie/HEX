using UnityEngine;
using System;
using System.Collections.Generic;
using Assets;

public delegate void ModPlayer(Player p);

public class Talent
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Icon { get; private set; } //path
    public ModPlayer Unlock { get; private set; }
    public Talent(string name, string description, string icon, ModPlayer unlock)
    {
        Name = name;
        Description = description;
        Icon = icon;
        Unlock = unlock;
    }
}
public static class TalentDictionary
{
    public static Talent[][] Talents; //first number is level, second order is placement
    static TalentDictionary()
    {
        Talents = new Talent[10][];
        for (int i = 0; i < 10; i++)
        {
            Talents[i] = new Talent[3];
        }

        Talents[0][0] = new Talent("Fireball",
            "Tap: Fire a projectile causing 25 Fire damage.\nSwipe: Cause a firestorm of radius 10 at the location that lasts for 5 seconds and causes 5 Fire damage/second to anything in its radius.",
            "UI_Elements/AbilityIcons/Fireball",
            delegate(Player p) { UnlockPrimaryAbility(p, new AbilityPair(new FireballTap(), new FireballSwipe(), Talents[0][0].Name, Talents[0][0].Description, Talents[0][0].Icon )); });
        Talents[0][1] = new Talent("Nature's Wrath",
            "Tap: Fire a projectile causing 25 Nature damage.\nSwipe: Cause a line of falling wooden spikes causing 25 Nature damage to anyone who is hit by them.",
            "UI_Elements/AbilityIcons/NaturesWrath", 
            delegate(Player p) { UnlockPrimaryAbility(p, new AbilityPair(new NWTap(), new NWSwipe(), Talents[0][1].Name, Talents[0][1].Description, Talents[0][1].Icon)); });
        Talents[0][2] = new Talent("Lance",
            "Tap: Fire a cone of shards causing 25 Metal damage.\nSwipe: Fire a projectile in the direction after 2 seconds that causes 50 Metal damage.",
            "UI_Elements/AbilityIcons/Lance", 
            delegate(Player p) { UnlockPrimaryAbility(p, new AbilityPair(new LanceTap(), new LanceSwipe(), Talents[0][2].Name, Talents[0][2].Description, Talents[0][2].Icon)); });
        Talents[1][0] = new Talent("Winter's Chill",
            "Tap: Fire a beam that does 5 Ice damage/s and slows by 50% for as long as its contact.\nSwipe: Fire a cone of ice that immobilizes the target for 3 seconds.",
            "UI_Elements/AbilityIcons/WintersChill",
            delegate(Player p) { UnlockSecondaryAbility(p, new AbilityPair(new WintersChillTap(), new WintersChillSwipe(), Talents[1][0].Name, Talents[1][0].Description, Talents[1][0].Icon)); });
        Talents[1][1] = new Talent("Steel Yourself",
            "Tap: Fire a projectile that slows by (1-(timepassed / 3))%. Lasts for 3 seconds.\nSwipe: Create an area around yourself of radius 10 where you steal 25% SP from anyone in the area.",
            "UI_Elements/AbilityIcons/SteelYourself",
            delegate(Player p) { UnlockSecondaryAbility(p, new AbilityPair(new SteelyourselfTap(), new SteelyourselfSwipe(), Talents[1][1].Name, Talents[1][1].Description, Talents[1][1].Icon)); });
        Talents[1][2] = new Talent("Stone", 
            "Tap: Fire a projectile that stuns for 1 seconds.\nSwipe: Create a Rock Wall that blocks movement and projectiles that lasts for 6 seconds", 
            "UI_Elements/AbilityIcons/Stone", 
            delegate(Player p) { UnlockSecondaryAbility(p, new AbilityPair(new StoneTap(), new StoneSwipe(), Talents[1][2].Name, Talents[1][2].Description, Talents[1][2].Icon)); });
        Talents[2][0] = new Talent("Trap", 
            "Adds the Trap prefab to your Citadel: An invisible block that cannot be destroyed that does causes an effect when players run over it. After being triggered, traps cannot be triggered again for 30 seconds. Costs 100% of material cost. The effect is based on material:\nSteel: 50 Metal damage\nWood: 15 Nature damage\nFire: 50 Fire damage over 5 seconds\nWater: Silences for 6 seconds\nStone: 25 Earth Damage\nAir:Transports target outside of Citadel platform\nObsidian: Causes the target to die after 10 seconds\nIce: Roots for 6 seconds\n Electric: Stuns for 3 seconds\n", 
            "UI_Elements/AbilityIcons/Trap", 
            delegate(Player p) { UnlockPrefab(p, Assets.CitadelBuilder.UnlockBlocks.Trap); });
        Talents[2][1] = new Talent("Turret", 
            "Adds the Turret prefab to your Citadel. Turrets automatically fire a projectile every second at the closest target. Has 50% of material's HP. Costs 200% of material's cost.  The effect is based on material:\nSteel: 50 Metal damage\nWood: 15 Nature damage\nFire: 50 Fire Damage over 5 seconds.\nWater: Silences for 6 seconds\nStone: 25 Earth Damage\nAir: Transports target outside of Citadel platform\nObsidian: 125 Earth Damage\nIce: Roots for 6 seconds\n Electric: Stuns for 2 seconds\n", 
            "UI_Elements/AbilityIcons/Turret", 
            delegate(Player p) { UnlockPrefab(p, Assets.CitadelBuilder.UnlockBlocks.Turret); });
        Talents[2][2] = new Talent("Battle Flag", "Adds the Battle Flag prefab to your Citadel. Within radius of 5, gives you a buff that lasts for 10 seconds. Has 20 HP. Costs 100% of material cost. Buff is based on material:\n:\nSteel: Next 2 attacks you're hit with do 0 damage.\nWood: Ressurect in half as long if you die.\nFire: +25% SP.\nWater: Regain 10 HP/s\nStone: +20% SD\nAir: +20% Movement Speed\nObsidian: Next attack does 1000% damage\nIce: Next attack also roots target for 4 seconds.\n Electric: Next attack also stuns for 2 seconds\n", "UI_Elements/AbilityIcons/Flag", delegate(Player p) { UnlockPrefab(p, Assets.CitadelBuilder.UnlockBlocks.Flag); });
        Talents[3][0] = new Talent("Lightning Bolt", 
            "Tap: Causes a bolt of lightning at the target you specified after 2 seconds that does 50 Lightning damage.\nSwipe: If your swipe contains you and your target, fire a lightning bolt that upon hitting them, chains to everyone within 10 until everyone has been hit or no one is within 10. Does 20 Lightning Damage.", 
            "UI_Elements/AbilityIcons/LightningBolt", 
            delegate(Player p) { UnlockPrimaryAbility(p, new AbilityPair(new LightningBoltTap(), new LightningBoltSwipe(), Talents[3][0].Name, Talents[3][0].Description, Talents[3][0].Icon)); });
        Talents[3][1] = new Talent("Gust", 
            "Tap: Knocks all targets within 10, 10 away and does 15 Wind Damage.\nSwipe: Causes a powerful gust that knocks down everyone in the swipe for 3 seconds.", 
            "UI_Elements/AbilityIcons/Gust", 
            delegate(Player p) { UnlockPrimaryAbility(p, new AbilityPair(new GustTap(), new GustSwipe(), Talents[3][1].Name, Talents[3][1].Description, Talents[3][1].Icon)); });
        Talents[3][2] = new Talent("Drown",
            "Tap: Smother the target with water, doing 5 Damage every 3 seconds for 12 seconds.\nSwipe: Gigantic tidal wave in direction of swipe that does 30 damage to not drowning targets and 60 damage to drowning targets.",
            "UI_Elements/AbilityIcons/Drown",
            delegate(Player p) { UnlockPrimaryAbility(p, new AbilityPair(new DrownTap(), new DrownSwipe(), Talents[3][2].Name, Talents[3][2].Description, Talents[3][2].Icon)); });
        Talents[4][0] = new Talent("Fire's Fury",
            "Tap: Gain +25% SP for 10 seconds.\nSwipe: Cause your next Fire's Fury buff to grant +100% SP but also -25% SD.",
            "UI_Elements/AbilityIcons/FiresFury",
            delegate(Player p) { UnlockSecondaryAbility(p, new AbilityPair(new FiresFuryTap(), new FiresFurySwipe(), Talents[4][0].Name, Talents[4][0].Description, Talents[4][0].Icon)); });
        Talents[4][1] = new Talent("Water's Cure", 
            "Tap: Causes you to regain 50 HP over 10 seconds.\nSwipe: Cause your next Water's Cure buff to heal yourself for 100 instantly and grant you +25% SD.", 
            "UI_Elements/AbilityIcons/WatersCure", 
            delegate(Player p) { UnlockSecondaryAbility(p, new AbilityPair(new WatersCureTap(), new WatersCureSwipe(), Talents[4][1].Name, Talents[4][1].Description, Talents[4][1].Icon)); });
        Talents[4][2] = new Talent("Speed of Wind",
            "Tap: Gives you +25% Movement Speed.\nSwipe: Transport yourself up to 15 along your swipe.",
            "UI_Elements/AbilityIcons/SpeedOfWind",
            delegate(Player p) { UnlockSecondaryAbility(p, new AbilityPair(new SpeedOfWindTap(), new SpeedOfWindSwipe(), Talents[4][2].Name, Talents[4][2].Description, Talents[4][2].Icon)); });
        Talents[5][0] = new Talent("Mirror", 
            "Adds a new material to your Citadel that causes normal blocks to reflect 25% of the damage they take.\nTraps made with this material cause the target to take 50% of the damage of their next five attacks.\nTurrets made with this material do more damage if they are lower health.\nBattle Flags made with this material cause the owner to gain a buff that makes them reflect the next 3 attacks that hit them.", 
            "UI_Elements/AbilityIcons/Mirror", 
            delegate(Player p) { UnlockMaterial(p, Assets.CitadelBuilder.UnlockMats.Mirror); });
        Talents[5][1] = new Talent("Learner", 
            "Adds a new material to your Citadel that causes normal blocks to change element to be strong against the last attack that hit them.\nTraps made with this material cause the target to do no damage on their next five attacks.\nTurrets made with this material change the element they are firing to be strong against the last attack they received.\nBattle Flags made with this material cause the owner to gain a buff that makes their attacks automatically strong against what they are attacking.", 
            "UI_Elements/AbilityIcons/Learner", 
            delegate(Player p) { UnlockMaterial(p, Assets.CitadelBuilder.UnlockMats.Learner); });
        Talents[5][2] = new Talent("Linker", 
            "Adds a new material to your Citadel that takes 50% of normal damage when you are alive.\nTraps made with this material kill their target instantly if you are alive, otherwise do nothing.\nTurrets made with this material do twice as much damage if you are alive.\nBattle Flags made with this material cause the owner to gain a buff that gives them double SP and SD, but are instantly destroyed when the player dies.", 
            "UI_Elements/AbilityIcons/Linker", 
            delegate(Player p) { UnlockMaterial(p, Assets.CitadelBuilder.UnlockMats.Linker); });
        Talents[6][0] = new Talent("Summon", "Tap: Summons a minion that fights alongside you for 10 seconds. Can only have one. Minions fire projectiles that do 10 True damage at the location of your taps. \nSwipe: Stampede of minions that stuns anyone caught in it and does 5 True damage/second.", "UI_Elements/AbilityIcons/Summon", defaultScript);
        Talents[6][1] = new Talent("Storm", "Tap: Reduces the target's SD by 25% for 10 seconds.\nSwipe: Cone that stuns targets caught in it for 4 seconds.", "UI_Elements/AbilityIcons/Storm", defaultScript);
        Talents[6][2] = new Talent("Entangling Roots", "Tap: Roots everyone within 5 of you for 2 seconds.\nSwipe: Roots everyone in the line for 5 seconds.", "UI_Elements/AbilityIcons/EntanglingRoots", defaultScript);
        Talents[7][0] = new Talent("Citadel Defender", "While on your citadel platform, gain 50% SD", "UI_Elements/AbilityIcons/CitadelDefender", defaultScript);
        Talents[7][1] = new Talent("Citadel Assaulter", "While on an enemy citadel's platform, gain 67% SP", "UI_Elements/AbilityIcons/CitadelAssaulter", defaultScript);
        Talents[7][2] = new Talent("Rapid Response", "While not on any citadel platform, gain 50% Movement Speed", "UI_Elements/AbilityIcons/RapidResponse", defaultScript);
        Talents[8][0] = new Talent("Darkness", "Tap: Fire a projectile causing 50 True damage. If it hits a target, causes 25 True damage to you.\nSwipe: Creates a wall along your swipe that will instantly kill any player that passes through it. Takes 1 second to activate.", "UI_Elements/AbilityIcons/Darkness", defaultScript);
        Talents[8][1] = new Talent("Blizzard", "Tap: Creates a blizzard at the tap target after 1 second that lasts for 2 seconds. Enemies caught in it are slowed by 50% and take 10 Ice damage/second.\nSwipe: Fire a powerful blizzard along the line that slows enemies caught in it by 75% and causes them to take 20 damage/second", "UI_Elements/AbilityIcons/Blizzard",
            delegate(Player p) { UnlockSecondaryAbility(p, new AbilityPair(new FireballTap(), new BlizzardTap(), Talents[8][1].Name, Talents[8][1].Description, Talents[8][1].Icon)); });
            // delegate(Player p) { UnlockPrimaryAbility(p, new AbilityPair(new FireballTap(), new BlizzardTap(), Talents[0][0].Name, Talents[0][0].Description, Talents[0][0].Icon)); });
        Talents[8][2] = new Talent("The Mountain", "Tap: Causes a boulder to fall on that location after 1 second. Causes 20 Earth damage to anyone it hits. \nSwipe: If your swipe contains both you and an enemy, that enemy is lifted off the ground and stunned, and takes 15 Earth damage/s for 2 seconds but is untargetable.", "UI_Elements/AbilityIcons/TheMountain", defaultScript);
           
        Talents[9][0] = new Talent("Regenerator", "Adds a permanent Spire to your Citadel that causes all blocks to regenerate for as long as it stands. Has 200 HP.", "UI_Elements/AbilityIcons/Regenerator", defaultScript);
        Talents[9][1] = new Talent("Shielder", "Adds a permanent Spire to your Citadel that causes blocks to have a shield for 20% of their life. Shield will regenerate if the block hasn't taken damage for 10 seconds and the Spire stands. Has 200 HP.", "UI_Elements/AbilityIcons/Shielder", defaultScript);
        Talents[9][2] = new Talent("Altar Protector", "Adds a permanent Spire to your Citadel that causes your Altar to take 50% of normal damage for as long as it stands.", "UI_Elements/AbilityIcons/AltarProtector", defaultScript);
    }

    private static void defaultScript(Player p)
    {
        
    }

    //TEMP METHOD
    public static bool HasDefaultScript(Talent t)
    {
        return t.Unlock == defaultScript;
    }

    private static void UnlockPrimaryAbility(Player p, AbilityPair ability)
    {
        Player CurrentPlayer = p;
        Debug.Log(CurrentPlayer.PrimaryAbilities == null);
        if (CurrentPlayer.PrimaryAbilities.Count >= 3)
        {
            throw new Exception("y u do dis");
        }
        CurrentPlayer.PrimaryAbilities.Add(ability);
    }
    private static void UnlockSecondaryAbility(Player p, AbilityPair ability)
    {
        Player CurrentPlayer = p;
        if (CurrentPlayer.SecondaryAbilities.Count >= 3)
        {
            throw new Exception("y u do dis");
        }
        CurrentPlayer.SecondaryAbilities.Add(ability);
    }
    private static void UnlockPrefab(Player p, Assets.CitadelBuilder.UnlockBlocks prefab)
    {
        Player CurrentPlayer = p;
        CurrentPlayer.UnlockedBlock = prefab;
    }
    private static void UnlockMaterial(Player p, Assets.CitadelBuilder.UnlockMats mat)
    {
        Player CurrentPlayer = p;
        CurrentPlayer.UnlockedMaterial = mat;
    }
    private static void UnlockSpire(Player p, Assets.CitadelBuilder.SpireType spireType)
    {
        Player CurrentPlayer = p;
        CurrentPlayer.UnlockedSpire = spireType;
    }


    private static string defaultIcon
    {
        get
        {
            return "UI_Elements/AbilityIcons/placeholder";
        }
    }
    private enum TalentType
    {
        PrimaryAbility,
        SecondaryAbility,
        Material,
        Prefab,
        Spire,
        Passive
    }
}
