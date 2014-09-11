using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class CJPyroclasticMaterialEditor : MaterialEditor {
	
	bool prevscatter; bool scattering;
	int prevoctaves; int octaves;
	int prevquality; int quality;
	bool advanced;
	string[] qualitySel = new[] {"QUALITY_LOW", "QUALITY_MED", "QUALITY_HIGH"};
	bool changed = false;
	Material targetMat;
	
	public override void Awake() {
		targetMat = target as Material;
	}
	
	public override void OnInspectorGUI() {
		GUILayout.Space(6);
		float controlSize = 64;
		EditorGUIUtility.LookLikeControls(Screen.width - controlSize - 20);
		
		targetMat = target as Material;
		string[] keyWords = targetMat.shaderKeywords;
		SetTexture("_RampTex", TextureProperty("_RampTex", "Color Ramp", ShaderUtil.ShaderPropertyTexDim.TexDim2D));
		SetFloat("_Radius", FloatProperty("_Radius", "Radius"));
		SetFloat("_Heat", FloatProperty("_Heat", "Heat"));
		//bool octaves = ArrayUtility.Contains(keyWords, "OCTAVES_1");
		
		//checkbox for scattering
		scattering = !ArrayUtility.Contains(keyWords, "SCATTERING_OFF");
		scattering = EditorGUILayout.Toggle("Scattering", scattering);
		if (scattering != prevscatter) {
			changed = true;
			prevscatter = scattering;
		}
		
		//handle noise octave count
		octaves = EditorGUILayout.Popup ("Octaves", OctavesValue()-1, new[] {"1", "2", "3", "4", "5"})+1; //the +/- 1 is transferring array index to actual number
        if (octaves != prevoctaves) {
            changed = true;
			prevoctaves = octaves;
        }
		
		//handle quality
		quality = EditorGUILayout.Popup ("Quality", Quality(), new[] {"Low", "Medium", "High"});
        if (quality != prevquality) {
            changed = true;
			prevquality = quality;
        }
		
		advanced = EditorGUILayout.Foldout(advanced, "Advanced");
		if (advanced) {
			EditorGUILayout.LabelField("Noise Texture:");
			SetTexture("_MainTex", TextureProperty("_MainTex", "Check the readme for requirements", ShaderUtil.ShaderPropertyTexDim.TexDim2D));
			SetVector("_SpherePos", VectorProperty("_SpherePos", "Sphere Position"));
		}
				
		if (changed) {
			var newKeywords = new List<string> {scattering ? "SCATTERING_ON" : "SCATTERING_OFF", "OCTAVES_" + octaves, qualitySel[quality]};
			targetMat.shaderKeywords = newKeywords.ToArray();
			EditorUtility.SetDirty (targetMat);
		}
	}
	
	int OctavesValue() {
		for (int n = 1; n <= 5; ++n) {
			if (ArrayUtility.Contains(targetMat.shaderKeywords, "OCTAVES_"+n)) return n;
		}
		return 4;
	}
	
	int Quality() {
		if (ArrayUtility.Contains(targetMat.shaderKeywords, "QUALITY_LOW")) return 0;
		if (ArrayUtility.Contains(targetMat.shaderKeywords, "QUALITY_MED")) return 1;
		if (ArrayUtility.Contains(targetMat.shaderKeywords, "QUALITY_HIGH")) return 2;
		return 2;
	}
}