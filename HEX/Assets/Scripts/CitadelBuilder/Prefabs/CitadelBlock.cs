using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.NetworkLayout;

namespace Assets.CitadelBuilder
{
    public abstract class CitadelBlock
    {
        public GameObject Prefab { get; protected set; } //IF YOU ARE ACCESSING THIS AND YOUR NAME IS NOT MOUSEPICK, PLEASE FUCK OFF
        public CMaterial Mat { get; set; }
        public string TypeName { get; private set; }
        public Quaternion Rotation { get; private set; }
        public void Rotate(float rot, Vector3 axis)
        {
            if (Prefab != null)
            {
                Prefab.transform.Rotate(axis, rot, Space.World);
                Rotation = Quaternion.AngleAxis(rot, axis) * Rotation;
                if (Rotation != Prefab.transform.rotation)
                {
                    Debug.Log(Rotation);
                    Debug.Log(Prefab.transform.rotation);
                }
            }
            else
            {
                Rotation *= Quaternion.AngleAxis(rot, axis);
            }
            
        }

        public CitadelBlock(string file)
        {
            Rotation = Quaternion.AngleAxis(-90.0f, Vector3.right);
            Prefab = Resources.Load(file) as GameObject;
            Prefab.transform.rotation = Quaternion.AngleAxis(-90.0f, Vector3.right);
            TypeName = Prefab.name;
            if (Rotation != Prefab.transform.rotation)
            {
                throw new Exception("rotations are broken");
            }
        }
        
        public virtual CitadelBlock Instantiate(CMaterial m, Vector3 pos = new Vector3(), Quaternion rot = new Quaternion())
        {
            Prefab = MonoBehaviour.Instantiate(Prefab, pos, rot) as GameObject;
            if (m.DiffuseColor != null) Prefab.renderer.material.color = m.DiffuseColor;
            Mat = m;
            Rotation = rot;         
            return this;
        }
        public void EditGameObject(GameObject prefab)
        {
            prefab.transform.rotation = Rotation;
            prefab.GetComponent<INet_Block>().Mat = Mat;
            prefab.renderer.material.color = Mat.DiffuseColor;
        }
        public static CitadelBlock TypeToBlock(Type t)
        {
            CitadelBlock newBlock;

            if (t == typeof(Brick))
            {
                newBlock = new Brick();
            }
            else if (t == typeof(ConcaveCorner))
            {
                newBlock = new ConcaveCorner();
            }
            else if (t == typeof(ConvexCorner))
            {
                newBlock = new ConvexCorner();
            }
            else if (t == typeof(Spire))
            {
                newBlock = new Spire();
            }
            else if (t == typeof(Trap))
            {
                newBlock = new Trap();
            }
            else if (t == typeof(Turret))
            {
                newBlock = new Turret();
            }
            else if (t == typeof(Flag))
            {
                newBlock = new Flag();
            }
            else if (t == typeof(Altar))
            {
                newBlock = new Altar();
            }
            else
            {
                throw new Exception("Add this type to this queer ass pretend switch statement dickfuck");
            }
            return newBlock;
        }
    }

    public enum UnlockBlocks
    {
        Trap,
        Turret,
        Flag
    }
}
