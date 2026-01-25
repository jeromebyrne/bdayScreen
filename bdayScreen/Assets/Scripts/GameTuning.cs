using UnityEngine;

[CreateAssetMenu(menuName = "BdayScreen/Game Tuning", fileName = "GameTuning")]
public class GameTuning : ScriptableObject
{
    [Header("Runner")]
    [Min(0f)] public float runnerSpeed = 2.6f;
    public float runnerMinX = -9f;
    public float runnerMaxX = 9f;

    [Header("Camera")]
    public Vector3 cameraOffset = new Vector3(0f, 0f, -10f);
    [Min(0f)] public float cameraSmoothTime = 0f;

}
