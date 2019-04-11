/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEngine;
using System.Collections;

public class FireLightControl : MonoBehaviour {

	private Light fireLight;

	//Range for min/max values of variable
	[Range(0f, 8f)]
	public float minIntensity = 1.5f;
	[Range(0f, 8f)]
	public float maxIntensity = 2.5f;

	float randomValue;

	void Start()
	{
		fireLight = GetComponent<Light> ();
		randomValue = Random.Range(0.0f, 65000f);
	}

	// FireLight Blinking
	void Update()
	{
		float noise = Mathf.PerlinNoise(randomValue, Time.time);
		fireLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
	}
}
