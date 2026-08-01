using System.Collections;
using UnityEngine;

public class CameraShakeScript : MonoBehaviour
{
    // ヒットストップとカメラシェイクを同時に行うコルーチン
    public IEnumerator HitStopAndShake(float duration, float magnitude)
    {
        Camera mainCam = Camera.main;
        Vector3 originalCamPos = mainCam != null ? mainCam.transform.localPosition : Vector3.zero;

        // ヒットストップ
        Time.timeScale = 0.05f;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Time.timeScaleの影響を受けない unscaledDeltaTime を使用
            elapsed += Time.unscaledDeltaTime;

            // カメラをランダム振動
            if (mainCam != null)
            {
                float x = originalCamPos.x + Random.Range(-1f, 1f) * magnitude;
                float y = originalCamPos.y + Random.Range(-1f, 1f) * magnitude;
                mainCam.transform.localPosition = new Vector3(x, y, originalCamPos.z);
            }

            // 次のフレームまで待機
            yield return null;
        }

        // 時間の流れとカメラの位置を元に戻す
        Time.timeScale = 1.0f;
        if (mainCam != null)
        {
            mainCam.transform.localPosition = originalCamPos;
        }
    }
}