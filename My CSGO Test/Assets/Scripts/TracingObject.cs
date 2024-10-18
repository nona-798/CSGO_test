using UnityEngine;

public abstract class TracingObject : MonoBehaviour
{
    public abstract void FindObject(GameObject obj, string name);
    public abstract void SetPos(GameObject obj, string name);
    
}
