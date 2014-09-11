using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using Assets;
using Assets.CitadelBuilder;

public class MousePick : MonoBehaviour
{
    internal int ConstructionPoints { get; private set; }
    internal ISelectable CurrentSelector;
    internal bool Changed;
   

    Dictionary<GameObject, CitadelBlock> ObjToBlock;
    Dictionary<Point, CitadelBlock> BlockPlacements;
    //HashSet<InterfaceTexture> Textures;
    public Light selector;

    private PersistentData manager;

    void Start() 
    {
        ActivePlacer.ReInitialize();
        BlockMover.ReInitialize(selector);

        manager = GameObject.Find("Persistence").GetComponent<PersistentData>();

        ObjToBlock = new Dictionary<GameObject, CitadelBlock>();
        BlockPlacements = new Dictionary<Point, CitadelBlock>();
        ConstructionPoints = manager.CurrentPlayer.ConstructionPoints;
        CurrentSelector = ActivePlacer.Get;

        Changed = false;  
	}
   
    internal void Trashcan()
    {
        if (CurrentSelector == TrashcanSelector.Get)
        {
            CurrentSelector = ActivePlacer.Get;
        }
        else if (CurrentSelector == BlockMover.Get)
        {
            if (ObjToBlock[BlockMover.Get.Block].GetType() != typeof(Altar))
            {
                ConstructionPoints += ObjToBlock[BlockMover.Get.Block].Mat.GetCost(ObjToBlock[BlockMover.Get.Block]);
                BlockPlacements.Remove(BlockMover.Get.BlockSelected);
                ObjToBlock.Remove(BlockMover.Get.Block);
                DestroyImmediate(BlockMover.Get.Block);
                BlockMover.Get.Block = null;
                BlockMover.Get.BlockSelected = default(Point);
                CurrentSelector = ActivePlacer.Get;
            }
        }
        else
        {
            CurrentSelector = TrashcanSelector.Get;
        }
    }

    // Update is called once per frame
    Vector3 defLightPos = new Vector3(-100, -100, -100);
    void Update()
    {    
        if (CurrentSelector != BlockMover.Get && selector.transform.position != defLightPos)
        {
            selector.transform.position = defLightPos;
        }
        RayCastHandling();
    }

    bool displayError = false;
    DateTime timeDuration = default(DateTime);
    


    void OnGUI()
    {      
        int backup = GUI.skin.label.fontSize;
        GUI.skin.label.fontSize = 32;   
        Vector2 size2 = GUI.skin.label.CalcSize(new GUIContent("NOT ENOUGH CONSTRUCTION POINTS"));
        if (displayError)
        {
            if (timeDuration == default(DateTime))
            {
                timeDuration = DateTime.Now;
            }
            else if (DateTime.Now.Subtract(timeDuration).TotalSeconds > 5)
            {
                displayError = false;
                timeDuration = default(DateTime);
            }
            GUI.Label(new Rect(Screen.width * .5f - size2.x/2, 0, size2.x, size2.y), "NOT ENOUGH CONSTRUCTION POINTS");
        }
        GUI.skin.label.fontSize = backup;

    }
    void RayCastHandling()
    {
        var ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit collision;

        if (Physics.Raycast(ray.origin, ray.direction, out collision, 100) && Input.GetMouseButtonDown(0))
        {


            Vector3 colpoint = collision.point;
            Point p = GridHandler.GetPlaceOnGrid(colpoint);
            Debug.Log(p);
            if (collision.collider.gameObject.name == "GridPlane")
            {
                GridHandling(colpoint, p);
            }
            else
            {
                BlockHandling(collision, p);
            }
        }
    }
    void GridHandling(Vector3 colpoint, Point p)
    {
        if (BlockPlacements.ContainsKey(p))
        {
            Debug.Log("This means that you clicked on a grid, and it hit a square where the game thinks there's a block: " + colpoint.ToString());
            return;
        }
        if (CurrentSelector == ActivePlacer.Get)
        {
            Type t = ActivePlacer.Get.PrefabSelected.GetType();
            CitadelBlock newBlock = CitadelBlock.TypeToBlock(t);
            if (ConstructionPoints >= ActivePlacer.Get.MaterialSelected.GetCost(ActivePlacer.Get.PrefabSelected))
            {
                newBlock.Instantiate(ActivePlacer.Get.MaterialSelected, GridHandler.GetPlacePosition(p, colpoint.y), newBlock.Rotation);
                ConstructionPoints -= ActivePlacer.Get.MaterialSelected.GetCost(ActivePlacer.Get.PrefabSelected);
                BlockPlacements.Add(p, newBlock);
                ObjToBlock.Add(newBlock.Prefab, newBlock);
                Changed = true;
            }
            else
            {
                displayError = true;
            }
        }
        else if (CurrentSelector == BlockMover.Get)
        {
            BlockPlacements.Remove(BlockMover.Get.BlockSelected);
            BlockPlacements.Add(p, ObjToBlock[BlockMover.Get.Block]);
            BlockMover.Get.BlockSelected = p;
            BlockMover.Get.Block.transform.position = GridHandler.GetPlacePosition(p, colpoint.y);
            CurrentSelector = ActivePlacer.Get;
            Changed = true;
        }
    }
    void BlockHandling(RaycastHit collision, Point p)
    {
        if (!BlockPlacements.ContainsKey(p))
        {
            Debug.Log("This means that you clicked on a block, but the game does not recognize it: " + collision.point.ToString());
            return;
        }
        if (CurrentSelector == BlockMover.Get && BlockMover.Get.BlockSelected == p)
        {
            ObjToBlock[BlockMover.Get.Block].Rotate(90.0f, Vector3.up);
            Changed = true;
        }
        else if (CurrentSelector == BlockMover.Get)
        {
            BlockMover.Get.BlockSelected = p;
            BlockMover.Get.Block = collision.collider.gameObject;
        }
        else if (CurrentSelector == TrashcanSelector.Get)
        {
            if (ObjToBlock[collision.collider.gameObject].GetType() != typeof(Altar))
            {
                DestroyBlock(collision.collider.gameObject, p);
                Changed = true;
            }
            else
            {
                CurrentSelector = BlockMover.Get;
                BlockMover.Get.BlockSelected = p;
                BlockMover.Get.Block = collision.collider.gameObject;
            }
        }
        else
        {
            CurrentSelector = BlockMover.Get;
            BlockMover.Get.BlockSelected = p;
            BlockMover.Get.Block = collision.collider.gameObject;
        }
    }
    void DestroyBlock(GameObject o, Point p)
    {
        ConstructionPoints += ObjToBlock[o].Mat.GetCost(ObjToBlock[o]);
        BlockPlacements.Remove(p);
        ObjToBlock.Remove(o);
        DestroyImmediate(o);
    }
    internal void ExportToCitadel(string name, int citadel)
    {
        CitadelBlock[,] toReturn = new CitadelBlock[Citadel.NUM_SQUARES, Citadel.NUM_SQUARES];
        for (int i = 0; i < toReturn.GetLength(0); i++)
        {
            for (int j = 0; j < toReturn.GetLength(1); j++)
            {
                Point p = new Point(i - 9, j - 9);
                CitadelBlock c;
                if (BlockPlacements.TryGetValue(p, out c))
                {
                    toReturn[i, j] = c;
                }
                else
                {
                    toReturn[i, j] = null;
                }
            }
        }
        Citadel penismonster = new Citadel(citadel, name);
        penismonster.Blocks = toReturn;
        GameObject.Find("Persistence").GetComponent<PersistentData>().Citadels[citadel] = penismonster;
        Debug.Log("sup from save");
        Changed = false;
    }
    internal void ImportFromCitadel(Citadel cit)
    {
        Debug.Log("sup from load");
        ObjToBlock.Clear();
        BlockPlacements.Clear();
        var ray = camera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        RaycastHit collision;
        Physics.Raycast(ray.origin, ray.direction, out collision, 100);
        bool hasAltar = false;
        for (int i = 0; i < cit.Blocks.GetLength(0); i++)
        {
            for (int j = 0; j < cit.Blocks.GetLength(1); j++)
            {
                if (cit.Blocks[i, j] != null)
                {
                    if (cit.Blocks[i, j].GetType() == typeof(Altar))
                    {
                        if (hasAltar)
                        {
                            throw new Exception("Citadel has two altars, invalid");
                        }
                        else
                        {
                            hasAltar = true;
                        }
                    }
                    var block = CitadelBlock.TypeToBlock(cit.Blocks[i, j].GetType());
                    block.Rotate(cit.Blocks[i, j].Rotation.eulerAngles.y, Vector3.up);
                    Point p = new Point(i - 9, j - 9);
                    var m = cit.Blocks[i, j].Mat;
                    block.Instantiate(m, GridHandler.GetPlacePosition(p, collision.point.y), block.Rotation);
                    BlockPlacements.Add(p, block);
                    ObjToBlock.Add(block.Prefab, block);
                    ConstructionPoints -= m.GetCost(cit.Blocks[i, j]);
                    if (ConstructionPoints < 0)
                    {
                        throw new Exception("Exceeded Potential Costs. Invalid");
                    }
                }
            }
        }
        if (!hasAltar)
        {
            throw new Exception("Citadel has no altars. invalid");
        }

    }
   
}
