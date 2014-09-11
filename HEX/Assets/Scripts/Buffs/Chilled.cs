using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Buffs
{
    class Chilled : Debuff, MSDebuff
    {
        protected int damage;
        public double msreduction
        {
            get
            {
                return .5;
            }
        }
        public Chilled()
            : base(1, "", 1)
        {
            damage = (int) Math.Round(5f * manager.CurrentPlayer.SpellPower);
        }
        bool doubleDamage = false;
        public override void ApplyEffect()
        {
            manager.CurrentPlayer.MovementSpeed *= (1 - msreduction);
            manager.giveDamage("Ice Beam", manager.CurrentPlayer.Alias, damage);
            Debug.Log("Buff applied: " + manager.CurrentPlayer.MovementSpeed);
            if (doubleDamage)
            {
                damage += damage;
                doubleDamage = false;
            }
        }
        public override void CleanUp()
        {
            manager.CurrentPlayer.MovementSpeed /= (1 - msreduction);
            Debug.Log("Buff cleaned: " + manager.CurrentPlayer.MovementSpeed);
        }
        protected override void ResolveCritical(PEffect other)
        {
            doubleDamage = true;
        }
    }
}
