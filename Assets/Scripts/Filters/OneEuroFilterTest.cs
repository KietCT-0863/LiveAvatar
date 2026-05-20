using UnityEngine;

/// <summary>
/// Test script for OneEuroFilter
/// Demonstrates filter behavior with noisy input
/// </summary>
public class OneEuroFilterTest : MonoBehaviour
{
    [Header("Test Settings")]
    [SerializeField] private bool runTest = true;
    [SerializeField] private float noiseAmplitude = 0.5f;
    
    private OneEuroFilter filter;
    private float testValue = 0f;
    
    void Start()
    {
        // Initialize filter
        filter = new OneEuroFilter();
        
        Debug.Log("[OneEuroFilterTest] Filter initialized");
        Debug.Log("[OneEuroFilterTest] Press Space to toggle test");
    }
    
    void Update()
    {
        // Toggle test with Space key
        if (Input.GetKeyDown(KeyCode.Space))
        {
            runTest = !runTest;
            Debug.Log($"[OneEuroFilterTest] Test {(runTest ? "started" : "stopped")}");
        }
        
        if (!runTest) return;
        
        // Generate test value (sine wave)
        testValue = Mathf.Sin(Time.time * 2f);
        
        // Add noise
        float noisyValue = testValue + Random.Range(-noiseAmplitude, noiseAmplitude);
        
        // Apply filter
        float filteredValue = filter.Filter(noisyValue, Time.time);
        
        // Log every second
        if (Time.frameCount % 60 == 0)
        {
            Debug.Log($"[OneEuroFilterTest] Original: {testValue:F3} | Noisy: {noisyValue:F3} | Filtered: {filteredValue:F3}");
        }
        
        // Visualize with Debug.DrawLine (in Scene view)
        Vector3 noisyPos = new Vector3(Time.time % 10f, noisyValue, 0);
        Vector3 filteredPos = new Vector3(Time.time % 10f, filteredValue, 0);
        
        Debug.DrawLine(noisyPos, noisyPos + Vector3.up * 0.1f, Color.red, 0.1f);
        Debug.DrawLine(filteredPos, filteredPos + Vector3.up * 0.1f, Color.green, 0.1f);
    }
}
