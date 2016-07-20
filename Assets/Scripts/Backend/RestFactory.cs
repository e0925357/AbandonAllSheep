using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Simple RestFactory to make GET and POST Requests.
/// </summary>
public class RestFactory : MonoBehaviour
{
	private string results;

	/// <summary>
	/// The result of the last completed request. null, if there was not request yet or the last requets failed.
	/// </summary>
	public string Results
	{
		get { return results; }
	}

	/// <summary>
	/// Makes a Post Request which is executed in a coroutine. When the request has completed, onComplete ist called.
	/// </summary>
	/// <param name="url">The url of the POST request. The url must be '%' escaped.</param>
	/// <param name="wwwForm">The wwwForm to use the parameters from. Must not be null.</param>
	/// <param name="onComplete">Called when the request hast completed (even if it has failed --> check the www.error field)</param>
	/// <returns>The WWW object used for the request.</returns>
	public WWW RestPost(string url, WWWForm wwwForm, Action<WWW> onComplete)
	{
		if (wwwForm == null)
		{
			throw new ArgumentNullException("wwwForm");
		}

		WWW www = new WWW(url, wwwForm);
		StartCoroutine(WaitForRequest(www, onComplete));
		return www;
	}

	/// <summary>
	/// Makes a Post Request which is executed in a coroutine. When the request has completed, onComplete ist called.
	/// This method allows custom data and headers to be used by the request.
	/// </summary>
	/// <param name="url">The url of the POST request. The url must be '%' escaped.</param>
	/// <param name="postData">The data to be posted.</param>
	/// <param name="headers">The headers to be used.</param>
	/// <param name="onComplete">Called when the request hast completed (even if it has failed --> check the www.error field)</param>
	/// <returns>The WWW object used for the request.</returns>
	public WWW RestPost(string url, byte[] postData, Dictionary<string, string> headers, Action<WWW> onComplete)
	{
		WWW www = new WWW(url, postData, headers);
		StartCoroutine(WaitForRequest(www, onComplete));
		return www;
	}

	/// <summary>
	/// Creates a Get request which is executed in a coroutine. When the request has completed, onComplete ist called.
	/// </summary>
	/// <param name="url">The url of the GET request. The url must be '%' escaped.</param>
	/// <param name="onComplete">Called when the request hast completed (even if it has failed --> check the www.error field)</param>
	/// <returns>The WWW object used for the request.</returns>
	public WWW RestGet(string url, Action<WWW> onComplete)
	{
		WWW www = new WWW(url);
		StartCoroutine(WaitForRequest(www, onComplete));
		return www;
	}

	/// <summary>
	/// Method which waits for the result of the www object. Executed as Coroutine.
	/// </summary>
	/// <param name="www">The WWW object to wait for.</param>
	/// <param name="onComplete">Called when the request hast completed (even if it has failed --> check the www.error field)</param>
	/// <returns>The IEnumerator for the Coroutine mechanism.</returns>
	private IEnumerator WaitForRequest(WWW www, Action<WWW> onComplete)
	{
		yield return www;

		if (www.error == null)
		{
			results = www.text;
		}
		if (www.error != null)
		{
			results = null;
			Debug.Log(string.Format("Rest Request failed: {0}", www.error));
		}
		onComplete(www);
	}
}
