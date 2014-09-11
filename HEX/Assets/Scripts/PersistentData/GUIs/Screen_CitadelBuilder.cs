using UnityEngine;
using System.Collections;
using Assets.CitadelBuilder;

public class Screen_CitadelBuilder : HScreen
{
    ScrollingInterfaceTextureSet _materials;
    ScrollingInterfaceTextureSet _prefabs;
    InterfaceTextureSet _otherButtons;
    MousePick _picker;
    int _activeCitadel;
    string _name;

    public Screen_CitadelBuilder(int activeCitadel, string name = "")
        : base()
    {
        _activeCitadel = activeCitadel;
        _name = name == "" ? "Citadel"+activeCitadel : name;
    }
   

    protected override void StartCritical()
    {
        _picker = GameObject.Find("Main Camera").GetComponent<MousePick>();
        Debug.Log(base.manager.Citadels[_activeCitadel] == null);
        _picker.ImportFromCitadel(base.manager.Citadels[_activeCitadel]);
        _otherButtons = new InterfaceTextureSet();
        _materials = new ScrollingInterfaceTextureSet(new Rect(Screen.width * .8f - ScrollingInterfaceTexture.scrollOffset, 0, Screen.width * .2f, Screen.height * .75f));
        _prefabs = new ScrollingInterfaceTextureSet(new Rect(0, 0, Screen.width * .2f, Screen.height * .75f));
        _materials.Add("Wood", new InterfaceTexture("UI_Elements/CitadelBuilder/Wood", new Rect(Screen.width * .8f, 0, Screen.width * .2f, Screen.height * .25f), SetMaterial<WoodMaterial>));
        _materials.Add("Stone", new InterfaceTexture("UI_Elements/CitadelBuilder/Stone", new Rect(Screen.width * .8f, Screen.height * .25f, Screen.width * .2f, Screen.height * .25f), SetMaterial<StoneMaterial>));
        _materials.Add("Fire", new InterfaceTexture("UI_Elements/CitadelBuilder/Fire", new Rect(Screen.width * .8f, Screen.height * .50f, Screen.width * .2f, Screen.height * .25f), SetMaterial<FireMaterial>));
        _materials.Add("Air", new InterfaceTexture("UI_Elements/CitadelBuilder/Air", new Rect(Screen.width * .8f, Screen.height * .75f, Screen.width * .2f, Screen.height * .25f), SetMaterial<AirMaterial>));
        _materials.Add("Water", new InterfaceTexture("UI_Elements/CitadelBuilder/Water", new Rect(Screen.width * .8f, Screen.height * 1.0f, Screen.width * .2f, Screen.height * .25f), SetMaterial<WaterMaterial>));
        _materials.Add("Elec", new InterfaceTexture("UI_Elements/CitadelBuilder/Electricity", new Rect(Screen.width * .8f, Screen.height * 1.25f, Screen.width * .2f, Screen.height * .25f), SetMaterial<ElecMaterial>));
        _materials.Add("Ice", new InterfaceTexture("UI_Elements/CitadelBuilder/Ice", new Rect(Screen.width * .8f, Screen.height * 1.50f, Screen.width * .2f, Screen.height * .25f), SetMaterial<IceMaterial>));
        _materials.Add("Steel", new InterfaceTexture("UI_Elements/CitadelBuilder/Steel", new Rect(Screen.width * .8f, Screen.height * 1.75f, Screen.width * .2f, Screen.height * .25f), SetMaterial<SteelMaterial>));
        _materials.Add("Obsidian", new InterfaceTexture("UI_Elements/CitadelBuilder/Obsidian", new Rect(Screen.width * .8f, Screen.height * 2.00f, Screen.width * .2f, Screen.height * .25f), SetMaterial<ObsidianMaterial>));
        _materials.Add("Linker", new InterfaceTexture("UI_Elements/CitadelBuilder/Linker", new Rect(Screen.width * .8f, Screen.height * 2.25f, Screen.width * .2f, Screen.height * .25f), SetMaterial<LinkerMaterial>));
        _materials.Add("Learner", new InterfaceTexture("UI_Elements/CitadelBuilder/Learner", new Rect(Screen.width * .8f, Screen.height * 2.5f, Screen.width * .2f, Screen.height * .25f), SetMaterial<LearnerMaterial>));
        _materials.Add("Mirror", new InterfaceTexture("UI_Elements/CitadelBuilder/Mirror", new Rect(Screen.width * .8f, Screen.height * 2.75f, Screen.width * .2f, Screen.height * .25f), SetMaterial<MirrorMaterial>));

        SetMaterial<WoodMaterial>();
        _materials.GetByName("Linker").Active = false;
        _materials.GetByName("Learner").Active = false;
        _materials.GetByName("Mirror").Active = false;
        switch (manager.CurrentPlayer.UnlockedMaterial)
        {
            case UnlockMats.Learner:
                _materials.GetByName("Learner").Active = true;
                break;
            case UnlockMats.Linker:
                _materials.GetByName("Linker").Active = true;
                break;
            case UnlockMats.Mirror:
                _materials.GetByName("Mirror").Active = true;
                break;
        }

        _prefabs.Add("Brick", new InterfaceTexture("UI_Elements/CitadelBuilder/BasicBlock", new Rect(0, 0, Screen.width * .2f, Screen.height * .25f), SetPrefab<Brick>));
        _prefabs.Add("ConvexCorner", new InterfaceTexture("UI_Elements/CitadelBuilder/ConvexBlock", new Rect(0, Screen.height * .25f, Screen.width * .2f, Screen.height * .25f), SetPrefab<ConvexCorner>));
        _prefabs.Add("ConcaveCorner", new InterfaceTexture("UI_Elements/CitadelBuilder/ConcaveBlock", new Rect(0, Screen.height * .50f, Screen.width * .2f, Screen.height * .25f), SetPrefab<ConcaveCorner>));
        _prefabs.Add("Turret", new InterfaceTexture("UI_Elements/CitadelBuilder/Turret", new Rect(0, Screen.height * .75f, Screen.width * .2f, Screen.height * .25f), SetPrefab<Turret>));
        _prefabs.Add("Trap", new InterfaceTexture("UI_Elements/CitadelBuilder/Trap", new Rect(0, Screen.height * 1.00f, Screen.width * .2f, Screen.height * .25f), SetPrefab<Trap>));
        _prefabs.Add("Flag", new InterfaceTexture("UI_Elements/CitadelBuilder/Flag", new Rect(0, Screen.height * 1.25f, Screen.width * .2f, Screen.height * .25f), SetPrefab<Flag>));


        SetPrefab<Brick>();
        _prefabs.GetByName("Turret").Active = false;
        _prefabs.GetByName("Trap").Active = false;
        _prefabs.GetByName("Flag").Active = false;

        switch (manager.CurrentPlayer.UnlockedBlock)
        {
            case UnlockBlocks.Turret:
                _prefabs.GetByName("Turret").Active = true;
                break;
            case UnlockBlocks.Trap:
                _prefabs.GetByName("Trap").Active = true;
                break;
            case UnlockBlocks.Flag:
                _prefabs.GetByName("Flag").Active = true;
                break;
        }

        _otherButtons.Add(new InterfaceTexture("UI_Elements/CitadelBuilder/UI_RotateHorizontal", new Rect(0.00f, Screen.height * .89f, Screen.width * .1f, Screen.height * .08f), () => _picker.camera.transform.RotateAround(new Vector3(0, 0, 0), Vector3.up, 45)));
        _otherButtons.Add(new InterfaceTexture("UI_Elements/CitadelBuilder/UI_RotateVertical", new Rect(0.00f, Screen.height * .78f, Screen.width * .1f, Screen.height * .08f), () => { GridHandler.RotateX(_picker.camera); }));
        _otherButtons.Add("trash", new InterfaceTexture("UI_Elements/CitadelBuilder/UI_Trash", new Rect(Screen.width * .1f, Screen.height * .78f, Screen.width * .1f, Screen.height * .16f), _picker.Trashcan));
        _otherButtons.Add(new InterfaceTexture("UI_Elements/CitadelBuilder/UI_CP", new Rect(Screen.width * .8f, Screen.height * .76f, Screen.width * .08f, Screen.height * .1f)));
        _otherButtons.Add("cp", new InterfaceTexture("UI_Elements/UI_ButtonGeneralTemplate", new Rect(Screen.width * .8f, Screen.height * .875f, Screen.width * .08f, Screen.height * .1f)));
        _otherButtons.Add("save", new InterfaceTexture("UI_Elements/UI_SaveButton", new Rect(Screen.width * .9f, Screen.height * .76f, Screen.width * .08f, Screen.height * .1f), () => { _picker.ExportToCitadel(_name, _activeCitadel); }));
        _otherButtons.Add(new InterfaceTexture("UI_Elements/UI_BackButton", new Rect(Screen.width * .9f, Screen.height * .875f, Screen.width * .08f, Screen.height * .1f), delegate() { ExportToFile(); Application.LoadLevel("Main"); GameObject.Find("Persistence").GetComponent<PersistentData>().screenManager.ActiveScreen = GameObject.Find("Persistence").GetComponent<PersistentData>().screenManager.PreviousScreen; }));
    }
    protected override void DrawCritical()
    {
        _materials.Draw();
        _prefabs.Draw();
        _otherButtons.Draw();
    }
    protected override void UpdateCritical()
    {
        _materials.Update();
        _prefabs.Update();
        _otherButtons.Update();
        _otherButtons.GetByName("cp").Text = _picker.ConstructionPoints+"";
        if (_picker.CurrentSelector == TrashcanSelector.Get)
        {
            _otherButtons.GetByName("trash").Selected = true;
        }
        else
        {
            _otherButtons.GetByName("trash").Selected = false;
        }
        if (_picker.Changed)
        {
            _otherButtons.GetByName("save").Active = true;
        }
        else
        {
            _otherButtons.GetByName("save").Active = false;
        }
    }
    private void SetMaterial<E>() where E : CMaterial
    {
        if (_picker.CurrentSelector != ActivePlacer.Get)
        {
            _picker.CurrentSelector = ActivePlacer.Get;
        }
        ActivePlacer.Get.MaterialSelected = CMaterial.TypeToMat(typeof(E));
        string[] parse = typeof(E).ToString().Split('.');
        string typer = parse[parse.Length - 1];
        typer = typer.Remove(typer.Length - 8);
        _materials.Selected = _materials.GetByName(typer);
    }
    private void SetPrefab<E>() where E : CitadelBlock
    {
        if (_picker.CurrentSelector != ActivePlacer.Get)
        {
            _picker.CurrentSelector = ActivePlacer.Get;
        }
        ActivePlacer.Get.PrefabSelected = CitadelBlock.TypeToBlock(typeof(E));
        string[] parse = typeof(E).ToString().Split('.');
        string typer = parse[parse.Length - 1];
        Debug.Log(typer);
        _prefabs.Selected = _prefabs.GetByName(typer);
    }
    public void ExportToFile()
    {
        GameObject.Find("Persistence").GetComponent<PersistentData>().Citadels[_activeCitadel].SaveToFile();
        if (manager.CurrentPlayer.DefaultCitadel == -1)
        {
            manager.CurrentPlayer.DefaultCitadel = _activeCitadel;
            manager.CurrentPlayer.Save();
        }
    }
    
  
}
