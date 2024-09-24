using UnityEngine;

namespace Network.Systems.Input
{
    public interface IPlayerInput
    {
        float Horizontal { get; }
        float Vertical { get; }
        Vector3 CombinedAxes { get; }
    }
}