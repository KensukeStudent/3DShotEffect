using UnityEngine;

public class HoundProjectile : MonoBehaviour
{
    public Transform target; // 目標
    public float speed = 5f; // 弾の移動速度
    public float stoppingDistance = 1f; // 停止距離
    public float controlPointOffset = 2f; // 制御点のオフセット
    public float maxDistance = 10f; // 追尾切り替えの距離

    private float t = 0f; // ベジェ曲線の進行度

    private Vector3 startPoint;

    private void Start()
    {
        startPoint = transform.position;
    }

    private void Update()
    {
        if (target != null)
        {
            // 目標との距離を計算
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            // 追尾距離に到達しているかチェック
            if (distanceToTarget < maxDistance)
            {
                // ターゲットに向かって移動
                Vector3 direction = (target.position - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;

                // 停止距離よりも遠ければ、tをリセット
                if (distanceToTarget > stoppingDistance)
                {
                    t = 0f; // ベジェ曲線の進行度をリセット
                }
            }
            else
            {
                // ベジェ曲線の制御点を計算
                Vector3 startPoint = transform.position;
                Vector3 endPoint = target.position;
                Vector3 controlPoint = startPoint + (target.position - startPoint).normalized * controlPointOffset;

                // ベジェ曲線のポイントを計算
                Vector3 bezierPoint = CalculateBezierPoint(t, startPoint, controlPoint, endPoint);
                transform.position = bezierPoint;

                // tを進める
                t += Time.deltaTime * speed;

                // tが1を超えたら停止
                if (t > 1f)
                {
                    t = 1f;
                }
            }
        }
    }

    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        // ベジェ曲線のポイントを計算
        return (1 - t) * (1 - t) * p0 + 2 * (1 - t) * t * p1 + t * t * p2;
    }
}
