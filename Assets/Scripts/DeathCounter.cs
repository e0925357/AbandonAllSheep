using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DeathCounter : MonoBehaviour {

	public Text deathsText;
	public Color flashColor;
	public float flashTime = 1.0f;
	public float flashScale = 1.5f;

	private Color defaultColor;

	int deathCount = 0;

	void OnEnable()
	{
		Health.onDeath += sheepDeath;
	}

	void OnDisable()
	{
		Health.onDeath -= sheepDeath;
	}

	void sheepDeath(GameObject sheep)
	{
		deathCount++;

		deathsText.text = "" + deathCount;

		StartCoroutine(flash());
	}

	IEnumerator flash()
	{
		Vector3 maxScale = new Vector3(flashScale, flashScale, 1);
		Vector3 minScale = new Vector3(1, 1, 1);

		defaultColor = deathsText.color;
		deathsText.color = flashColor;
		deathsText.transform.localScale = maxScale;

		for (float timer = 0; timer < flashTime; timer += Time.deltaTime)
		{
			yield return new WaitForEndOfFrame();

			float t = timer / flashTime;

			deathsText.color = Color.Lerp(flashColor, defaultColor, t);
			deathsText.transform.localScale = Vector3.Lerp(maxScale, minScale, t);
		}

		deathsText.color = defaultColor;
		deathsText.transform.localScale = minScale;
	}
}
