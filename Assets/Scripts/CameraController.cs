using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    private bool _isLogging = false;

    void Start()
    {
        UnitSpawner.OnDominoesTilesPlaced += UnitSpawner_OnDominoesTilesPlaced;
    }

    private void UnitSpawner_OnDominoesTilesPlaced(List<Image> imageList)
    {
        var min = Vector3.positiveInfinity;
        var max = Vector3.negativeInfinity;

        foreach (var image in imageList)
        {
            if (!image) continue;

            // Get the 4 corners in world coordinates
            var v = new Vector3[4];
            image.rectTransform.GetWorldCorners(v);

            // Update min and max
            foreach (var vector3 in v)
            {
                min = Vector3.Min(min, vector3);
                max = Vector3.Max(max, vector3);
            }
        }

        // Create the bounds
        var bounds = new Bounds();
        bounds.SetMinMax(min, max);
        if (_isLogging) Debug.Log(bounds);

        // Get position of min and max on screen
        Vector3 screenPositionMin = mainCamera.WorldToScreenPoint(min);
        Vector3 screenPositionMax = mainCamera.WorldToScreenPoint(max);

        if (_isLogging) Debug.Log($"ScreenPos: [{screenPositionMin}   {screenPositionMax}]");

        Vector2 XY = bounds.center;
        if (_isLogging) Debug.Log($"Screen: [{Screen.width}   {Screen.height}]");
        float Z = mainCamera.transform.position.z;

        // If images bound width and heigth larger than half of screen, camera will move backward a little bit
        float w = (screenPositionMax.x - screenPositionMin.x) / (Screen.width / 2);
        float h = (screenPositionMax.y - screenPositionMin.y) / (Screen.height / 2);
        if (_isLogging) Debug.Log($"w,h: [{w}   {h}]");
        float maxK = Mathf.Max(w, h);
        if (maxK > 1f)
            Z -= (maxK - 1f) * 500f;

        if (_isLogging) Debug.Log($"maxK,z: [{maxK}     {Z}]");
        mainCamera.transform.position = new Vector3(XY.x, XY.y, Z);
    }
}
