using UnityEngine;
using UnityEngine.InputSystem;

public class ExcavatorController : MonoBehaviour
{
    [Header("*** Boom ***")]
    public Transform boom;        // Boomオブジェクト
    public Transform boomAxis;    // BoomAxisオブジェクト

    [Header("*** Arm ***")]
    public Transform arm;
    public Transform armAxis;

    // リセット用の初期位置と回転を保存する変数
    private Vector3 initBoomPos;
    private Quaternion initBoomRot;
    private Vector3 initArmPos;
    private Quaternion initArmRot;

    public float rotateSpeed = 60f;

    void Start()
    {
        // 初期位置と回転を保存（リセット用）
        initBoomPos = boom.position;
        initBoomRot = boom.rotation;
        initArmPos = arm.position;
        initArmRot = arm.rotation;
    }

    void Update()
    {
        // Eキー：右レバー押す → ブーム下げる
        if (Keyboard.current.eKey.isPressed)
        {
            boom.RotateAround(boomAxis.position, Vector3.forward, -rotateSpeed * Time.deltaTime);
        }

        // Dキー：右レバー引く → ブーム上げる
        if (Keyboard.current.dKey.isPressed)
        {
            boom.RotateAround(boomAxis.position, Vector3.forward, rotateSpeed * Time.deltaTime);
        }

        // Qキー：Arm Dump（アームを伸ばす）
        if (Keyboard.current.qKey.isPressed)
        {
            arm.RotateAround(armAxis.position, Vector3.forward, rotateSpeed * Time.deltaTime);
        }

        // Aキー：Arm Curl（アームを曲げる）
        if (Keyboard.current.aKey.isPressed)
        {
            arm.RotateAround(armAxis.position, Vector3.forward, -rotateSpeed * Time.deltaTime);
        }

        // Sキー：Reset
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            boom.SetPositionAndRotation(initBoomPos, initBoomRot);
            arm.SetPositionAndRotation(initArmPos, initArmRot);
        }
    }
}