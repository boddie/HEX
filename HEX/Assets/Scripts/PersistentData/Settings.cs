using System;
using System.IO;
using System.Collections.Generic;

public class Settings
{
    public float Graphics;
    public float MasterVolume;

    private const string DEFAULT_SETTINGS_FILENAME = "settings";

   // private Profile _belongsTo;
    private string _fileName;

    /// <summary> Full path to the settings file. </summary>
    public string FilePath
    {
        get { return DEFAULT_SETTINGS_FILENAME + _fileName; }
    }

    /// <summary>
    /// Create and/or load a settings file.
    /// </summary>
    /// <param name="belongsTo">Profile the settings file belongs to.</param>
    public Settings() // Profile belongsTo)
    {
        Graphics = 0.0f;
        MasterVolume = 0.0f;

       //  _belongsTo = belongsTo;
        _fileName = DEFAULT_SETTINGS_FILENAME + ".hexings";

        if (File.Exists(DEFAULT_SETTINGS_FILENAME + _fileName))
        {
            Load();
        }
        else
        {
            Save();
        }
        
    }

    /// <summary>
    /// Load the settings file if it already exists.
    /// </summary>
    private void Load()
    {
        using (FileStream file = File.OpenRead(FilePath))
        {
            using (StreamReader reader = new StreamReader(file))
            {
                while (!reader.EndOfStream)
                {
                    string[] entry = reader.ReadLine().Split('=');

                    switch (entry[0])
                    {
                        case "Graphics":
                            Graphics = float.Parse(entry[1]);
                            break;
                        case "Master Volume":
                            MasterVolume = float.Parse(entry[1]);
                            break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Save the settings.
    /// </summary>
    public void Save()
    {
        using (FileStream file = File.OpenWrite(FilePath))
        {
            using (StreamWriter writer = new StreamWriter(file))
            {
                writer.WriteLine("Graphics=" + Graphics);
                writer.WriteLine("Master Volume=" + MasterVolume);
            }
        }
    }
}