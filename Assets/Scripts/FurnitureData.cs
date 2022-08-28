using UnityEngine;

[System.Serializable]
public class FurnitureData
{
    public string Name;
    public float[] Position;
    public float[] Rotation;
    public int materialID;

    public FurnitureData(Furniture obj)
    {
        Name = obj.Name;
        materialID = obj.materialID;

        Vector3 objPos = obj.transform.position;
        Vector3 objRot = obj.transform.eulerAngles;

        Position = new float[]
        {
            objPos.x, objPos.y, objPos.z
        };

        Rotation = new float[]
        {
            objRot.x, objRot.y, objRot.z
        };
    }
}
