/// <summary>
/// 디버그 로그 한 번에 비활성화
/// </summary>
public static class Logging
{
    [System.Diagnostics.Conditional("ENABLE_LOG")]
    static public void Log(object message)
    {
        UnityEngine.Debug.Log(message);
    }
}