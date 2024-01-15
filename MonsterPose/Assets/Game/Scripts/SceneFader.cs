using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{

	public Image img;
	public AnimationCurve curve;

    private void OnEnable()
    {
		StartCoroutine(FadeOut());
	}
	IEnumerator FadeOut()
	{
		float t = 0f;

		while (t < 1f)
		{
			t += Time.deltaTime;
			float a = curve.Evaluate(t);
			img.color = new Color(0f, 0f, 0f, a);
			yield return 0;
		}
        if (t >= 1)
        {
			t = 1f;

			while (t > 0f)
			{
				t -= Time.deltaTime;
				float a = curve.Evaluate(t);
				img.color = new Color(0f, 0f, 0f, a);
				yield return 0;
			}
		}
	}

}
