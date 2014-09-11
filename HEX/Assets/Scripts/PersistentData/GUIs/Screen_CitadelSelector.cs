using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets;
using Assets.CitadelBuilder;
using UnityEngine;
using System.IO;


class Screen_CitadelSelector : HScreen
{
    InterfaceTextureSet its;

     protected override void StartCritical()
     {
         its = new InterfaceTextureSet();
         its.Add("new0", new InterfaceTexture("UI_Elements/CitadelBuilder/UI_New", new Rect(_standardWidth * 1.25f, _standardHeight * 2f, _standardWidth * 1.5f, _standardHeight*4f), () => { NewCitadel(0); }));
         its.Add("load0", new InterfaceTexture("UI_Elements/CitadelBuilder/UI_Load", new Rect(_standardWidth * 1.25f, _standardHeight * 2f, _standardWidth * 1.5f, _standardHeight * 4f), () => { LoadCitadel(0); }));
         its.Add("delete0", new InterfaceTexture("UI_Elements/UI_DeleteButton", new Rect(_standardWidth*1.5f, _standardHeight * 6.5f, _standardWidth, _standardHeight), () => { DeleteCitadel(0); }));
         its.Add("name0", new InterfaceTexture("UI_Elements/UI_ButtonGeneralTemplate", new Rect(_standardWidth * 1.5f, _standardHeight, _standardWidth, _standardHeight), () => { RenameOn(0); }));
         if (manager.Citadels[0] == null)
         {
             its.GetByName("load0").Visible = false;
             its.GetByName("delete0").Visible = false;
             its.GetByName("name0").Visible = false;
         }
         else
         {
             its.GetByName("new0").Visible = false;
             its.GetByName("name0").Text = manager.Citadels[0].Name;
         }
         its.Add("new1", new InterfaceTexture("UI_Elements/CitadelBuilder/UI_New", new Rect(_standardWidth * 3.25f, _standardHeight * 2f, _standardWidth * 1.5f, _standardHeight * 4f), () => { NewCitadel(1); }));
         its.Add("load1", new InterfaceTexture("UI_Elements/CitadelBuilder/UI_Load", new Rect(_standardWidth * 3.25f, _standardHeight * 2f, _standardWidth * 1.5f, _standardHeight * 4f), () => { LoadCitadel(1); }));
         its.Add("delete1", new InterfaceTexture("UI_Elements/UI_DeleteButton", new Rect(_standardWidth * 3.5f, _standardHeight * 6.5f, _standardWidth, _standardHeight), () => { DeleteCitadel(1); }));
         its.Add("name1", new InterfaceTexture("UI_Elements/UI_ButtonGeneralTemplate", new Rect(_standardWidth * 3.5f, _standardHeight, _standardWidth, _standardHeight), () => { RenameOn(1); }));
         if (manager.Citadels[1] == null)
         {
             its.GetByName("load1").Visible = false;
             its.GetByName("delete1").Visible = false;
             its.GetByName("name1").Visible = false;
         }
         else
         {
             its.GetByName("new1").Visible = false;
             its.GetByName("name1").Text = manager.Citadels[1].Name;
         }
         its.Add("new2", new InterfaceTexture("UI_Elements/CitadelBuilder/UI_New", new Rect(_standardWidth * 5.25f, _standardHeight * 2f, _standardWidth * 1.5f, _standardHeight * 4f), () => { NewCitadel(2); }));
         its.Add("load2", new InterfaceTexture("UI_Elements/CitadelBuilder/UI_Load", new Rect(_standardWidth * 5.25f, _standardHeight * 2f, _standardWidth * 1.5f, _standardHeight * 4f), () => { LoadCitadel(2); }));
         its.Add("delete2", new InterfaceTexture("UI_Elements/UI_DeleteButton", new Rect(_standardWidth * 5.5f, _standardHeight * 6.5f, _standardWidth, _standardHeight), () => { DeleteCitadel(2); }));
         its.Add("name2", new InterfaceTexture("UI_Elements/UI_ButtonGeneralTemplate", new Rect(_standardWidth * 5.5f, _standardHeight, _standardWidth, _standardHeight), () => { RenameOn(2); }));
         if (manager.Citadels[2] == null)
         {
             its.GetByName("load2").Visible = false;
             its.GetByName("delete2").Visible = false;
             its.GetByName("name2").Visible = false;
         }
         else
         {
             its.GetByName("new2").Visible = false;
             its.GetByName("name2").Text = manager.Citadels[2].Name;
         }
         its.Add(new InterfaceTexture("UI_Elements/UI_BackButton", new Rect(Screen.width * .01f, Screen.width * .01f, Screen.width * .1f, Screen.height * .1f), () => manager.screenManager.ActiveScreen = manager.screenManager.ActiveScreen = new Screen_Dropdown()));
     }
     private bool renameOn0 = false;
     private bool renameOn1 = false;
     private bool renameOn2 = false;
     private void RenameCitadel(int selectedCitadel)
     {
          
         Rect textfieldRect = new Rect(_standardWidth * (1.5f + 2 * (float)selectedCitadel), _standardHeight, _standardWidth, _standardHeight);
         its.GetByName("name" + selectedCitadel).Text = GUI.TextField(textfieldRect, its.GetByName("name" + selectedCitadel).Text, 24);
         if (Input.GetMouseButtonDown(0) && !its.GetByName("name"+selectedCitadel).DrawRect.Contains(InputHandler.MousePosition))
         {
             RenameOff(selectedCitadel);
             manager.Citadels[selectedCitadel].RenameCitadel(its.GetByName("name" + selectedCitadel).Text);
         }
     }
     private void RenameOn(int selectedCitadel)
     {
         switch (selectedCitadel)
         {
             case 1:
                 renameOn1 = true;
                 break;
             case 2:
                 renameOn2 = true;
                 break;
             case 0:
                 renameOn0 = true;
                 break;
         }
     }
     private void RenameOff(int selectedCitadel)
     {
         switch (selectedCitadel)
         {
             case 1:
                 renameOn1 = false;
                 break;
             case 2:
                 renameOn2 = false;
                 break;
             case 0:
                 renameOn0 = false;
                 break;
         }
     }
     private void NewCitadel(int selectedCitadel)
     {
         manager.Citadels[selectedCitadel] = new Citadel(selectedCitadel, "Citadel"+selectedCitadel); 
         Application.LoadLevel("Builder");
         manager.screenManager.ActiveScreen = new Screen_CitadelBuilder(selectedCitadel); 
         manager.screenManager.PreviousScreen = new Screen_CitadelSelector();
     }
     private void LoadCitadel(int selectedCitadel)
     {
         Application.LoadLevel("Builder"); 
         manager.screenManager.ActiveScreen = new Screen_CitadelBuilder(selectedCitadel, manager.Citadels[selectedCitadel].Name);
         manager.screenManager.PreviousScreen = new Screen_CitadelSelector();
     }
     private void DeleteCitadel(int citadel)
     {
         manager.Citadels[citadel].Delete();
         if (manager.CurrentPlayer.DefaultCitadel == citadel)
         {
             for (int i = 0; i < manager.Citadels.Length; i++)
             {
                 if (manager.Citadels[i] != null)
                 {
                     manager.CurrentPlayer.DefaultCitadel = i;              
                 }
             }
             if (manager.CurrentPlayer.DefaultCitadel == citadel)
             {
                 manager.CurrentPlayer.DefaultCitadel = -1;
             }
         }
         its.GetByName("new" + citadel).Visible = true;
         its.GetByName("delete" + citadel).Visible = false;
         its.GetByName("load" + citadel).Visible = false;
         its.GetByName("name" + citadel).Visible = false;
     }

    protected override void UpdateCritical()
    {
        its.Update();        
    }

    protected override void DrawCritical()
    {
        its.Draw();
        if (renameOn0)
        {
            RenameCitadel(0);
        }
        if (renameOn1)
        {
            RenameCitadel(1);
        }
        if (renameOn2)
        {
            RenameCitadel(2);
        }

    }
}

