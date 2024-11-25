using UnityEngine;

public class UserInputManager : MonoBehaviour
{
    [SerializeField] private float maxLoad = 50f; // 最大负载
    [SerializeField] private float defaultSpeed = 3.5f; // 默认速度
    [SerializeField] private float defaultRotationSpeed = 120f; // 默认旋转速度

    private float userSpeed;
    private float userRotationSpeed;
    private float userLoadWeight;

    public void SetSpeed(float speed) => userSpeed = Mathf.Clamp(speed, 0, 10);
    public void SetRotationSpeed(float rotationSpeed) => userRotationSpeed = Mathf.Clamp(rotationSpeed, 0, 720);
    public void SetLoadWeight(float loadWeight) => userLoadWeight = Mathf.Clamp(loadWeight, 0, maxLoad);

    public float GetSpeed() => userSpeed > 0 ? userSpeed : defaultSpeed;
    public float GetRotationSpeed() => userRotationSpeed > 0 ? userRotationSpeed : defaultRotationSpeed;
    public float GetLoadWeight() => userLoadWeight;
    public float GetMaxLoad() => maxLoad;
}
