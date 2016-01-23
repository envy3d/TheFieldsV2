using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SceneColorControl : MonoBehaviour 
{
	public bool calibrationMode				= false;
	public Material skyMaterial;
	public Color sceneColor 				= Color.gray;
	public Color defaultSceneColor 			= Color.gray;
	public Color defaultFogColor 			= Color.gray;
	public float fogDensity 				= 0.01f;
	public Light keyLight;
	public float keyLightIntensity 			= 0.5f;
	public Color keyLightColor 				= Color.white;
	public float ambientLightInfluence		= 0.75f;
	public float fogColorInfluence			= 0.75f;
	public float keylightColorInfluence 	= 0.0f;
	
	bool colorsInResetPosition = false;
	
	void Update () 
	{
		
		if(!calibrationMode)
		{
			UpdateSceneColors();
		}
		else
		{
			ResetColors();
			Debug.Log ("Please set Default Scene Color and Default Fog Color. Exit Calibration Mode when ready.");
		}
		
			keyLight.intensity = keyLightIntensity + sceneColor.grayscale * keylightColorInfluence - keylightColorInfluence;
			keyLight.color = keyLightColor + sceneColor * keylightColorInfluence;
			
	}
	
	void ResetColors()
	{
		RenderSettings.skybox.SetColor ("_Tint", Color.gray);
		RenderSettings.fogColor = Color.gray;
	}
	
	void OnValidate()
	{
		RenderSettings.skybox = skyMaterial;
	}
	
	void UpdateSceneColors()
	{
		RenderSettings.ambientLight = Vector4.Lerp(defaultSceneColor, sceneColor, ambientLightInfluence);
		
		Color skyColorDifference = defaultSceneColor - sceneColor;
		Color skyTint = (skyColorDifference * -0.5f + Color.gray);
		Color fogTint = 2f * defaultFogColor * (skyColorDifference * -0.5f + Color.gray);
		
		RenderSettings.skybox.SetColor ("_Tint", skyTint);
		float keylightIsDirectional = 0f;
		if(keyLight.type == LightType.Directional){keylightIsDirectional = 1f;}

		RenderSettings.fogColor = Vector4.Lerp(defaultFogColor, 0.5f * (fogTint + sceneColor) + skyColorDifference*0.5f*(keyLight.color * keyLight.intensity * System.Convert.ToSingle(keyLight.enabled)) * keylightIsDirectional,fogColorInfluence);
		RenderSettings.fogDensity = fogDensity * (RenderSettings.fogColor.grayscale + 1f + 0.2f*(keyLight.color.grayscale * keyLight.intensity * System.Convert.ToSingle(keyLight.enabled)) * keylightIsDirectional);
	}
}
