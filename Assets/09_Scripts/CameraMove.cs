using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform player;            // 기준이 되는 플레이어 위치
    public LayerMask cameraCollision;   // 충돌 감지할 레이어
    private Transform cameraBase;       // 부모 오브젝트 (camera_base)

    public Vector3 localOriginalPosition = new Vector3(0, 4, -5); // 기본 오프셋 (조정 필요)
    public float minDistanceThreshold = 0.1f; // 최소 거리 임계값
    public float lerpSpeed = 5f;         // 보간 속도

    void Start()
    {
        // 부모 오브젝트 참조
        cameraBase = transform.parent;
        if (cameraBase == null)
        {
            Debug.LogError("Camera must be a child of camera_base!");
        }
    }

    void LateUpdate()
    {
        if (cameraBase == null || player == null) return;

        // 카메라의 원래 자리 (camera_base의 로컬 위치)
        Vector3 worldOriginalPosition = cameraBase.TransformPoint(localOriginalPosition);

        // 카메라에서 플레이어로의 방향 및 거리
        Vector3 rayDir = transform.position - player.position;
        float distance = rayDir.magnitude;

        // 플레이어에서 카메라 방향으로 Ray 발사
        if (Physics.Raycast(player.position, rayDir.normalized, out RaycastHit hit, distance, cameraCollision))
        {
            // 충돌 지점에서 약간 후퇴
            transform.position = hit.point - rayDir.normalized * 0.1f;
        }
        else
        {
            // 충돌이 없으면 원래 자리로 부드럽게 이동
            float distanceToTarget = Vector3.Distance(transform.position, worldOriginalPosition);
            if (distanceToTarget > minDistanceThreshold)
            {
                transform.position = Vector3.Lerp(transform.position, worldOriginalPosition, Time.deltaTime * lerpSpeed);
            }
        }
    }
}