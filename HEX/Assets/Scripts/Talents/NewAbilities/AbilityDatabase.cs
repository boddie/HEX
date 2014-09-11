using System.Collections.Generic;
using System;
using Assets.CitadelBuilder;
using Assets.Elements;
using Assets.Buffs;

public class AbilityDatabase
{
    private static AbilityDatabase _instance;
    public static AbilityDatabase Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new AbilityDatabase();
            }
            return _instance;
        }
    }

    public BaseAbility GetAbility(int identifier)
    {
        switch (identifier)
        {
            case 0:
                return new FireballTap();
            case 1:
                return new FireballSwipe();
            case 2:
                return new NWTap();
            case 3:
                return new NWSwipe();
            case 4:
                return new LanceTap();
            case 5:
                return new LanceSwipe();
            case 6:
                return new WintersChillTap();
            case 7:
                return new WintersChillSwipe();
            case 8:
                return new SteelyourselfTap();
            case 9:
                return new SteelyourselfSwipe();
            case 10:
                return new StoneTap();
            case 11:
                return new StoneSwipe();
            //12, 13, 14, 15, 16, 17 reserved for prefabs
            case 18:
                return new LightningBoltTap();
            case 19:
                return new LightningBoltSwipe();
            case 20:
                return new GustTap();
            case 21:
                return new GustSwipe();
            case 22:
                return new DrownTap();
            case 23:
                return new DrownSwipe();
            case 24:
                return new FiresFuryTap();
            case 25:
                return new FiresFurySwipe();
            case 26:
                return new WatersCureTap();
            case 27:
                return new WatersCureSwipe();
            case 28:
                return new SpeedOfWindTap();
            case 29:
                return new SpeedOfWindSwipe();
            //30, 31, 32, 33, 34, 35 reserved for materials
            case 36:
                return new BlizzardTap();
           // case 37:
           //     return new BlizzardSwipe();
            case 38:
                return new DrownTap();
            case 39:
                return new DrownSwipe();
            case 100:
                return new TurretAbility(new FireMaterial());
            case 101:
                return new TurretAbility(new ElecMaterial());
            case 102:
                return new TurretAbility(new IceMaterial());
            case 103:
                return new TurretAbility(new StoneMaterial());
            case 104:
                return new TurretAbility(new ObsidianMaterial());
            case 105:
                return new TurretAbility(new SteelMaterial());
            case 106:
                return new TurretAbility(new MirrorMaterial());
            case 107:
                return new TurretAbility(new WaterMaterial());
            case 108:
                return new TurretAbility(new AirMaterial());
            case 109:
                return new TurretAbility(new WoodMaterial());
            case 110:
                return new TurretAbility(new LinkerMaterial());
            case 111:
                return new TurretAbility(new LearnerMaterial());
            default:
                throw new Exception("Not in AbilityDatabase");
        }
    }

    public int GetIdentifier(BaseAbility ability)
    {
        if (ability.GetType() == typeof(FireballTap))
        {
            return 0;
        }
        else if (ability.GetType() == typeof(FireballSwipe))
        {
            return 1;
        }
        else if (ability.GetType() == typeof(NWTap))
        {
            return 2;
        }
        else if (ability.GetType() == typeof(NWSwipe))
        {
            return 3;
        }
        else if (ability.GetType() == typeof(LanceTap))
        {
            return 4;
        }
        else if (ability.GetType() == typeof(LanceSwipe))
        {
            return 5;
        }
        else if (ability.GetType() == typeof(WintersChillTap))
        {
            return 6;
        }
        else if (ability.GetType() == typeof(WintersChillSwipe))
        {
            return 7;
        }
        else if (ability.GetType() == typeof(SteelyourselfTap))
        {
            return 8;
        }
        else if (ability.GetType() == typeof(SteelyourselfSwipe))
        {
            return 9;
        }
        else if (ability.GetType() == typeof(StoneTap))
        {
            return 10;
        }
        else if (ability.GetType() == typeof(StoneSwipe))
        {
            return 11;
        }
        else if (ability.GetType() == typeof(GustTap))
        {
            return 20;
        }
        else if (ability.GetType() == typeof(GustSwipe))
        {
            return 21;
        }
        else if (ability.GetType() == typeof(FiresFuryTap))
        {
            return 24;
        }
        else if (ability.GetType() == typeof(FiresFurySwipe))
        {
            return 25;
        }
        else if (ability.GetType() == typeof(WatersCureTap))
        {
            return 26;
        }
        else if (ability.GetType() == typeof(WatersCureSwipe))
        {
            return 27;
        }
        else if (ability.GetType() == typeof(SpeedOfWindTap))
        {
            return 28;
        }
        else if (ability.GetType() == typeof(SpeedOfWindSwipe))
        {
            return 29;
        }
        else if (ability.GetType() == typeof(LightningBoltTap))
        {
            return 18;
        }
        else if (ability.GetType() == typeof(LightningBoltSwipe))
        {
            return 19;
        }
        else if (ability.GetType() == typeof(DrownTap))
        {
            return 22;
        }
        else if (ability.GetType() == typeof(DrownSwipe))
        {
            return 23;
        }

        else if (ability.GetType() == typeof(TurretAbility))
        {
            TurretAbility tability = ability as TurretAbility;
            if (ability.Element == typeof(Fire))
            {
                return 100;
            }
            if (ability.Element.GetType() == typeof(Electricity))
            {
                return 101;
            }
            if (ability.Element.GetType() == typeof(Ice))
            {
                return 102;
            }
            if (ability.Element.GetType() == typeof(Earth))
            {
                //103: stone
                //104: obsidian
                if (tability.MaterialName.Contains("Stone"))
                {
                    return 103;
                }
                else if (tability.MaterialName.Contains("Obsidian"))
                {
                    return 104;
                }
                else
                {
                    throw new Exception("Your namer isn't working");
                }
            }
            if (ability.Element.GetType() == typeof(Metal))
            {
                return 105;
            }
            if (ability.Element.GetType() == typeof(Water))
            {
                return 107;
            }
            if (ability.Element.GetType() == typeof(Wind))
            {
                //108: air
                //111: learner
                if (tability.MaterialName.Contains("Air"))
                {
                    return 108;
                }
                else if (tability.MaterialName.Contains("Learner"))
                {
                    return 111;
                }
                else
                {
                    throw new Exception("Namer isn't working");
                }
            }
            if (ability.Element.GetType() == typeof(Nature))
            {
                //109: wood
                //110: linker
                if (tability.MaterialName.Contains("Wood"))
                {
                    return 109;
                }
                else if (tability.MaterialName.Contains("Linker"))
                {
                    return 110;
                }
                else
                {
                    throw new Exception("Your namer isn't working");
                }
            }
        }
        else if (ability.GetType() == typeof(BlizzardTap))
        {
            return 36;
        }

        return -1;
    }
}

public class AbilityEffectDatabase
{
    private static AbilityDatabase _instance;
    public static AbilityDatabase Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new AbilityDatabase();
            }
            return _instance;
        }
    }

    public PEffect GetAbilityEffect(int identifier)
    {
        switch (identifier)
        {
            case 0:
                break;
        }

        return null;
    }
}

