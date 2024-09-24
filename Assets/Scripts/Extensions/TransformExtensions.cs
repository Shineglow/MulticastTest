using UnityEngine;

namespace Network.Extensions
{
    public static class TransformExtensions
    {
        public static void GetPositionAndRotation(this Transform t, out Vector3 position, out Quaternion rotation)
        {
            position = t.position;
            rotation = t.rotation;
        }
    }
}