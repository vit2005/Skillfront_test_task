using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
   
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

            // update min and max
            foreach (var vector3 in v)
            {
                min = Vector3.Min(min, vector3);
                max = Vector3.Max(max, vector3);
            }
        }

        // create the bounds
        var bounds = new Bounds();
        bounds.SetMinMax(min, max);
        Debug.Log(bounds);
        Vector2 XY = bounds.center;

        float Z = mainCamera.transform.position.z;
        float w = bounds.extents.x / (Screen.width / 2);
        float h = bounds.extents.y / (Screen.height / 2);
        Debug.Log($"[{w}   {h}]");
        float maxK = Mathf.Max(w, h);
        if (maxK > 0.5f)
            Z -= (maxK - 0.5f)*100f;
        mainCamera.transform.position = new Vector3(XY.x, XY.y, Z);
    }
}
