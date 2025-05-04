using UnityEngine;

[HutongGames.PlayMaker.ActionCategory("Camera")]
[HutongGames.PlayMaker.Tooltip("Rotates the camera around the player with mouse drag and prevents camera from clipping through objects.")]
public class RotateCameraWithCollision : HutongGames.PlayMaker.FsmStateAction
{
    [HutongGames.PlayMaker.RequiredField]
    [HutongGames.PlayMaker.Tooltip("The camera GameObject to move.")]
    public HutongGames.PlayMaker.FsmOwnerDefault cameraObject;

    [HutongGames.PlayMaker.RequiredField]
    [HutongGames.PlayMaker.Tooltip("The player GameObject to rotate around.")]
    public HutongGames.PlayMaker.FsmGameObject player;

    [HutongGames.PlayMaker.Tooltip("Rotation speed.")]
    public HutongGames.PlayMaker.FsmFloat rotateSpeed;

    [HutongGames.PlayMaker.Tooltip("LayerMask for camera collision detection.")]
    public LayerMask cameraCollision;

    public override void Reset()
    {
        cameraObject = null;
        player = null;
        rotateSpeed = 5f;
        cameraCollision = -1; // Everything
    }

    public override void OnUpdate()
    {
        // Use the instance Fsm property instead of the static class
        GameObject cam = Fsm.GetOwnerDefaultTarget(cameraObject);
        if (cam == null || player.Value == null) return;

        // 마우스 좌클릭 드래그로 회전
        if (Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            Transform camTransform = cam.transform;
            Vector3 playerPos = player.Value.transform.position;

            camTransform.RotateAround(playerPos, camTransform.up, mouseX * rotateSpeed.Value);
            camTransform.RotateAround(playerPos, camTransform.right, -mouseY * rotateSpeed.Value);

            // Z축 회전 방지
            Vector3 angles = camTransform.eulerAngles;
            camTransform.eulerAngles = new Vector3(angles.x, angles.y, 0);
        }

        // 카메라 충돌 방지
        Vector3 rayDir = cam.transform.position - player.Value.transform.position;
        if (Physics.Raycast(player.Value.transform.position, rayDir, out RaycastHit hit, float.MaxValue, cameraCollision))
        {
            cam.transform.position = hit.point - rayDir.normalized;
        }
    }
}