//using UnityEngine;
//using System.Collections;
//using System.IO;
//using Assets.CitadelBuilder;


//Note: This will eventually be used the first time they open the game

//public class Screen_Splash : HScreen
//{
//    private Texture enterButton;
//    private Texture hexLogo;
	
//    public Screen_Splash ()
//        : base ()
//    {
//        hexLogo = Resources.Load("Textures/HexLogo") as Texture;
//        enterButton = Resources.Load("Textures/EnterButton") as Texture;
//    }
	
//    public override void Start ()
//    {
//        /*
//        if (File.Exists(Profile.BASE_PATH + "Last_Profile"))
//        {
//            using (FileStream file = File.OpenRead(Profile.BASE_PATH + "Last_Profile"))
//            {
//                using (StreamReader reader = new StreamReader(file))
//                {
//                    string profileName = reader.ReadLine();
//                    manager.profile = new Profile(profileName).LoadProfile();
//                }
//            }
//        }
//         */
        
//    }
	
//    public override void Update ()
//    {
		
//    }
	
//    public override void Draw ()
//    {
//        if(hexLogo != null)
//            GUI.DrawTexture(new Rect(Screen.width / 2 - (300*Screen.width / 1366),
//                (50*Screen.height / 597), (600*Screen.width / 1366), (225*Screen.height / 597)), hexLogo, ScaleMode.ScaleToFit);
//        if(enterButton != null)
//            GUI.DrawTexture(new Rect(Screen.width / 2 - (250*Screen.width / 1366),
//                Screen.height - (100*Screen.height/597), (500*Screen.width / 1366), (75*Screen.height/597)), enterButton, ScaleMode.ScaleToFit);
//    }
	
//    override protected void OnButtonPressed (string button)
//    {
//        switch (button)
//        {
//        case "Enter":
//            Application.LoadLevel ("Main");
//                if(base.manager.profile == null)
//                {

//                }
//                else
//                {
//                    base.manager.screenManager.ActiveScreen = new Screen_CharacterSelect();
//                    base.manager.screenManager.PreviousScreen = new Screen_ProfileSelect();
//                }
//            break;
//        case "Back":
//            Application.Quit();
//            break;
//        }
//    }
//}
