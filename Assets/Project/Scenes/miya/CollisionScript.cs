using UnityEngine;

public class CollisionScript : MonoBehaviour
{
    public float currentDamage = 30.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // 衝突した瞬間に呼ばれる
    private void OnCollisionEnter(Collision collision)
    {
        // 衝突相手のオブジェクト名を表示
        Debug.Log("ぶつかりました: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Castle"))
        {
            collision.gameObject.transform.parent.gameObject.GetComponent<PrefabSwitcherScript>().Damage(currentDamage);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
