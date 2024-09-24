using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(Camera), typeof(PositionConstraint))]
public class CameraController : MonoBehaviour
{
    [SerializeField] 
    private new Camera camera;
    private PositionConstraint _positionConstraint;

    private void Awake()
    {
        camera ??= GetComponent<Camera>();
        _positionConstraint = GetComponent<PositionConstraint>();
    }

    public void FollowCharacter(Transform playerTransform)
    {
        StopFollowing();
        _positionConstraint.AddSource(new ConstraintSource()
        {
            sourceTransform = playerTransform,
            weight = 1f
        });
        _positionConstraint.constraintActive = true;
    }
        
    public void StopFollowing()
    {
        if (_positionConstraint.sourceCount > 0)
        {
            _positionConstraint.RemoveSource(0);
        }
        _positionConstraint.constraintActive = false;
    }
}