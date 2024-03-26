using UnityEngine;

[RequireComponent(typeof(SkinnedMeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class UpdateMeshColliderFromAnimation : MonoBehaviour
{
    private SkinnedMeshRenderer sr;
    private MeshCollider mc;
    [SerializeField]
    public int updateIntervalsFrameCount = 5;

    private Mesh curMesh;
    public void Start()
    {
        sr = transform.GetComponent<SkinnedMeshRenderer>();
        mc = transform.GetComponent<MeshCollider>();
        curMesh = new Mesh();
    }

    private int updateCount = 0;
    public void Update()
    {
        updateCount++;
        if (updateCount > updateIntervalsFrameCount)
        {
            sr.BakeMesh(curMesh);
            mc.sharedMesh = curMesh;
            updateCount = 0;
        }
    }
}
