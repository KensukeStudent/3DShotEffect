using UnityEngine;

/// <summary>
/// 3D版運動方程式
/// </summary>
public class Shot : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [Header("y軸の高さに比例する"), SerializeField]
    private float animationLength;

    private float timer = 0;

    [SerializeField]
    private Transform start;

    [SerializeField]
    private Transform end;

    private Vector3 defaultPos;

    private float vy;
    private float vx;

    [SerializeField, Range(-360, 360), Header("弧の描き方")]
    private float angle = 0;

    private void Start()
    {
        defaultPos = transform.position;
        float t = animationLength;
        float gravity = Physics.gravity.y;

        // y軸
        var startPos = start.position;
        var endPos = end.position;
        var diffY = (endPos - startPos).y;
        vy = (diffY - gravity * 0.5f * t * t) / t;

        // forward軸
        // y軸を除いた長さ
        Vector2 p1 = new Vector2(start.position.x, start.position.z);
        Vector2 p2 = new Vector2(end.position.x, end.position.z);
        float magnitude = p2.magnitude - p1.magnitude;
        vx = magnitude / t;
    }

    private void Update()
    {
        float gravity = Physics.gravity.y;
        timer += Time.deltaTime * speed;

        if (timer >= animationLength)
        {
            timer = 0;
            return;
        }

        var localEulerAngles = transform.localEulerAngles;
        localEulerAngles.z = angle;
        transform.localEulerAngles = localEulerAngles;

        Vector3 forward = vx * timer * transform.forward;
        Vector3 up = (vy * timer + 0.5f * gravity * timer * timer) * transform.up;

        transform.position = forward;
        transform.position += up;
        transform.position += defaultPos;

        Debug.Log($"forward: {transform.forward}");
        Debug.Log($"right: {transform.right}");
        Debug.Log($"up: {transform.up}");
    }
}
