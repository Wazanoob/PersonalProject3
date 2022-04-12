using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    #region CreateText in world
    //Create Text in the world
    public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int frontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = 5000, float xRotation = 0)
    {
        if (color == null) color = Color.white;
        return CreateWorldText(parent, text, localPosition, frontSize, (Color)color, textAnchor, textAlignment, sortingOrder, xRotation);
    }

    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder, float xRotation)
    {
        GameObject gameObject = new GameObject("World Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        transform.Rotate(xRotation, 0, 0);
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }
    #endregion

    #region MousePosition 2D
    //Get Mouse Position 2D
    //Without parameters and with z = 0f
    public static Vector3 GetMousePosition2D()
    {
        Vector3 mouseWorldPosition = GetMousePosition2DWithZ(Input.mousePosition, Camera.main);
        mouseWorldPosition.z = 0f;
        return mouseWorldPosition;
    }
    //Without parameters and with z != 0f
    public static Vector3 GetMousePosition2DWithZ()
    {
        return GetMousePosition2DWithZ(Input.mousePosition, Camera.main);
    }
    //With specific Camera
    public static Vector3 GetMousePosition2DWithZ(Camera worldCamera)
    {
        return GetMousePosition2DWithZ(Input.mousePosition, worldCamera);
    }
    //Return Mouse Position
    public static Vector3 GetMousePosition2DWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 mouseWorldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return mouseWorldPosition;
    }
    #endregion

    #region MousePosition 3D
    //Get Mouse Position 3D
    public static Vector3 GetMousePosition3D(LayerMask layerMask)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask))
        {
            return raycastHit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }
    #endregion

    #region TouchPosition 3D
    public static Vector3 GetTouchPosition3D(Vector3 touchPos, LayerMask layerMask)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPos);


        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask))
        {
            return raycastHit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }
    #endregion
}
