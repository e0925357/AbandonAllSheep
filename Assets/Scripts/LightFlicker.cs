using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float random, random2;
    private Vector3 initialScale;

	// Use this for initialization
	void Start ()
	{
	    spriteRenderer = GetComponent<SpriteRenderer>();
        random = Random.Range(0.0f, 65535.0f);
        initialScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update ()
	{
        Color color = spriteRenderer.color;
        float noise = Mathf.PerlinNoise(random, Time.time);
        float noise2 = Mathf.PerlinNoise(random2, Time.time);
        color.a = Mathf.Lerp(0.1f, 0.3f, noise); 

        float scale = Mathf.Lerp(0.9f, 1.25f, noise2);
        transform.localScale = new Vector3(initialScale.x * scale, initialScale.y * scale, 1.0f);
        spriteRenderer.color = color;
	}
}
