using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.Android;

public class ConnectionUIManager : MonoBehaviour
{
    public TextMeshProUGUI connectingText;
    public TextMeshProUGUI wifiStatusText;
    public TextMeshProUGUI tableStatusText;

    private readonly string[] dots = { ".", "..", "..." };

    void Start()
    {
        StartCoroutine(FullConnectionSequence());
    }

    IEnumerator FullConnectionSequence()
    {
        // 1. Animate "Connecting"
        yield return StartCoroutine(AnimateWithText(connectingText, "Connecting", 3f));

        // 2. Animate "Wi-Fi" until connected
        bool wifiConnected = false;
        do
        {
            yield return StartCoroutine(AnimateWithText(wifiStatusText, "Wi-Fi", 2f));
            yield return StartCoroutine(CheckWifiAndInternet((connected) =>
            {
                wifiConnected = connected;
                Debug.Log("Wi-Fi connected");
            }));

            wifiStatusText.text = wifiConnected ? "Wi-Fi: Connected" : "Wi-Fi: Disconnected";
            wifiStatusText.color = wifiConnected ? Color.green : Color.red;

            if (!wifiConnected)
                yield return new WaitForSeconds(1f);
        } while (!wifiConnected);

        yield return new WaitForSeconds(1f);

        // 3. Animate "Table" (simulated)
        yield return StartCoroutine(AnimateWithText(tableStatusText, "Table", 5f));
        tableStatusText.text = "Table: Connected";
        tableStatusText.color = Color.green;

        // 4. Fade out all text
        yield return StartCoroutine(FadeOutTextGroup(1f));
    }

    IEnumerator AnimateWithText(TextMeshProUGUI textObj, string baseText, float durationSeconds)
    {
        float timer = 0f;
        int dotIndex = 0;
        while (timer < durationSeconds)
        {
            textObj.text = baseText + dots[dotIndex];
            dotIndex = (dotIndex + 1) % dots.Length;
            yield return new WaitForSeconds(0.5f);
            timer += 0.5f;
        }
    }

    IEnumerator CheckWifiAndInternet(System.Action<bool> callback)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            while (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                yield return null;
            }
        }
#endif

        yield return new WaitForSeconds(1f);

        if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            using (UnityWebRequest request = UnityWebRequest.Head("https://www.google.com"))
            {
                request.timeout = 5;
                yield return request.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
                callback(request.result == UnityWebRequest.Result.Success);
#else
                callback(!(request.isNetworkError || request.isHttpError));
#endif
                yield break;
            }
        }

        callback(false);
    }

    IEnumerator FadeOutTextGroup(float duration)
    {
        float elapsed = 0f;

        Color connectColor = connectingText.color;
        Color wifiColor = wifiStatusText.color;
        Color tableColor = tableStatusText.color;

        while (elapsed < duration)
        {
            float t = 3f - (elapsed / duration);

            Color fadeConnect = connectColor;
            fadeConnect.a = t;
            connectingText.color = fadeConnect;

            Color fadeWifi = wifiColor;
            fadeWifi.a = t;
            wifiStatusText.color = fadeWifi;

            Color fadeTable = tableColor;
            fadeTable.a = t;
            tableStatusText.color = fadeTable;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure fully invisible at the end
        Color invisible = new Color(1, 1, 1, 0);
        connectingText.color = invisible;
        wifiStatusText.color = invisible;
        tableStatusText.color = invisible;
    }
}
