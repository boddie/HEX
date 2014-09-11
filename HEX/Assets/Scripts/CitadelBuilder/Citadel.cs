using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

namespace Assets.CitadelBuilder
{
    public class Citadel
    {
        public const int NUM_SQUARES = 20; //this on the other hand appears to be an actual constant, unless i change the texture mapping
        public const int REC_DIM = 10; //can probably actually pull this from unity


        public const string CitadelExtension = ".citadel";
        public const string CitadelFolderName = "Citadels\\";
        public static string CitadelFolderPath
        {
            get
            {
                return manager.CurrentPlayer.CharFolderPath + "\\" + CitadelFolderName;
            }
        }
        public string CitadelFileName
        {
            get
            {
                return Name + CitadelExtension;
            }
        }
        public string CitadelFilePath
        {
            get
            {
                return CitadelFolderPath + "\\" + CitadelFileName;
            }
        }
        private static PersistentData manager;
        public CitadelBlock[,] Blocks { get; set; }
        public string Name { get; private set; } 
        private int _number;
        static Citadel()
        {
            manager = GameObject.Find("Persistence").GetComponent<PersistentData>();
        }
        /// <summary>
        /// This will be associated with New Citadel
        /// </summary>
        public Citadel(int number, string name)
        {
            Name = name;
            _number = number;
            Blocks = new CitadelBlock[NUM_SQUARES, NUM_SQUARES];
            Blocks[NUM_SQUARES / 2, NUM_SQUARES / 2] = new Altar();
        }

        public void RenameCitadel(string renamer)
        {
            File.Move(CitadelFilePath, CitadelFolderPath + "//" + renamer+CitadelExtension);
            Name = renamer;
        }

        public static void LoadCitadels()
        {
            manager.Citadels = new Citadel[3]; //CLEARING DATA SO THAT IT DOESN'T BE DUMB
            int i = -1;
            try
            {
                Debug.Log(manager.CurrentPlayer);
                //Debug.Log(manager.profile.SelectedPlayer);
                foreach (var localfile in Directory.GetFiles(manager.CurrentPlayer.CharFolderPath + "\\" + CitadelFolderName, "*" + CitadelExtension))
                {
                    i++;
                    LoadFromFile(localfile);
                    if (i > 2)
                    {
                        Debug.LogError("You are bad and should feel bad. I SAID THREE CITADELS. THREE!");
                    }
                }
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory(manager.CurrentPlayer.CharFolderPath + "\\" + CitadelFolderName);
            }
        }

        public static void DeleteAllCitadels()
        {
            if (manager.Citadels != null)
            {
                foreach (Citadel citadel in manager.Citadels)
                {
                    if (citadel != null)
                    {
                        citadel.Delete();
                    }
                }
            }
            try
            {
                Directory.Delete(manager.CurrentPlayer.CharFolderPath + "\\" + CitadelFolderName + "\\");
            }
            catch (DirectoryNotFoundException)
            {
                // That's okay. This player just never entered the lobby and thus the directory wasn't created.
            }
            catch (IOException)
            {
                //whoopsies. This means our cleanup was NOT successful. We're going to have to do a full push:
                Directory.Delete(manager.CurrentPlayer.CharFolderPath + "\\" + CitadelFolderName + "\\", true);
            }

        }
        static void LoadFromFile(string path)
        {
            List<String> lines = File.ReadAllLines(path).ToList();
            //line 0 is special, the rest are blocks
            string[] fileNameExt = path.Split('\\', '.');
            Citadel c = null;
            string def = "";
            if (lines.Count != Citadel.NUM_SQUARES * Citadel.NUM_SQUARES + 1)
            {
                throw new Exception("invalid citadel file dickfuk");
            }
            for (int i = 0; i < lines.Count; i++)
            {
                string[] stuff = lines[i].Split(new string[] { "$CASH$" }, StringSplitOptions.None);
                //Debug.Log(stuff.Length);
                if (i == 0)
                {
                    Debug.Log(stuff[0]);
                    c = new Citadel(Convert.ToInt32(stuff[0]), fileNameExt[fileNameExt.Length - 2]);
                    def = stuff[1];
                }
                else
                {
                    int row = (i-1) / Citadel.NUM_SQUARES;
                    int column = (i-1) - row * Citadel.NUM_SQUARES;

                    switch (stuff[0])
                    {
                        case "Brick":
                            c.Blocks[row, column] = new Brick();
                            break;
                        case "Altar":
                            c.Blocks[row, column] = new Altar();
                            break;
                        case "ConcaveCorner":
                            c.Blocks[row, column] = new ConcaveCorner();
                            break;
                        case "ConvexCorner":
                            c.Blocks[row, column] = new ConvexCorner();
                            break;
                        case "Flag":
                            c.Blocks[row, column] = new Flag();
                            break;
                        case "Spire":
                            c.Blocks[row, column] = new Spire();
                            break;
                        case "Trap":
                            c.Blocks[row, column] = new Trap();
                            break;
                        case "Turret":
                            c.Blocks[row, column] = new Turret();
                            break;
                        case "None":
                            c.Blocks[row, column] = null;
                            break;
                        default:
                            throw new Exception("You are bad and should feel bad." + stuff[0] + " isn't a prefab dickwad");
                    }
                    switch (stuff[1])
                    {
                        case "Air":
                            c.Blocks[row, column].Mat = new AirMaterial();
                            break;
                        case "Steel":
                            c.Blocks[row, column].Mat = new SteelMaterial();
                            break;
                        case "Fire":
                            c.Blocks[row, column].Mat = new FireMaterial();
                            break;
                        case "Obsidian":
                            c.Blocks[row, column].Mat = new ObsidianMaterial();
                            break;
                        case "Stone":
                            c.Blocks[row, column].Mat = new StoneMaterial();
                            break;
                        case "Water":
                            c.Blocks[row, column].Mat = new WaterMaterial();
                            break;
                        case "Wood":
                            c.Blocks[row, column].Mat = new WoodMaterial();
                            break;
                        case "AltarMaterial":
                            c.Blocks[row, column].Mat = new AltarMaterial();
                            break;
                        case "Elec":
                            c.Blocks[row, column].Mat = new ElecMaterial();
                            break;
                        case "Learner":
                            c.Blocks[row, column].Mat = new LearnerMaterial();
                            break;
                        case "Linker":
                            c.Blocks[row, column].Mat = new LinkerMaterial();
                            break;
                        case "Mirror":
                            c.Blocks[row, column].Mat = new MirrorMaterial();
                            break;
                        case "Ice":
                            c.Blocks[row, column].Mat = new IceMaterial();
                            break;
                        case "None":
                            break;
                        default:
                            throw new Exception("You are bad and should feel bad." + stuff[1] + " isn't a material dickwad");
                    }
                    switch (stuff[2])
                    {
                        case "0":
                            break;
                        case "1":
                            c.Blocks[row, column].Rotate(90.0f, Vector3.up);
                            break;
                        case "2":
                            c.Blocks[row, column].Rotate(180.0f, Vector3.up);
                            break;
                        case "3":
                            c.Blocks[row, column].Rotate(270.0f, Vector3.up);
                            break;
                    }
                }
            }
            manager.Citadels[c._number] = c;
            if (def == "Default")
            {
                manager.CurrentCitadel = c._number;
            }
        }
        public void Delete()
        {
            File.Delete(CitadelFilePath);
            manager.Citadels[_number] = null;
            if (manager.CurrentCitadel == _number)
            {
                for (int i = 0; i < 2; i++)
                {
                    if (manager.Citadels[i] != null)
                    {
                        manager.CurrentCitadel = _number;
                    }
                }
            }
        }
        public void SaveToFile()
        {
            if (manager.CurrentCitadel == -1)
            {
                manager.CurrentCitadel = _number;
            }
            List<String> prefabs = new List<string>();
            List<String> materials = new List<String>();
            List<String> rotations = new List<String>();
            string def = _number == manager.CurrentCitadel ? "Default" : "Nope";
            for (int i = 0; i < Blocks.GetLength(0); i++)
            {
                for (int j = 0; j < Blocks.GetLength(1); j++)
                {
                    if (Blocks[i, j] == null)
                    {
                        prefabs.Add("None");
                    }
                    else
                    {
                        Type t = Blocks[i, j].GetType();
                        if (t == typeof(Brick))
                        {
                            prefabs.Add("Brick");
                        }
                        else if (t == typeof(ConcaveCorner))
                        {
                            prefabs.Add("ConcaveCorner");
                        }
                        else if (t == typeof(ConvexCorner))
                        {
                            prefabs.Add("ConvexCorner");
                        }
                        else if (t == typeof(Spire))
                        {
                            prefabs.Add("Spire");
                        }
                        else if (t == typeof(Trap))
                        {
                            prefabs.Add("Trap");
                        }
                        else if (t == typeof(Turret))
                        {
                            prefabs.Add("Turret");
                        }
                        else if (t == typeof(Flag))
                        {
                            prefabs.Add("Flag");
                        }
                        else if (t == typeof(Altar))
                        {
                            prefabs.Add("Altar");
                        }
                        else
                        {
                            throw new Exception("Add this type to this queer ass pretend switch statement dickfuck");
                        }
                    }
                    if (Blocks[i, j] == null || Blocks[i, j].Mat == null)
                    {
                        materials.Add("None");
                    }
                    else
                    {
                        Type t2 = Blocks[i, j].Mat.GetType();
                        if (t2 == typeof(AirMaterial))
                        {
                            materials.Add("Air");
                        }
                        else if (t2 == typeof(SteelMaterial))
                        {
                            materials.Add("Steel");
                        }
                        else if (t2 == typeof(FireMaterial))
                        {
                            materials.Add("Fire");
                        }
                        else if (t2 == typeof(ObsidianMaterial))
                        {
                            materials.Add("Obsidian");
                        }
                        else if (t2 == typeof(StoneMaterial))
                        {
                            materials.Add("Stone");
                        }
                        else if (t2 == typeof(WaterMaterial))
                        {
                            materials.Add("Water");
                        }
                        else if (t2 == typeof(WoodMaterial))
                        {
                            materials.Add("Wood");
                        }
                        else if (t2 == typeof(ElecMaterial))
                        {
                            materials.Add("Elec");
                        }
                        else if (t2 == typeof(LearnerMaterial))
                        {
                            materials.Add("Learner");
                        }
                        else if (t2 == typeof(LinkerMaterial))
                        {
                            materials.Add("Linker");
                        }
                        else if (t2 == typeof(MirrorMaterial))
                        {
                            materials.Add("Mirror");
                        }
                        else if (t2 == typeof(IceMaterial))
                        {
                            materials.Add("Ice");
                        }
                        else if (t2 == typeof(AltarMaterial))
                        {
                            materials.Add("AltarMaterial");
                        }
                        else
                        {
                            throw new Exception("Add this type to this queer ass pretend switch statement dickfuck");
                        }
                    }
                    if (Blocks[i, j] != null)
                    {
                        Quaternion angle = Blocks[i, j].Rotation;
                        Vector3 euler = angle.eulerAngles;
                        int val = (int)(euler.y / 90.0f);
                        rotations.Add("" + val);
                    }
                    else
                    {
                        rotations.Add("0");
                    }

                }
            }
            List<String> lines = new List<String>();
            lines.Add(_number + "$CASH$" + def);
            for (int i = 0; i < prefabs.Count; i++)
            {
                if (i == materials.Count)
                {
                    Debug.Log("Not enough materials");
                }
                if (i == rotations.Count)
                {
                    Debug.Log("Not enough rotations");
                }
                lines.Add(prefabs[i] + "$CASH$" + materials[i] + "$CASH$" + rotations[i]);
            }
            File.WriteAllLines(CitadelFilePath, lines.ToArray());
        }
    }
}
