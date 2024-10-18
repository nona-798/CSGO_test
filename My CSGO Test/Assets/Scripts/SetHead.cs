using UnityEngine;
public class SetHead : TracingObject
{
    private GameObject Head;
    new readonly string name = "Head";

    private void Awake()
    {
        FindObject(Head, name);
    }
    // Update is called once per frame
    void Update()
    {
        SetPos(Head,"Head");
    }
    public override void FindObject(GameObject obj, string name)
    {
        obj = GameObject.Find(name).gameObject;
        Head = obj;
    }
    public override void SetPos(GameObject obj, string name)
    {
        if (obj.name == name)
        {
            this.transform.position = obj.transform.position;
        }
        else return;
    }
}
