using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class RadialMenuCreator : MonoBehaviour
{
    public OVRInput.Button spawnButton;

    [SerializeField] private Sprite[] radialSprites;

    public int numberOfRadialPart = 7;
    public int Gap;
    public GameObject radialPartPrefab;
    public Transform radialPartCanvas;
    public Transform handTransform;

    public UnityEvent<int> OnPartSelected;

    private List<GameObject> spawnedPart = new List<GameObject>();
    private int currentSelectedRadialPart = -1;

    void Update()
    {
        if (OVRInput.GetDown(spawnButton))
        {
            SpawnRadialParts();
        }

        if (radialPartCanvas.gameObject.activeSelf)
        {
            GetSelectedRadialPart();
        }

        if (OVRInput.GetUp(spawnButton))
        {
            HideAndTriggerSelected();
        }
    }

    public void HideAndTriggerSelected()
    {
        Debug.Log("Selected part: " + currentSelectedRadialPart);
        OnPartSelected.Invoke(currentSelectedRadialPart);
        radialPartCanvas.gameObject.SetActive(false);
        currentSelectedRadialPart = -1;
    }

    public void GetSelectedRadialPart()
    {
        Vector3 centerToHand = handTransform.position - radialPartCanvas.position;
        Vector3 centerToHandProjected = Vector3.ProjectOnPlane(centerToHand, radialPartCanvas.forward);

        float angle = Vector3.SignedAngle(-radialPartCanvas.up, centerToHandProjected, -radialPartCanvas.forward);
        if (angle < 0) angle += 360;

        currentSelectedRadialPart = (int)(angle * numberOfRadialPart / 360);

        for (int i = 0; i < spawnedPart.Count; i++)
        {
            Image img = spawnedPart[i].GetComponent<Image>();
            if (i == currentSelectedRadialPart)
            {
                img.color = Color.yellow;
                spawnedPart[i].transform.localScale = Vector3.one * 1.2f;
            }
            else
            {
                img.color = Color.white;
                spawnedPart[i].transform.localScale = Vector3.one;
            }
        }
    }

    public void SpawnRadialParts()
    {
        radialPartCanvas.gameObject.SetActive(true);
        radialPartCanvas.position = handTransform.position;
        radialPartCanvas.rotation = handTransform.rotation;

        foreach (GameObject part in spawnedPart)
        {
            Destroy(part);
        }
        spawnedPart.Clear();

        for (int i = 0; i < numberOfRadialPart; i++)
        {
            float angle = -i * (360f / numberOfRadialPart);
            Vector3 radialPartEulerAngle = new Vector3(0, 0, angle);
            GameObject spawnedRadialPart = Instantiate(radialPartPrefab, radialPartCanvas);
            spawnedRadialPart.transform.position = radialPartCanvas.position;
            spawnedRadialPart.transform.localEulerAngles = radialPartEulerAngle;

            Image img = spawnedRadialPart.GetComponent<Image>();
            if (i < radialSprites.Length)
                img.sprite = radialSprites[i];

            img.fillAmount = 1f / numberOfRadialPart;

            spawnedPart.Add(spawnedRadialPart);
        }
    }
}
