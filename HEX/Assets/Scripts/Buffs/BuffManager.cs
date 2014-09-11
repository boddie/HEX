using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Buffs
{
	public class BuffManager
	{
        private Dictionary<Type, Buff> _buffs; //ensures uniqueness
        private Dictionary<Type, Debuff> _debuffs; 
        public BuffManager()
        {
            _buffs = new Dictionary<Type, Buff>();
            _debuffs = new Dictionary<Type, Debuff>();
        }
        public bool HasPEffect(PEffect p)
        {
            return _buffs.ContainsKey(p.GetType()) || _debuffs.ContainsKey(p.GetType());
        }
        public void AddBuff(Buff b)
        {
            if (_buffs.ContainsKey(b.GetType()))
            {
                _buffs[b.GetType()].Resolve(b);
            }
            else
            {
                _buffs.Add(b.GetType(), b);
            }
        }
        public bool RemoveBuff(Buff b)
        {
            return _buffs.Remove(b.GetType());
        }
        public bool RemoveEffect(PEffect p)
        {
            if (typeof(Buff).IsAssignableFrom(p.GetType()))
            {
                Debug.Log("Removing Effect of " + p.GetType());
                return RemoveBuff(p as Buff);
            }
            else if (typeof(Debuff).IsAssignableFrom(p.GetType()))
            {
                Debug.Log("Removing Effect of " + p.GetType());
                return RemoveDebuff(p as Debuff);
            }
            throw new Exception("why did you do this. why");
        }
        public void ClearAll()
        {
            _buffs.Clear();
            _debuffs.Clear();
        }
        public void AddDebuff(Debuff d)
        {
            if (_debuffs.ContainsKey(d.GetType()))
            {
                _debuffs[d.GetType()].Resolve(d);
            }
            else
            {
                _debuffs.Add(d.GetType(), d);
            }
        }
        public bool RemoveDebuff(Debuff d)
        {
            return _debuffs.Remove(d.GetType());
        }

        public void Update()
        {
            List<KeyValuePair<Type, Buff>> btoRemove =_buffs.Where((b) => !b.Value.Active).ToList();
            List<KeyValuePair<Type, Debuff>> dtoRemove = _debuffs.Where((d) => !d.Value.Active).ToList();
            foreach (var debuffs in dtoRemove)
            {
                PersistentData.Instance.RemoveEffect(PersistentData.Instance.CurrentPlayer.Alias, debuffs.Value.Serialize());
            }
            foreach (var buff in btoRemove)
            {
                PersistentData.Instance.RemoveEffect(PersistentData.Instance.CurrentPlayer.Alias, buff.Value.Serialize());
            }
            foreach (var buff in _buffs)
            {
                buff.Value.Update();
            }
            foreach (var debuff in _debuffs)
            {
                debuff.Value.Update();
            }           
        }
        //public void Draw()
        //{
        //    Vector2 bposition = new Vector2(Screen.width - PEffect.IconWidth.x, 0);
        //    Vector2 dposition = new Vector2(Screen.width - PEffect.IconWidth.x, PEffect.IconWidth.y*2);
        //    foreach (var buff in _buffs)
        //    {
        //        buff.Value.Draw(bposition);
        //        bposition.x -= PEffect.IconWidth.x * 1.25f; 
        //    }
        //    foreach (var debuff in _debuffs)
        //    {
        //        debuff.Value.Draw(dposition);
        //        dposition.x -= PEffect.IconWidth.x * 1.25f;
        //    }
        //}
	}
}
