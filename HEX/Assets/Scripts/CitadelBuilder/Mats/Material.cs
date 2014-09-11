using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Elements;

namespace Assets.CitadelBuilder
{
    public abstract class CMaterial
    {
        protected int _cost;
        public Color DiffuseColor { get; protected set; } //this should eventually be a texture or something
        public int Health { get; protected set; } //for now, this will be the only variable, in the future that won't be sufficient
        public int CurrentHealth { get; private set; }

        public int GetCost(CitadelBlock block)
        {
            if (block.GetType() == typeof(Turret))
            {
                return _cost * 2;
            }
            else
            {
                return _cost;
            }
        }

        public virtual void PerformOnHitEffect(string attacker, IElement ofAttack, int damage)
        {
            if (Element.WeakAgainst(ofAttack))
            {
                damage *= 2;
            }
            else if (Element.StrongAgainst(ofAttack))
            {
                damage /= 2;
            }
            CurrentHealth -= damage;
        }
        public virtual bool Dead()
        {
            return CurrentHealth <= 0;
        }
        public void Init()
        {
            CurrentHealth = Health;
        }
        public IElement Element { get; protected set; }
        public static CMaterial TypeToMat(Type t)
        {
            CMaterial toReturn = null;
            if (t == typeof(AirMaterial))
            {
                toReturn = new AirMaterial();
            }
            else if (t == typeof(ElecMaterial))
            {
                toReturn = new ElecMaterial();
            }
            else if (t == typeof(FireMaterial))
            {
                toReturn = new FireMaterial();
            }
            else if (t == typeof(IceMaterial))
            {
                toReturn = new IceMaterial();
            }
            else if (t == typeof(LearnerMaterial))
            {
                toReturn = new LearnerMaterial();
            }
            else if (t == typeof(LinkerMaterial))
            {
                toReturn = new LinkerMaterial();
            }
            else if (t == typeof(MirrorMaterial))
            {
                toReturn = new MirrorMaterial();
            }
            else if (t == typeof(ObsidianMaterial))
            {
                toReturn = new ObsidianMaterial();
            }
            else if (t == typeof(SteelMaterial))
            {
                toReturn = new SteelMaterial();
            }
            else if (t == typeof(StoneMaterial))
            {
                toReturn = new StoneMaterial();
            }
            else if (t == typeof(WaterMaterial))
            {
                toReturn = new WaterMaterial();
            }
            else if (t == typeof(WoodMaterial))
            {
                toReturn = new WoodMaterial();
            }
            else if (t == typeof(AltarMaterial))
            {
                toReturn = new AltarMaterial();
            }
            else
            {
                throw new Exception("you're a real asshat " + t);
            }
            return toReturn;
        }
        public static CMaterial Deserialize(byte[] type)
        {
            Debug.Log("Attempting Deserialization...");
            CMaterial toReturn = null;
            int typer = BitConverter.ToInt32(type, 0);
            switch (typer)
            {
                case 1:
                    toReturn = new AirMaterial();
                    break;
                case 2:
                    toReturn = new ElecMaterial();
                    break;
                case 3:
                    toReturn = new FireMaterial();
                    break;
                case 4:
                    toReturn = new IceMaterial();
                    break;
                case 5:
                    toReturn = new LinkerMaterial();
                    break;
                case 6:
                    toReturn = new LearnerMaterial();
                    break;
                case 7:
                    toReturn = new MirrorMaterial();
                    break;
                case 8:
                    toReturn = new ObsidianMaterial();
                    break;
                case 9:
                    toReturn = new SteelMaterial();
                    break;
                case 10:
                    toReturn = new StoneMaterial();
                    break;
                case 11:
                    toReturn = new WaterMaterial();
                    break;
                case 12:
                    toReturn = new WoodMaterial();
                    break;
                case 13:
                    toReturn = new AltarMaterial();
                    break;
            }
            if (toReturn == null)
            {
                throw new Exception("FUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUCK YOUUUUUUUUUUUUUUUUUUUUUUU");
            }
            return toReturn;
        }
        public virtual byte[] Serialize()
        {
            byte[] type = new byte[4];
            if (this.GetType() == typeof(AirMaterial))
            {
                type = BitConverter.GetBytes(1);
            }
            else if (this.GetType() == typeof(ElecMaterial))
            {
                type = BitConverter.GetBytes(2);
            }
            else if (this.GetType() == typeof(FireMaterial))
            {
                type = BitConverter.GetBytes(3);
            }
            else if (this.GetType() == typeof(IceMaterial))
            {
                type = BitConverter.GetBytes(4);
            }
            else if (this.GetType() == typeof(LinkerMaterial))
            {
                type = BitConverter.GetBytes(5);
            }
            else if (this.GetType() == typeof(LearnerMaterial))
            {
                type = BitConverter.GetBytes(6);
            }
            else if (this.GetType() == typeof(MirrorMaterial))
            {
                type = BitConverter.GetBytes(7);
            }
            else if (this.GetType() == typeof(ObsidianMaterial))
            {
                type = BitConverter.GetBytes(8);
            }
            else if (this.GetType() == typeof(SteelMaterial))
            {
                type = BitConverter.GetBytes(9);
            }
            else if (this.GetType() == typeof(StoneMaterial))
            {
                type = BitConverter.GetBytes(10);
            }
            else if (this.GetType() == typeof(WaterMaterial))
            {
                type = BitConverter.GetBytes(11);
            }
            else if (this.GetType() == typeof(WoodMaterial))
            {
                type = BitConverter.GetBytes(12);
            }
            else if (this.GetType() == typeof(AltarMaterial))
            {
                type = BitConverter.GetBytes(13);
            }
            return type;
        }

        
    }
    public enum UnlockMats
    {
        Mirror,
        Learner,
        Linker
    }
}
