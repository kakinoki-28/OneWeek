using UnityEngine;
using System.Collections.Generic;

public class CollisionScript : MonoBehaviour
{
    [SerializeField] private MousePullTest pullInput;
    [SerializeField] private BatteringRamResetController resetController;
    [SerializeField] private CameraShakeScript shakeScript;
    [SerializeField] private AttackResultController attackResultController;
    [SerializeField] private float MaxDamage = 30.0f;
    [SerializeField] private float thresholdTime = 2.0f;
    [SerializeField] private float waitCollideThreshold = 5.0f;
    private Dictionary<GameObject, float> lastCollisionTimes = new Dictionary<GameObject, float>();

    private Rigidbody rb;
    private bool Collided = false;
    private float afterReleaseTime = 0f;

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
        if (shakeScript == null)
        {
            Debug.LogError("CameraShakeScriptが見つかりません");
            enabled = false;
            return;
        }
        if (attackResultController == null)
        {
            Debug.LogError("AttackResultControllerが見つかりません");
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
            Debug.Log("ヒットストップ: " + (0.05f+0.2f*pullInput.Power) + "秒" + "、カメラ揺れ量: " + (0.05f+0.3f*pullInput.Power) );
            StartCoroutine(shakeScript.HitStopAndShake(0.05f+0.2f*pullInput.Power, 0.05f+0.3f*pullInput.Power));

            lastCollisionTimes[collisionParent] = Time.time;
        }
    }

    void Update()
    {
        // 武器が発射された後、一定時間衝突がなければリセットする
        if (pullInput.HasReleasedWeapon)
        {
            if (afterReleaseTime == 0f)
            {
              afterReleaseTime = Time.time;
              Debug.Log($"afterReleaseTime set to {afterReleaseTime}");  
            } 
            if(!Collided && Time.time - afterReleaseTime < waitCollideThreshold) return;
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
                // 今回の攻撃による被害額を保存
                attackResultController.RecordAttackResult();
                
                resetController.ResetWeapon();
            }
        }else
        {
            Collided = false;
            afterReleaseTime = 0f;
        }
    }
}
