using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class onChangeValueSlider : MonoBehaviour {

	void OnEnable()
	{
		if (this.name.Contains("1"))//Music
		{
			this.GetComponent<Slider>().value = SoundManager.instance.GetMusicVolume();
		}else if (this.name.Contains("2"))//Effect
		{
			this.GetComponent<Slider>().value = SoundManager.instance.GetEffectVolume();
		}
		else//master
		{
			this.GetComponent<Slider>().value = SoundManager.instance.GetMasterVolume();
		}
	}

	public void SetMasterVolume(Slider slider)
	{
		if (slider)
		{
			SoundManager.instance.SetMasterVolume(slider.value);
		}
		
	}

	public void SetMusicVolume(Slider slider)
	{
		if (slider)
		{
			SoundManager.instance.SetMusicVolume(slider.value);
		}
	}

	public void SetEffectValue(Slider slider)
	{
		if (slider)
		{
			SoundManager.instance.SetEffectVolume(slider.value);
		}
	}
}
