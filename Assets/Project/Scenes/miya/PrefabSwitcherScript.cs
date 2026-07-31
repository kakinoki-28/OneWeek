using UnityEngine;

public class PrefabSwitcherScript : MonoBehaviour
{
    [System.Serializable]
    public struct DestructionStage
    {
        public float thresholdRatio;
        public GameObject stageObject;
    }
    public float maxHealth = 100f;
    //private float currentHealth;
    public float currentHealth;


    public DestructionStage[] stages;
    private GameObject currentInstance;
    private int currentStageIndex = 0;

    void Start()
    {
        currentHealth = maxHealth;
        currentInstance = gameObject;
        stagesInit();
        UpdateObject(1.0f);
    }

    private void stagesInit()
    {
        string base_name = gameObject.name;
        stages = new DestructionStage[3];
        for (int i=0; i<3; i++)
        {
            stages[i] = new DestructionStage { 
                thresholdRatio = 1.0f-i/3.0f, 
                stageObject = transform.Find($"{base_name}_break{i}").gameObject
            };            
        }
        
    }

    [ContextMenu("DebugDamage")]
    private void DebugDamage()
    {
        Damage(10f);
    }
    public void Damage(float damage)
    {
        if(currentHealth <= 0f) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0f);

        UpdateObject( currentHealth/maxHealth );

        if(currentHealth <= 0f) OnDestroyed();
    }

    private void UpdateObject(float healthRatio)
    {
        int targetStageIndex = 0;
        for (int i = 0; i < stages.Length; i++)
        {
            if (healthRatio <= stages[i].thresholdRatio)
            {
                targetStageIndex = i;
            }
        }

        // ステージが変化した場合のみプレハブを置き換える
        if (targetStageIndex != currentStageIndex)
        {
            currentStageIndex = targetStageIndex;
            SwitchObject();
        }
    }

    private void SwitchObject()
    {
        for (int i = 0; i < stages.Length; i++)
        {
            if (stages[i].stageObject != null) stages[i].stageObject.SetActive(i == currentStageIndex);
        }
    }   

    private void OnDestroyed()
    {
        Debug.Log(gameObject.name + " が完全に破壊されました。");
        if (currentInstance != null)  currentInstance.SetActive(false);
    }

}
