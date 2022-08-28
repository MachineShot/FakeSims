using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour
{
    public GameObject[] objects;
    public GameObject pendingObj;
    private Vector3 pos;
    private RaycastHit hit;
    public Material currentMaterial;
    public TextMeshProUGUI rotateTxt;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Toggle gridToggle;
    [SerializeField] private Material[] materials;
    bool gridOn = true;
    public bool canPlace = true;
    public float rotateAmount;
    public float gridSize;

    private void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000, layerMask))
        {
            if (hit.collider != null)
            {
                pos = hit.point;
                pos.y += 1;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(pendingObj != null)
        {
            if (gridOn)
            {
                pendingObj.transform.position = new Vector3(
                    RoundToNearestGrid(pos.x),
                    RoundToNearestGrid(pos.y),
                    RoundToNearestGrid(pos.z));
            }
            else
            {
                pendingObj.transform.position = pos;
            }

            if (Input.GetMouseButtonDown(0) && canPlace)
            {
                PlaceObject();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                RotateObject();
            }
            UpdateMaterials();
            rotateTxt.gameObject.SetActive(true);
        }
        else
        {
            rotateTxt.gameObject.SetActive(false);
        }
    }

    public void SelectObject(int index)
    {
        pendingObj = Instantiate(objects[index], pos, transform.rotation);
        pendingObj.name = objects[index].name;

        currentMaterial = pendingObj.GetComponentInChildren<MeshRenderer>().material;
    }

    public void PlaceObject()
    {
        foreach (Transform transform in pendingObj.transform)
        {
            GameObject child = transform.gameObject;
            child.GetComponent<MeshRenderer>().material = currentMaterial;
        }
        pendingObj.tag = "Object";
        pendingObj = null;
    }

    public void ToggleGrid()
    {
        if (gridToggle.isOn)
        {
            gridOn = true;
        }
        else
        {
            gridOn = false;
        }
    }

    float RoundToNearestGrid(float pos)
    {
        float xDiff = pos % gridSize;
        pos -= xDiff;
        if (xDiff > (gridSize / 2))
        {
            pos += gridSize;
        }
        return pos;
    }

    void RotateObject()
    {
        pendingObj.transform.Rotate(Vector3.up, rotateAmount);
    }

    void UpdateMaterials()
    {
        if (pendingObj != null)
        {
            foreach (Transform transform in pendingObj.transform)
            {
                GameObject child = transform.gameObject;
                if (canPlace)
                {
                    child.GetComponent<MeshRenderer>().material = materials[0];
                }
                else
                {
                    child.GetComponent<MeshRenderer>().material = materials[1];
                }
            }
        }
    }
}
