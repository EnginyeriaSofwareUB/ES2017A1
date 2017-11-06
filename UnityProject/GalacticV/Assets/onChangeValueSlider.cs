using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class onChangeValueSlider : MonoBehaviour {

	public void SetMasterVolume(Slider slider)
	{
		SoundManager.instance.SetMasterVolume(slider.value);
	}

	public void SetMusicVolume(Slider slider)
	{
		SoundManager.instance.SetMusicVolume(slider.value);
	}
}
