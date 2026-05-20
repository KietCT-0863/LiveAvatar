using UnityEngine;

/// <summary>
/// One Euro Filter - Industry standard for smoothing noisy data
/// Reference: http://cristal.univ-lille.fr/~casiez/1euro/
/// 
/// Purpose: Reduces jitter in pose tracking data while maintaining responsiveness
/// </summary>
public class OneEuroFilter
{
    // Filter parameters
    private float minCutoff;  // Minimum cutoff frequency (lower = more smoothing)
    private float beta;       // Speed coefficient (higher = more responsive to fast movements)
    private float dCutoff;    // Derivative cutoff frequency
    
    // Filter state
    private float prevValue;
    private float prevDerivative;
    private float prevTime;
    private bool isInitialized;
    
    /// <summary>
    /// Constructor with default parameters optimized for pose tracking
    /// </summary>
    public OneEuroFilter()
    {
        this.minCutoff = 1.0f;
        this.beta = 0.007f;
        this.dCutoff = 1.0f;
        this.isInitialized = false;
    }
    
    /// <summary>
    /// Constructor with custom parameters
    /// </summary>
    /// <param name="minCutoff">Minimum cutoff frequency (default: 1.0)</param>
    /// <param name="beta">Speed coefficient (default: 0.007)</param>
    /// <param name="dCutoff">Derivative cutoff frequency (default: 1.0)</param>
    public OneEuroFilter(float minCutoff, float beta, float dCutoff)
    {
        this.minCutoff = minCutoff;
        this.beta = beta;
        this.dCutoff = dCutoff;
        this.isInitialized = false;
    }
    
    /// <summary>
    /// Apply filter to a single value
    /// </summary>
    /// <param name="value">Raw input value</param>
    /// <param name="timestamp">Current timestamp in seconds</param>
    /// <returns>Filtered (smoothed) value</returns>
    public float Filter(float value, float timestamp)
    {
        // Initialize on first call
        if (!isInitialized)
        {
            prevValue = value;
            prevDerivative = 0f;
            prevTime = timestamp;
            isInitialized = true;
            return value;
        }
        
        // Calculate time delta
        float dt = timestamp - prevTime;
        
        // Avoid division by zero
        if (dt <= 0f)
        {
            return prevValue;
        }
        
        // Calculate derivative (rate of change)
        float derivative = (value - prevValue) / dt;
        
        // Smooth the derivative
        float smoothedDerivative = LowPassFilter(derivative, prevDerivative, Alpha(dCutoff, dt));
        
        // Calculate adaptive cutoff frequency
        // Higher derivative = higher cutoff = less smoothing (more responsive)
        float cutoff = minCutoff + beta * Mathf.Abs(smoothedDerivative);
        
        // Apply low-pass filter to the value
        float smoothedValue = LowPassFilter(value, prevValue, Alpha(cutoff, dt));
        
        // Update state for next iteration
        prevValue = smoothedValue;
        prevDerivative = smoothedDerivative;
        prevTime = timestamp;
        
        return smoothedValue;
    }
    
    /// <summary>
    /// Calculate alpha (smoothing factor) from cutoff frequency and time delta
    /// </summary>
    /// <param name="cutoff">Cutoff frequency</param>
    /// <param name="dt">Time delta</param>
    /// <returns>Alpha value (0-1)</returns>
    private float Alpha(float cutoff, float dt)
    {
        float tau = 1.0f / (2.0f * Mathf.PI * cutoff);
        return 1.0f / (1.0f + tau / dt);
    }
    
    /// <summary>
    /// Low-pass filter implementation
    /// </summary>
    /// <param name="value">Current value</param>
    /// <param name="prevValue">Previous filtered value</param>
    /// <param name="alpha">Smoothing factor (0-1)</param>
    /// <returns>Filtered value</returns>
    private float LowPassFilter(float value, float prevValue, float alpha)
    {
        return alpha * value + (1.0f - alpha) * prevValue;
    }
    
    /// <summary>
    /// Reset filter state
    /// </summary>
    public void Reset()
    {
        isInitialized = false;
        prevValue = 0f;
        prevDerivative = 0f;
        prevTime = 0f;
    }
    
    /// <summary>
    /// Update filter parameters at runtime
    /// </summary>
    public void SetParameters(float minCutoff, float beta, float dCutoff)
    {
        this.minCutoff = minCutoff;
        this.beta = beta;
        this.dCutoff = dCutoff;
    }
}
