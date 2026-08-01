using UnityEngine;
using System.Collections.Generic;

public class CollisionScript : MonoBehaviour
{
    private MousePullTest pullInput;
    [SerializeField] private float MaxDamage = 30.0f;
    [SerializeField] private float thresholdTime = 1.0f;
    private Dictionary<GameObject, float> lastCollisionTimes = new Dictionary<GameObject, float>();

    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pullInput = transform.parent.gameObject.GetComponent<MousePullTest>();
    }

    // 衝突した瞬間に呼ばれる
    private void OnCollisionEnter(Collision collision)
    {
        // 衝突相手のオブジェクト名を表示
        Debug.Log("ぶつかりました: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Castle"))
        {
            GameObject collisionParent = collision.gameObject.transform.parent.gameObject;
            if (lastCollisionTimes.TryGetValue(collisionParent, out float lastTime))
            {
                if (Time.time - lastTime < thresholdTime) return; // 1秒以内は衝突判定を行わない
            }

            collisionParent.GetComponent<PrefabSwitcherScript>().Damage(MaxDamage*pullInput.Power);
            // 衝突相手のオブジェクト名を表示
            Debug.Log("ダメージ！: " + MaxDamage * pullInput.Power);
            Debug.Log("linearVelocity: " + rb.linearVelocity);

            rb.linearVelocity *= -0.8f; // 衝突後に反発
            lastCollisionTimes[collisionParent] = Time.time;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
