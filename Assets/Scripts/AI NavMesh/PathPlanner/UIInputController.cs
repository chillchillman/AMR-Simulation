using UnityEngine;
using TMPro; // 引入 TextMeshPro 命名空間

public class UIInputController : MonoBehaviour
{
    [SerializeField] private TMP_InputField speedInputField;         // TextMeshPro 的速度輸入框
    [SerializeField] private TMP_InputField angularSpeedInputField; // TextMeshPro 的旋轉速度輸入框
    [SerializeField] private TMP_InputField accelerationInputField; // TextMeshPro 的加速度輸入框
    [SerializeField] private CarNavigationController carController; // 車輛導航控制器

    private void Start()
    {
        // 綁定事件
        speedInputField.onEndEdit.AddListener(OnSpeedChanged);
        angularSpeedInputField.onEndEdit.AddListener(OnAngularSpeedChanged);
        accelerationInputField.onEndEdit.AddListener(OnAccelerationChanged);
    }

    private void OnSpeedChanged(string input)
    {
        if (float.TryParse(input, out float speed))
        {
            carController.SetSpeed(speed);
        }
        else
        {
            Debug.LogWarning("Invalid speed input");
        }
    }

    private void OnAngularSpeedChanged(string input)
    {
        if (float.TryParse(input, out float angularSpeed))
        {
            carController.SetAngularSpeed(angularSpeed);
        }
        else
        {
            Debug.LogWarning("Invalid angular speed input");
        }
    }

    private void OnAccelerationChanged(string input)
    {
        if (float.TryParse(input, out float acceleration))
        {
            carController.SetAcceleration(acceleration);
        }
        else
        {
            Debug.LogWarning("Invalid acceleration input");
        }
    }
}
