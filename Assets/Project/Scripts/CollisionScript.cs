using UnityEngine;
using System.Collections.Generic;

public class CollisionScript : MonoBehaviour
{
    [SerializeField] private MousePullTest pullInput;
    [SerializeField] private BatteringRamResetController resetController;
    [SerializeField] private float MaxDamage = 30.0f;
    [SerializeField] private float thresholdTime = 2.0f;
    private Dictionary<GameObject, float> lastCollisionTimes = new Dictionary<GameObject, float>();

    private Rigidbody rb;
    private bool Collided = false;

    void Awake()
    {
        if (pullInput == null)
        {
            Debug.LogError("MousePullTestが見つかりません");
            enabled = false;
            return;
        }

        if (resetController == null)
        {
            Debug.LogError("BatteringRamResetControllerが見つかりません");
            enabled = false;
            return;
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // 衝突した瞬間に呼ばれる
    private void OnCollisionEnter(Collision collision)
    {
        // 衝突相手のオブジェクト名を表示
        Debug.Log("ぶつかりました: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Castle"))
        {
            Collided = true;
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
            Debug.Log("After linearVelocity: " + rb.linearVelocity);

            lastCollisionTimes[collisionParent] = Time.time;
        }
    }

    void Update()
    {
        if (pullInput.HasReleasedWeapon && Collided)
        {
            foreach (var collideObject in lastCollisionTimes.Keys)
            {
                if(Time.time - lastCollisionTimes[collideObject] > thresholdTime)
                {
                    lastCollisionTimes.Remove(collideObject);
                    break; // Dictionaryのサイズが変わったのでループを抜ける
                }
            }
            if (lastCollisionTimes.Count == 0)
            {
                resetController.ResetWeapon();
            }
        }else
        {
            Collided = false;
        }
    }
}
