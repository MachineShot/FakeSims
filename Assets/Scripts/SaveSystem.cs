using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] Furniture chairPrefab;
    [SerializeField] Furniture tablePrefab;
    [SerializeField] Furniture sofaPrefab;

    [SerializeField] private Material[] materials;

    private SelectionManager selectionManager;

    public static List<Furniture> Objects = new List<Furniture>();
    const string OBJ_SUB = "/obj";
    const string OBJ_COUNT_SUB = "/obj.count";

    private void Start()
    {
        selectionManager = GameObject.Find("SelectionManager").GetComponent<SelectionManager>();
    }

    public void NewGame()
    {
        SceneManager.LoadScene(0);
    }

    public void SaveObj()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + OBJ_SUB;
        string countPath = Application.persistentDataPath + OBJ_COUNT_SUB;

        FileStream countStream = new FileStream(countPath, FileMode.Create);

        formatter.Serialize(countStream, Objects.Count);
        countStream.Close();

        for (int i = 0; i < Objects.Count; i++)
        {
            FileStream stream = new FileStream(path + i, FileMode.Create);
            FurnitureData data = new FurnitureData(Objects[i]);

            formatter.Serialize(stream, data);
            stream.Close();
        }
    }

    public void LoadObj()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + OBJ_SUB;
        string countPath = Application.persistentDataPath + OBJ_COUNT_SUB;
        int objCount = 0;

        if (File.Exists(countPath))
        {
            FileStream countStream = new FileStream(countPath, FileMode.Open);

            objCount = (int)formatter.Deserialize(countStream);
            countStream.Close();
        }
        else
        {
            Debug.LogError("Path not found in " + countPath);
        }

        for (int i = 0; i < objCount; i++)
        {
            if (File.Exists(path + i))
            {
                FileStream stream = new FileStream(path + i, FileMode.Open);
                FurnitureData data = formatter.Deserialize(stream) as FurnitureData;
                Furniture obj = null;

                stream.Close();

                Vector3 position = new Vector3(data.Position[0], data.Position[1], data.Position[2]);
                Vector3 rotation = new Vector3(data.Rotation[0], data.Rotation[1], data.Rotation[2]);

                switch (data.Name)
                {
                    case "Chair":
                        obj = Instantiate(chairPrefab, position, Quaternion.Euler(rotation));
                        obj.Name = data.Name;
                        break;
                    case "Table":
                        obj = Instantiate(tablePrefab, position, Quaternion.Euler(rotation));
                        obj.Name = data.Name;
                        break;
                    case "Sofa":
                        obj = Instantiate(sofaPrefab, position, Quaternion.Euler(rotation));
                        obj.Name = data.Name;
                        break;
                    default:
                        Debug.LogError("Object Name Not Found");
                        break;
                }

                obj.tag = "Object";
                selectionManager.selectedObj = obj.transform.gameObject;
                selectionManager.ChangeColor(data.materialID);
            }
            else
            {
                Debug.LogError("Path not found in " + (path + i));
            }
        }
        selectionManager.selectedObj = null;
    }
}
