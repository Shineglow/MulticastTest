using UnityEngine;

public class HealthBarAligner : MonoBehaviour
{
    [SerializeField] protected Transform target;
    [SerializeField] protected new Transform camera;
    [SerializeField] protected Vector3 offset;

    [SerializeField]
    private Renderer _renderer;

    private void Update()
    {
        if (!_renderer.isVisible) return;
        
        if (target != null)
        {
            transform.position = target.position + offset;
            transform.rotation = camera.rotation;
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void SetCameraTransform(Transform camera)
    {
        this.camera = camera.transform;
    }
    
    public void SetOffset(Vector3 offset)
    {
        this.offset = offset;
    }
}
