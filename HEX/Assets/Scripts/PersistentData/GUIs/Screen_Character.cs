using UnityEngine;
using System.Collections.Generic;
using Assets;

public class Screen_Character : HScreen
{
    InterfaceTextureSet[] TalentList;
    HashSet<InterfaceTexture> OtherButtons;
    InterfaceTexture Description;
    private Point _selection;
    private string[] _talentNames;
    private string _information;
    private int[] _selectedTalents;

    private GUISkin _skin;

    static Screen_Character()
    {
    }

    protected override void StartCritical()
    {    
        OtherButtons = new HashSet<InterfaceTexture>();
        TalentList = new InterfaceTextureSet[10];
        _selectedTalents = new int[TalentList.Length];
        for (int k = 0; k < TalentList.Length; k++)
        {
            _selectedTalents[k] = 3;
            int i = k;
            int j = i + 1;
            Talent[] talents = TalentDictionary.Talents[i];
            Debug.Log(talents.Length);
            TalentList[i] = new InterfaceTextureSet();
            TalentList[i].Add("level", new InterfaceTexture("UI_Elements/UI_ButtonGeneralTemplate", new Rect(_standardWidth * 2.75f, _standardHeight * (.5f + i * (.7f + .05f)), _standardWidth*.5f, _standardHeight * .7f), "Level "+j));
            TalentList[i].Add("icon1", new InterfaceTexture(talents[0].Icon, new Rect(_standardWidth * 3.30f, _standardHeight * (.5f + i * (.7f + .05f)), _standardWidth * .5f, _standardHeight * .7f), () => { SelectTalent(i, 1); }));
            TalentList[i].Add("name1", new InterfaceTexture("UI_Elements/UI_ButtonGeneralTemplate", new Rect(_standardWidth * 3.80f, _standardHeight * (.5f + i * (.7f + .05f)), _standardWidth, _standardHeight * .7f), talents[0].Name, () => { SelectTalent(i, 1); }));
            TalentList[i].Add("icon2", new InterfaceTexture(talents[1].Icon, new Rect(_standardWidth * 4.85f, _standardHeight * (.5f + i * (.7f + .05f)), _standardWidth * .5f, _standardHeight * .7f), () => { SelectTalent(i, 2); }));
            TalentList[i].Add("name2", new InterfaceTexture("UI_Elements/UI_ButtonGeneralTemplate", new Rect(_standardWidth * 5.35f, _standardHeight * (.5f + i * (.7f + .05f)), _standardWidth, _standardHeight * .7f), talents[1].Name, () => { SelectTalent(i, 2); }));
            TalentList[i].Add("icon3", new InterfaceTexture(talents[2].Icon, new Rect(_standardWidth * 6.40f, _standardHeight * (.5f + i * (.7f + .05f)), _standardWidth * .5f, _standardHeight * .7f), () => { SelectTalent(i, 3); }));
            TalentList[i].Add("name3", new InterfaceTexture("UI_Elements/UI_ButtonGeneralTemplate", new Rect(_standardWidth * 6.90f, _standardHeight * (.5f + i * (.7f + .05f)), _standardWidth, _standardHeight * .7f), talents[2].Name, () => { SelectTalent(i, 3); }));
            if (j > manager.CurrentPlayer.Level)
            {
                TalentList[i].GetByName("level").Active = false;
                TalentList[i].GetByName("icon1").Active = false;
                TalentList[i].GetByName("name1").Active = false;
                TalentList[i].GetByName("icon2").Active = false;
                TalentList[i].GetByName("name2").Active = false;
                TalentList[i].GetByName("icon3").Active = false;
                TalentList[i].GetByName("name3").Active = false;
            }
            for (int l = 1; l <= 3; l++)
            {
                if (TalentDictionary.HasDefaultScript(TalentDictionary.Talents[i][l-1]))
                {
                    
                    TalentList[i].GetByName("icon" + l).Text += "(inactive)";
                }
            }
        }
        Description = new InterfaceTexture("UI_Elements/UI_ButtonGeneralTemplate", new Rect(_standardWidth*.125f, _standardHeight*.5f, _standardWidth * 2.5f, _standardHeight * 5.75f));
        Description.Visible = false;
        OtherButtons.Add(new InterfaceTexture("UI_Elements/UI_SaveButton", new Rect(1.0125f * _standardWidth, _standardHeight * 6.5f, _standardWidth*.75f, _standardHeight), SaveTalents));
        OtherButtons.Add(new InterfaceTexture("UI_Elements/UI_BackButton", new Rect(.125f * _standardWidth, _standardHeight*6.5f, _standardWidth*.75f, _standardHeight), () => { manager.screenManager.ActiveScreen = new Screen_Dropdown(); manager.screenManager.PreviousScreen = this; }));
        OtherButtons.Add(new InterfaceTexture("UI_Elements/UI_CancelButton", new Rect(1.9f * _standardWidth, _standardHeight *6.5f, _standardWidth*.75f, _standardHeight), ClearSelection));
        //string guild = manager.CurrentPlayer.Guild.Name;
        //string path = "UI_Elements/CharacterCreation/UI_" + guild;
        //Debug.Log(path);
        //OtherButtons.Add(new InterfaceTexture(path, new Rect(0, 0, _standardWidth * 3, _standardHeight * 4)));

        LoadTalents();
    }
    private void SelectTalent(int talentrow, int talentColumn)
    {
        if (_selectedTalents[talentrow] != talentColumn - 1)
        {
            _selectedTalents[talentrow] = talentColumn - 1;
            TalentList[talentrow].Selected = TalentList[talentrow].GetByName("icon" + talentColumn);
            Description.Visible = true;
            Description.Text = TalentDictionary.Talents[talentrow][talentColumn - 1].Description;
        }
        else
        {
            _selectedTalents[talentrow] = 3;
            TalentList[talentrow].Selected = null;
            Description.Visible = false;
        }
    }
    private void LoadTalents()
    {
        for (int i = 0; i < _selectedTalents.Length; i++)
        {
            if (manager.CurrentPlayer.SelectedTalents[i] != 3)
            {
                 ConfirmTalent(i);
                 string toSelect = "icon" + (manager.CurrentPlayer.SelectedTalents[i]+1);
                 TalentList[i].Selected = TalentList[i].GetByName(toSelect);
            }
        }
    }
    private void SaveTalents()
    {
        for (int i = 0; i < _selectedTalents.Length; i++)
        {
            if (_selectedTalents[i] != 3)
            {
                TalentDictionary.Talents[i][_selectedTalents[i]].Unlock(manager.CurrentPlayer);
                manager.CurrentPlayer.SelectedTalents[i] = _selectedTalents[i];
                ConfirmTalent(i);
            }
        }
        ClearSelection();     
        manager.CurrentPlayer.Save();
    }
    private void ClearSelection()
    {
        for (int i = 0; i < _selectedTalents.Length; i++)
        {
            _selectedTalents[i] = 3;
            //if it's not confirmed, then get rid of that shite
            if (_selectedTalents[i] == manager.CurrentPlayer.SelectedTalents[i])
            {
                TalentList[i].Selected = null;
            }
        }
        Description.Visible = false;
    }
    private void ConfirmTalent(int i)
    {
        TalentList[i].SelectTexture = Resources.Load<Texture>("UI_Elements/confirmed");
        TalentList[i].GetByName("icon1").ClearInteractions();
        TalentList[i].GetByName("icon2").ClearInteractions();
        TalentList[i].GetByName("icon3").ClearInteractions();
        TalentList[i].GetByName("name1").ClearInteractions();
        TalentList[i].GetByName("name2").ClearInteractions();
        TalentList[i].GetByName("name3").ClearInteractions();
    }

    protected override void UpdateCritical()
    {
        foreach (var set in TalentList)
        {
            set.Update();
        }
        foreach (var tex in OtherButtons)
        {
            tex.Update();
        }
        Description.Update();
    }

    protected override void DrawCritical()
    {
        foreach (var set in TalentList)
        {
            set.Draw();
        }
        foreach (var tex in OtherButtons)
        {
            tex.Draw();
        }
        Description.Draw();
    }
}

