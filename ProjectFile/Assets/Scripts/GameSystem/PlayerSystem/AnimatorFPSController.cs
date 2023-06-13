using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorFPSController : MonoBehaviour
{
    [SerializeField, Range(1, 30)]
    int _fps = 30;

    Animator _animator;

    /// <summary>しきい値時間</summary>
    float _thresholdTime;

    /// <summary>スキップされた更新時間</summary>
    float _skippedTime;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.enabled = false;
        InitializeThresholdTime();
    }

    /// <summary>
    /// しきい値時間の初期化
    /// </summary>
    void InitializeThresholdTime()
    {
        _thresholdTime = 1f / _fps;
    }

    private void Update()
    {
        _skippedTime += Time.deltaTime;

        if (_thresholdTime > _skippedTime)
        {
            return;
        }

        // アニメーションの時間を計算する
        _animator.Update(_skippedTime);
        _skippedTime = 0f;
    }

    /// <summary>
    /// Inspectorの値変更時の処理
    /// </summary>
    private void OnValidate()
    {
        InitializeThresholdTime();
    }
}