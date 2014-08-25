
using System.Collections;
using UnityEngine;

public class TextFadeInOut : MonoBehaviour
{
    public float waitToStart = 0.0f;
	public float fadeDuration = 2.0f;
	public float fadeHoldDuration = 2.0f;
	
	private void Start ()
	{
		StartCoroutine(StartFading());
	}
	
	private IEnumerator StartFading()
	{
        SetToFade(0.0f);
        yield return new WaitForSeconds(waitToStart);
		yield return StartCoroutine(Fade(0.0f, 1.0f, fadeDuration));
        if (fadeHoldDuration >= 0.0f) {
            yield return new WaitForSeconds(fadeHoldDuration);
            yield return StartCoroutine(Fade(1.0f, 0.0f, fadeDuration));
            Destroy(gameObject);
        }
	}

    private void SetToFade (float a) {
        foreach (GUIText guiText in GetComponentsInChildren<GUIText>()) {
            guiText.font.material.color = new Color(guiText.font.material.color.r,
                                                    guiText.font.material.color.g,
                                                    guiText.font.material.color.b, a);
        }
    }
	
	private IEnumerator Fade (float startLevel, float endLevel, float time)
	{
		float speed = 1.0f/time;
		
		for (float t = 0.0f; t < 1.0; t += Time.deltaTime*speed)
		{
			float a = Mathf.Lerp(startLevel, endLevel, t);
            SetToFade (a);
			yield return 0;
		}
	}
}
