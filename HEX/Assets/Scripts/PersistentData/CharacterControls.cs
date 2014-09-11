using UnityEngine;

namespace Assets
{
	class CharacterControls : Controls
	{

        public CharacterControls()
            : base("CharacterControls")
        {
            // File wasn't found and keymap never got any controls
            if (keyMap.Count == 0)
            {
                keyMap.Add("Up", new Control(KeyCode.W));
                keyMap.Add("Left", new Control(KeyCode.A));
                keyMap.Add("Down", new Control(KeyCode.S));
                keyMap.Add("Right", new Control(KeyCode.D));
                keyMap.Add("Action1", new Control(KeyCode.Mouse0));
                keyMap.Add("Action2", new Control(KeyCode.Mouse1));
                keyMap.Add("Pause", new Control(KeyCode.Escape));

                SaveKeyMap();
            }
        }

        protected override void OnButtonPressed(string button)
        {

            if (button.Equals("Up"))
            {
                // Move Character Up
            }

            if (button.Equals("Left"))
            {
                // Move Character Left
            }

            if (button.Equals("Down"))
            {
                // Move Character Down
            }

            if (button.Equals("Right"))
            {
                // Move Character Right
            }



            if (button.Equals("Action1"))
            {
                // Left Click or 1 Finger Tap
            }

            if (button.Equals("Action2"))
            {
                // Right Click or 2 Finger Tap
            }
        }
	}
}
