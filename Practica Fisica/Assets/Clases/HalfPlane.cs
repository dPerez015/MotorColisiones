using UnityEngine;
using System.Collections;

public class HalfPlane : MonoBehaviour
{

    //Half planes are like walls, they don't move and have infinite mass
    [SerializeField]
    private float offset;
    [SerializeField]
    private Vec3 normal;

    public void Start()
    {

    }
    public float GetOffset() { return offset; }
    public Vec3 GetNormal() { return normal; }
    public float GetInverseMass() { return 0; }
    public void SetOffset(float _offset) { offset = _offset; }
    public void SetNormal(Vec3 _normal) { normal = _normal; }
}