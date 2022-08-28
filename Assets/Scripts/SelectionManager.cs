using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public GameObject selectedObj;
    public GameObject selectUI;
    public GameObject colorUI;
    public Button deleteButton;
    public Button moveButton;
    public TextMeshProUGUI objNameTxt;
    private BuildingManager buildingManager;
    [SerializeField] private Material[] materials;
    private Camera Camera;
    public float zoomSpeed = 20f;

    // Start is called before the first frame update
    void Start()
    {
        buildingManager = GameObject.Find("BuildingManager").GetComponent<BuildingManager>();
        //Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        Camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000))
            {
                if (hit.collider.gameObject.CompareTag("Object"))
                {
                    Select(hit.collider.gameObject);
                }
                else
                {
                    Deselect();
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Deselect();
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (Camera.main.orthographicSize <= 30)
            {
                Camera.main.orthographicSize += 0.5f;
            }

        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (Camera.main.orthographicSize >= 1)
            {
                Camera.main.orthographicSize -= 0.5f;
            }
        }


    }

    private void Select(GameObject obj)
    {
        if (obj == selectedObj)
        {
            return;
        }
        if (selectedObj != null)
        {
            Deselect();
        }
        Outline outline = obj.GetComponent<Outline>();
        if (outline == null)
        {
            obj.AddComponent<Outline>();
        }
        else
        {
            outline.enabled = true;
        }
        objNameTxt.text = obj.name;
        selectedObj = obj;
        selectUI.SetActive(true);
    }

    void Deselect()
    {
        if (selectedObj != null)
        {
            selectedObj.GetComponent<Outline>().enabled = false;
            selectUI.SetActive(false);
            colorUI.SetActive(false);
            selectedObj = null;
        }
    }

    public void Delete()
    {
        GameObject objToDestroy = selectedObj;
        Deselect();
        Destroy(objToDestroy);
    }

    public void Move()
    {
        buildingManager.pendingObj = selectedObj;
        buildingManager.currentMaterial = selectedObj.GetComponentInChildren<MeshRenderer>().material;
    }

    public void EnableColorUI()
    {
        if (selectedObj != null)
        {
            selectUI.SetActive(false);
            colorUI.SetActive(true);
        }
    }

    public void DisableColorUI()
    {
        colorUI.SetActive(false);
        if (selectedObj != null)
        {
            selectUI.SetActive(true);
        }
    }

    public void ChangeColor(int index)
    {
        GameObject child = null;
        foreach (Transform transform in selectedObj.transform)
        {
            child = transform.gameObject;
            child.GetComponent<MeshRenderer>().material = materials[index];
        }
        selectedObj.GetComponent<Furniture>().materialID = index;
    }
}
