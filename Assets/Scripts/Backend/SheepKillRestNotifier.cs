using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Notifier for the BloodySheep Backend to report a killed sheep.
/// Uses the BloodySheep Rest-API.
/// </summary>
[RequireComponent(typeof(RestFactory))]
public class SheepKillRestNotifier : MonoBehaviour
{
	private RestFactory restFactory;

	[Tooltip("The url to be called by the post request.")]
	public string url = "https://api.bloodysheep.lukasf.at/sheep/kill";

	// Use this for initialization
	void Start()
	{
		restFactory = GetComponent<RestFactory>();
	}

	void OnEnable()
	{
		Health.onDeath += SheepDeath;
	}

	void OnDisable()
	{
		Health.onDeath -= SheepDeath;
	}

	/// <summary>
	/// Callback method which is registered at the Health script. Only use for this purpose.
	/// To notify the Backend about a kill use SheepKilled() instead.
	/// </summary>
	/// <param name="sheep">The killed sheep gameobject.</param>
	void SheepDeath(GameObject sheep)
	{
		SheepKilled();
	}

	/// <summary>
	/// Called whenever a sheep died.
	/// </summary>
	public void SheepKilled()
	{
		WWWForm wwwForm = new WWWForm();

		// Add some dummy data, because rest POST request with a zero sized post buffer is not supported
		wwwForm.AddField("Sheep", "Maeh");
		byte[] rawData = wwwForm.data;
		Dictionary<string, string> headers = wwwForm.headers;

		// Set the current user in the Authentication to uniquely identify users
		// TODO: Currently it uses the device id --> use real User Names later instead
		headers.Add("Authorization", SystemInfo.deviceUniqueIdentifier);

		restFactory.RestPost(url, rawData, headers, KillRestCompleted);
	}

	/// <summary>
	/// Called when the rest request for the sheep kill has completed.
	/// </summary>
	/// <param name="www">The WWW object which was used for the request.</param>
	private void KillRestCompleted(WWW www)
	{
		// Do nothing for the moment
	}
}
