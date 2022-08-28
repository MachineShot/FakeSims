using UnityEngine;

public class Furniture : MonoBehaviour
{
    public FurnitureData Data;
    public string Name;
    public int materialID;

    private void Awake()
    {
        SaveSystem.Objects.Add(this);
    }

    private void OnDestroy()
    {
        SaveSystem.Objects.Remove(this);
    }
}
