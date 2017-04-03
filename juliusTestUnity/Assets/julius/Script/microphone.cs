using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class microphone : MonoBehaviour {

	public float vol = 0;
	public float[] spectrum;
	public float max = 1;
	public float min = 1;
    AudioSource _audio;

	private float GetAveragedVolume()
	{ 
		float[] data = new float[256];
		float a = 0;
		_audio.GetOutputData(data,0);
		
		foreach(float s in data)
		{
			a += Mathf.Abs(s);
		}
		
		return a / 256;
	}

	// Use this for initialization
	void Start () {
		_audio.clip = Microphone.Start(null, true, 999, 44100);  // マイクからのAudio-InをAudioSourceに流す
		_audio.loop = true;                                      // ループ再生にしておく
		_audio.mute = true;                                      // マイクからの入力音なので音を流す必要がない
		while (!(Microphone.GetPosition("") > 0)){}             // マイクが取れるまで待つ。空文字でデフォルトのマイクを探してくれる
		_audio.Play(); 

		spectrum = new float[1024];
	}

	// Update is called once per frame
	void Update () {
		spectrum = _audio.GetSpectrumData(1024, 0, FFTWindow.BlackmanHarris);
		vol = GetAveragedVolume();

		if(vol > max){
			max = vol;
		}
		if(vol < min){
			min = vol;
		}

	}
}
