using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Buffs
{
	public abstract class PEffect
	{
        protected PersistentData manager; //unfortunately, all buff effects should be applied through persistentdata for network purposes
        public static Vector2 IconWidth;
        static PEffect()
        {
            IconWidth = new Vector2(Screen.width * .05f, Screen.height * .1f);
        }
        public bool Active { get; protected set; }
        protected float Duration { get; private set; } //measured in s
        protected float TickTime { get; private set; } //measured in s
        private float CurrentTick;
        private float InactiveChecker;
        InterfaceTexture toDraw;

        private bool _firstUpdate = true;
        public PEffect(int duration, string iconfile, int tickTime = 0)
        {
            Active = true;
            Duration = duration;
            TickTime = tickTime;
            CurrentTick = tickTime;
            InactiveChecker = 0;
            toDraw = new InterfaceTexture(iconfile, new Rect(0, 0, IconWidth.x, IconWidth.y));
            manager = GameObject.Find("Persistence").GetComponent<PersistentData>();
        }
        
        public void Update()
        {      
            if (!Active)
            {
                return;
            }
            //Debug.Log("Updating a peffect");
            //essentially, it removes and applies the effect for actual buffs every iteration. This
            //allows buffs that need to do something every iteration the ability to do so, and ensures that
            //whatever change was made is eventually undone
            if (_firstUpdate)
            {
                _firstUpdate = false;
            }
            else
            {
                CleanUp();
            }
            CurrentTick += Time.deltaTime;
            InactiveChecker += Time.deltaTime;
            if (InactiveChecker > Duration)
            {
                Active = false;
                //Debug.Log("Removing");
                return;
            }
           // Debug.Log("CurrentTick: " + CurrentTick + "::: TickTime: " + TickTime);
            if (CurrentTick > TickTime)
            {
                //Debug.Log("Applying an effect");
                CurrentTick = 0;
                ApplyEffect();
            }           
        }

        public abstract void ApplyEffect();
        public abstract void CleanUp();

        //handles duplicates
        public void Resolve(PEffect other)
        {
            if (other.GetType() != this.GetType())
            {
                throw new Exception("wrong type dickwad");
            }
            this.Duration = Math.Max(this.Duration, other.Duration);
            this.TickTime = Math.Min(this.TickTime, other.TickTime);
            this.InactiveChecker = Math.Min(this.InactiveChecker, other.InactiveChecker);
            Debug.Log("Resolving effect of type: " + other.GetType());
            ResolveCritical(other);
        }
        protected abstract void ResolveCritical(PEffect other);
        //public void Draw(Vector2 position)
        //{
        //    Debug.Log("Drawing a peffect");
        //    toDraw.Position = position;
        //    toDraw.Draw();
        //}

        public int Serialize()
        {
            if (GetType() == typeof(Chilled))
            {
                return 1;
            }
            else if (GetType() == typeof(Frozen))
            {
                return 2;
            }
            else if (GetType() == typeof(Knockeddown))
            {
                return 4;
            }
            else if (GetType() == typeof(Drowning))
            {
                return 5;
            }
            else if (GetType() == typeof(Soft))
            {
                return 6;
            }
            else if (GetType() == typeof(Impale))
            {
                return 7;
            }
            else if (GetType() == typeof(Roots))
            {
                return 8;
            }
            else if (GetType() == typeof(EmpoweredRoots))
            {
                return 9;
            }
            else if (GetType() == typeof(Electrified))
            {
                return 10;
            }
            else if (GetType() == typeof(Stampede))
            {
                return 11;
            }
            else if (GetType() == typeof(Shocked))
            {
                return 12;
            }
            else if (GetType() == typeof(LeadLegs))
            {
                return 13;
            }
            else if (GetType() == typeof(Hotheaded))
            {
                return 14;
            }
            else if (GetType() == typeof(Furious))
            {
                return 15;
            }
            else if (GetType() == typeof(Steel))
            {
                return 16;
            }
            else if (GetType() == typeof(Haste))
            {
                return 17;
            }
            else if (GetType() == typeof(Renew))
            {
                return 18;
            }
            else if (GetType() == typeof(Rejuvenate))
            {
                return 19;
            }
            else if (GetType() == typeof(FlagBuff))
            {
                return 20;
            }
            else if (GetType() == typeof(IceBeamUse))
            {
                return 21;
            }
            else
            {
                throw new Exception("Not recognized as a Peffect");
            }
        }
        public static PEffect Deserialize(int id, double spellpower)
        {
            switch (id)
            {
                case 1:
                    return new Chilled();
                case 2:
                    return new Frozen();
                case 4:
                    return new Knockeddown();
                case 5:
                    return new Drowning(spellpower);
                case 6:
                    return new Soft();
                case 7:
                    return new Impale();
                case 8:
                    return new Roots();
                case 9:
                    return new EmpoweredRoots();
                case 10:
                    return new Electrified();
                case 11:
                    return new Stampede();
                case 12:
                    return new Shocked();
                case 13:
                    return new LeadLegs();
                case 14:
                    return new Hotheaded();
                case 15:
                    return new Furious();
                case 16:
                    return new Steel();
                case 17:
                    return new Haste();
                case 18:
                    return new Renew(spellpower);
                case 19:
                    return new Rejuvenate();
                case 20:
                    return new FlagBuff(0.0); //presently it's just particles, anyhow.
                case 21:
                    return new IceBeamUse();
            }
            throw new Exception("This isn't recognized as a valid id");
        }
	}
}
