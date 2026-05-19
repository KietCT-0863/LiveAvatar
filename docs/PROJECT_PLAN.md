# 🎭 LiveAvatar - Real-time Motion Capture System

---

## 📊 Project Overview

**Project Name**: LiveAvatar

**Goal**: Build a real-time markerless motion capture system using Unity + MediaPipe to control a 3D avatar with live body movements.

**Description**: LiveAvatar captures your body movements through a webcam and mirrors them onto a 3D avatar in real-time. Perfect for VTuber streaming, interactive displays, holographic exhibitions (Holobox), virtual presentations, and more.

**Tech Stack**:
- **Platform**: Unity 2021.3 LTS or 2022.3 LTS (or Unity 6)
- **Motion Capture**: MediaPipe Pose (via MediaPipe Unity Plugin)
- **3D Engine**: Unity (built-in)
- **Model Format**: VRM (Virtual Reality Model) - recommended
- **Target Display**: Any screen (PC, Laptop, Projector, Holobox, etc.)
- **OS**: Windows 10/11
- **Language**: C# (Unity scripting)

**Architecture**:
```
Camera → MediaPipe (Pose Detection) → C# Processing → Unity Avatar → Display Output
```

**Use Cases**:
- 🎥 VTuber streaming
- 🎭 Interactive exhibitions
- 📺 Virtual presentations
- 🎪 Holographic displays (Holobox)
- 🎮 Gaming/VR applications
- 🎬 Virtual production

---

## 🎯 Core Features

### Must Have (MVP)
1. ✅ Real-time pose tracking from webcam (33 body landmarks)
2. ✅ Smooth motion data (anti-jittering filters)
3. ✅ Accurate bone rotation mapping (position → quaternion)
4. ✅ VRM avatar animation
5. ✅ Optimized lighting for depth perception (3-point lighting + shadows)
6. ✅ Customizable background (black for hologram, or any color)
7. ✅ Fullscreen/windowed mode support

### Nice to Have (Future)
- 🎥 Multi-camera support (360° tracking)
- 🔄 BVH recording and playback
- 🎨 Multiple lighting presets
- 🎭 Multiple avatar switching
- 🤖 Inverse Kinematics (IK) for feet grounding
- ⚙️ OSC output for external control
- 🎬 Video recording/streaming integration
- 🖼️ Custom backgrounds (images, videos, green screen)

---

## 📁 Project Structure

```
LiveAvatar/
├── Assets/
│   ├── MediaPipe/                        # MediaPipe Unity Plugin
│   │   ├── SDK/
│   │   ├── Samples/
│   │   └── Resources/
│   ├── Models/
│   │   └── Avatar.vrm                    # Your VRM model
│   ├── Scenes/
│   │   └── MainScene.unity               # Main scene
│   ├── Scripts/
│   │   ├── Core/
│   │   │   ├── CameraManager.cs          # Webcam management
│   │   │   ├── MediaPipeManager.cs       # MediaPipe integration
│   │   │   └── PoseDataProcessor.cs      # Data smoothing & filtering
│   │   ├── Animation/
│   │   │   ├── PoseController.cs         # Main pose → avatar controller
│   │   │   ├── BoneMapper.cs             # Landmark → bone mapping
│   │   │   └── RotationCalculator.cs     # Position → rotation math
│   │   ├── Filters/
│   │   │   ├── OneEuroFilter.cs          # Anti-jittering filter
│   │   │   └── KalmanFilter.cs           # Alternative filter
│   │   └── Display/
│   │       ├── LightingController.cs     # Lighting setup
│   │       └── CameraController.cs       # Camera positioning
│   ├── Materials/
│   │   ├── AvatarMaterial.mat
│   │   └── ShadowOnlyMaterial.mat        # Ground shadow
│   └── Prefabs/
│       ├── AvatarRig.prefab
│       └── LightingSetup.prefab
├── Packages/
│   ├── com.github.homuler.mediapipe/     # MediaPipe Unity Plugin
│   └── com.vrmc.univrm/                  # UniVRM package
└── ProjectSettings/
    └── QualitySettings.asset
```

---

## 🚀 Development Phases (Master Plan)

---

## **Phase 1: Infrastructure Setup** (Day 1-2)

**Goal**: Setup Unity environment and import all required libraries

### Tasks:

#### 1.1 Install Unity Editor
```
- Download Unity Hub
- Install Unity 2021.3 LTS or 2022.3 LTS
- Add modules:
  ✅ Windows Build Support
  ✅ Visual Studio (if not installed)
```

#### 1.2 Create New Unity Project
```
1. Open Unity Hub
2. New Project → 3D (URP or Built-in Render Pipeline)
3. Project Name: LiveAvatar
4. Location: Choose your workspace
```

#### 1.3 Import MediaPipe Unity Plugin
```
Repository: https://github.com/homuler/MediaPipeUnityPlugin

Installation:
1. Open Package Manager (Window → Package Manager)
2. Click "+" → Add package from git URL
3. Enter: https://github.com/homuler/MediaPipeUnityPlugin.git?path=/Packages/com.github.homuler.mediapipe

Or manual:
1. Clone repository
2. Copy Packages/com.github.homuler.mediapipe to your project
```

#### 1.4 Import UniVRM
```
Repository: https://github.com/vrm-c/UniVRM

Installation:
1. Download latest UniVRM package (.unitypackage)
2. Assets → Import Package → Custom Package
3. Select UniVRM package
4. Import all

Or via Package Manager:
1. Add package from git URL
2. Enter: https://github.com/vrm-c/UniVRM.git?path=/Assets/VRMShaders
3. Then: https://github.com/vrm-c/UniVRM.git?path=/Assets/UniGLTF
4. Finally: https://github.com/vrm-c/UniVRM.git?path=/Assets/VRM
```

### Deliverables:
- ✅ Unity 2021.3/2022.3 LTS installed
- ✅ MediaPipe Unity Plugin imported
- ✅ UniVRM imported
- ✅ Project structure created

---

## **Phase 2: Backend Integration** (Day 3-5)

**Goal**: Implement data pipeline from camera to pose landmarks

### Tasks:

#### 2.1 Camera Setup (CameraManager.cs)
```csharp
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private WebCamTexture webCamTexture;
    
    void Start()
    {
        // Initialize webcam
        webCamTexture = new WebCamTexture(640, 480, 30);
        webCamTexture.Play();
    }
    
    public Texture GetCameraTexture()
    {
        return webCamTexture;
    }
}
```

**Key Points**:
- Use 640x480 or 1280x720 resolution (balance quality vs performance)
- 30fps is sufficient for pose detection
- Handle camera permissions

#### 2.2 MediaPipe Integration (MediaPipeManager.cs)
```csharp
using Mediapipe;
using Mediapipe.Unity;

public class MediaPipeManager : MonoBehaviour
{
    private PoseTrackingGraph poseGraph;
    private NormalizedLandmarkList poseLandmarks;
    
    void Start()
    {
        // Initialize MediaPipe Pose Graph
        poseGraph = new PoseTrackingGraph();
        poseGraph.Initialize();
        poseGraph.StartRun();
    }
    
    void Update()
    {
        // Feed camera frame to MediaPipe
        Texture2D frame = GetCameraFrame();
        poseGraph.AddTextureFrameToInputStream(frame);
        
        // Get pose landmarks (33 points)
        poseLandmarks = poseGraph.FetchNextValue();
    }
    
    public NormalizedLandmarkList GetPoseLandmarks()
    {
        return poseLandmarks;
    }
}
```

**Output**: 33 body landmarks with X, Y, Z coordinates + visibility score

#### 2.3 Anti-Jittering Filter (OneEuroFilter.cs)
```csharp
// One Euro Filter - Industry standard for smoothing
public class OneEuroFilter
{
    private float minCutoff = 1.0f;
    private float beta = 0.007f;
    private float dCutoff = 1.0f;
    
    private float prevValue;
    private float prevDerivative;
    private float prevTime;
    
    public float Filter(float value, float timestamp)
    {
        float dt = timestamp - prevTime;
        
        // Calculate derivative
        float derivative = (value - prevValue) / dt;
        float smoothedDerivative = LowPassFilter(derivative, prevDerivative, Alpha(dCutoff, dt));
        
        // Calculate cutoff frequency
        float cutoff = minCutoff + beta * Mathf.Abs(smoothedDerivative);
        
        // Apply low-pass filter
        float smoothedValue = LowPassFilter(value, prevValue, Alpha(cutoff, dt));
        
        // Update state
        prevValue = smoothedValue;
        prevDerivative = smoothedDerivative;
        prevTime = timestamp;
        
        return smoothedValue;
    }
    
    private float Alpha(float cutoff, float dt)
    {
        float tau = 1.0f / (2.0f * Mathf.PI * cutoff);
        return 1.0f / (1.0f + tau / dt);
    }
    
    private float LowPassFilter(float value, float prevValue, float alpha)
    {
        return alpha * value + (1.0f - alpha) * prevValue;
    }
}
```

**Why Critical**: Raw MediaPipe data is noisy → causes jittery animations

#### 2.4 Pose Data Processor (PoseDataProcessor.cs)
```csharp
public class PoseDataProcessor : MonoBehaviour
{
    private OneEuroFilter[] filters; // One filter per landmark coordinate
    
    void Start()
    {
        // Initialize 33 landmarks × 3 coordinates = 99 filters
        filters = new OneEuroFilter[99];
        for (int i = 0; i < 99; i++)
        {
            filters[i] = new OneEuroFilter();
        }
    }
    
    public Vector3[] ProcessLandmarks(NormalizedLandmarkList rawLandmarks)
    {
        Vector3[] smoothedLandmarks = new Vector3[33];
        float timestamp = Time.time;
        
        for (int i = 0; i < 33; i++)
        {
            float x = filters[i * 3 + 0].Filter(rawLandmarks[i].X, timestamp);
            float y = filters[i * 3 + 1].Filter(rawLandmarks[i].Y, timestamp);
            float z = filters[i * 3 + 2].Filter(rawLandmarks[i].Z, timestamp);
            
            smoothedLandmarks[i] = new Vector3(x, y, z);
        }
        
        return smoothedLandmarks;
    }
}
```

### Deliverables:
- ✅ Camera feed working
- ✅ MediaPipe detecting 33 landmarks
- ✅ Smooth pose data (no jitter)
- ✅ 30fps pose detection

---

## **Phase 3: Rigging & Animation** (Day 6-9)

**Goal**: Convert pose landmarks to avatar bone rotations

### Tasks:

#### 3.1 Bone Mapping (BoneMapper.cs)
```csharp
public class BoneMapper : MonoBehaviour
{
    // MediaPipe Landmark indices
    public enum LandmarkIndex
    {
        Nose = 0,
        LeftShoulder = 11,
        RightShoulder = 12,
        LeftElbow = 13,
        RightElbow = 14,
        LeftWrist = 15,
        RightWrist = 16,
        LeftHip = 23,
        RightHip = 24,
        LeftKnee = 25,
        RightKnee = 26,
        LeftAnkle = 27,
        RightAnkle = 28
    }
    
    // Unity Humanoid Bones
    private Transform head;
    private Transform spine;
    private Transform leftShoulder;
    private Transform rightShoulder;
    private Transform leftUpperArm;
    private Transform rightUpperArm;
    private Transform leftLowerArm;
    private Transform rightLowerArm;
    private Transform leftUpperLeg;
    private Transform rightUpperLeg;
    private Transform leftLowerLeg;
    private Transform rightLowerLeg;
    
    public void InitializeBones(Animator animator)
    {
        // Get bone references from Animator
        head = animator.GetBoneTransform(HumanBodyBones.Head);
        spine = animator.GetBoneTransform(HumanBodyBones.Spine);
        leftShoulder = animator.GetBoneTransform(HumanBodyBones.LeftShoulder);
        rightShoulder = animator.GetBoneTransform(HumanBodyBones.RightShoulder);
        leftUpperArm = animator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
        rightUpperArm = animator.GetBoneTransform(HumanBodyBones.RightUpperArm);
        leftLowerArm = animator.GetBoneTransform(HumanBodyBones.LeftLowerArm);
        rightLowerArm = animator.GetBoneTransform(HumanBodyBones.RightLowerArm);
        leftUpperLeg = animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
        rightUpperLeg = animator.GetBoneTransform(HumanBodyBones.RightUpperLeg);
        leftLowerLeg = animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
        rightLowerLeg = animator.GetBoneTransform(HumanBodyBones.RightLowerLeg);
    }
}
```

#### 3.2 Rotation Calculator (RotationCalculator.cs)
```csharp
public class RotationCalculator
{
    // Calculate rotation from 3 points (parent, current, child)
    public static Quaternion CalculateBoneRotation(Vector3 parent, Vector3 current, Vector3 child)
    {
        // Vector from parent to current
        Vector3 direction = (current - parent).normalized;
        
        // Vector from current to child
        Vector3 childDirection = (child - current).normalized;
        
        // Calculate rotation to align with direction
        Quaternion rotation = Quaternion.LookRotation(childDirection, Vector3.up);
        
        return rotation;
    }
    
    // Calculate arm rotation (shoulder → elbow → wrist)
    public static Quaternion CalculateArmRotation(Vector3 shoulder, Vector3 elbow, Vector3 wrist)
    {
        Vector3 upperArm = (elbow - shoulder).normalized;
        Vector3 foreArm = (wrist - elbow).normalized;
        
        // Calculate elbow bend angle
        float angle = Vector3.Angle(upperArm, foreArm);
        
        // Create rotation
        Vector3 axis = Vector3.Cross(upperArm, foreArm).normalized;
        Quaternion rotation = Quaternion.AngleAxis(angle, axis);
        
        return rotation;
    }
}
```

#### 3.3 Pose Controller (PoseController.cs) - Main Script
```csharp
public class PoseController : MonoBehaviour
{
    [Header("References")]
    public Animator avatarAnimator;
    public MediaPipeManager mediaPipeManager;
    public PoseDataProcessor dataProcessor;
    
    private BoneMapper boneMapper;
    
    void Start()
    {
        boneMapper = new BoneMapper();
        boneMapper.InitializeBones(avatarAnimator);
    }
    
    void LateUpdate()
    {
        // Get smoothed landmarks
        Vector3[] landmarks = dataProcessor.ProcessLandmarks(
            mediaPipeManager.GetPoseLandmarks()
        );
        
        // Apply rotations to avatar bones
        ApplyPoseToAvatar(landmarks);
    }
    
    void ApplyPoseToAvatar(Vector3[] landmarks)
    {
        // Head rotation
        Vector3 nose = landmarks[(int)BoneMapper.LandmarkIndex.Nose];
        Vector3 leftShoulder = landmarks[(int)BoneMapper.LandmarkIndex.LeftShoulder];
        Vector3 rightShoulder = landmarks[(int)BoneMapper.LandmarkIndex.RightShoulder];
        Vector3 neckCenter = (leftShoulder + rightShoulder) / 2f;
        
        Quaternion headRotation = RotationCalculator.CalculateBoneRotation(
            neckCenter, nose, Vector3.up
        );
        boneMapper.head.rotation = headRotation;
        
        // Left arm
        Vector3 leftElbow = landmarks[(int)BoneMapper.LandmarkIndex.LeftElbow];
        Vector3 leftWrist = landmarks[(int)BoneMapper.LandmarkIndex.LeftWrist];
        
        Quaternion leftArmRotation = RotationCalculator.CalculateArmRotation(
            leftShoulder, leftElbow, leftWrist
        );
        boneMapper.leftUpperArm.rotation = leftArmRotation;
        
        // Right arm
        Vector3 rightElbow = landmarks[(int)BoneMapper.LandmarkIndex.RightElbow];
        Vector3 rightWrist = landmarks[(int)BoneMapper.LandmarkIndex.RightWrist];
        
        Quaternion rightArmRotation = RotationCalculator.CalculateArmRotation(
            rightShoulder, rightElbow, rightWrist
        );
        boneMapper.rightUpperArm.rotation = rightArmRotation;
        
        // Legs (similar logic)
        // ... implement leg rotations
    }
}
```

#### 3.4 Inverse Kinematics (Optional - Advanced)
```
Use Unity's Animation Rigging package:
1. Window → Package Manager
2. Install "Animation Rigging"
3. Add Two Bone IK Constraint to legs
4. Ensure feet always touch ground plane
```

### Deliverables:
- ✅ Avatar bones mapped to landmarks
- ✅ Smooth, natural-looking movements
- ✅ Arms, legs, head tracking correctly
- ✅ No unnatural twisting or flipping

---

## **Phase 4: Display Optimization** (Day 10-11)

**Goal**: Setup lighting and rendering for optimal visual quality

### Tasks:

#### 4.1 Camera Setup (CameraController.cs)
```csharp
public class CameraController : MonoBehaviour
{
    [Header("Background Settings")]
    public Color backgroundColor = Color.black;
    public bool useCustomBackground = false;
    
    void Start()
    {
        Camera mainCamera = Camera.main;
        
        // Background setup
        mainCamera.clearFlags = CameraClearFlags.SolidColor;
        mainCamera.backgroundColor = backgroundColor;
        
        // Camera positioning
        mainCamera.fieldOfView = 45f;
        mainCamera.transform.position = new Vector3(0, 1.5f, 5f);
        mainCamera.transform.LookAt(new Vector3(0, 1f, 0));
    }
    
    public void SetBackgroundColor(Color color)
    {
        backgroundColor = color;
        Camera.main.backgroundColor = color;
    }
}
```

#### 4.2 Lighting Setup (LightingController.cs)
```csharp
public class LightingController : MonoBehaviour
{
    void Start()
    {
        // Ambient light setup
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.15f, 0.15f, 0.15f);
        
        // Create 3-point lighting
        CreateKeyLight();
        CreateFillLight();
        CreateRimLight();
        CreateGroundPlane();
    }
    
    void CreateKeyLight()
    {
        GameObject keyLight = new GameObject("Key Light");
        Light light = keyLight.AddComponent<Light>();
        light.type = LightType.Directional;
        light.color = Color.white;
        light.intensity = 1.0f;
        light.shadows = LightShadows.Soft;
        light.shadowStrength = 1.0f;
        keyLight.transform.rotation = Quaternion.Euler(50, -30, 0);
    }
    
    void CreateFillLight()
    {
        GameObject fillLight = new GameObject("Fill Light");
        Light light = fillLight.AddComponent<Light>();
        light.type = LightType.Directional;
        light.color = Color.white;
        light.intensity = 0.3f;
        light.shadows = LightShadows.None;
        fillLight.transform.rotation = Quaternion.Euler(30, 150, 0);
    }
    
    void CreateRimLight()
    {
        GameObject rimLight = new GameObject("Rim Light");
        Light light = rimLight.AddComponent<Light>();
        light.type = LightType.Directional;
        light.color = new Color(0.4f, 0.6f, 1.0f); // Light blue
        light.intensity = 0.5f;
        light.shadows = LightShadows.None;
        rimLight.transform.rotation = Quaternion.Euler(0, 180, 0);
    }
    
    void CreateGroundPlane()
    {
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground Shadow Plane";
        ground.transform.position = Vector3.zero;
        ground.transform.localScale = new Vector3(5, 1, 5);
        
        // Shadow-only material
        Material shadowMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        shadowMat.color = new Color(0, 0, 0, 0); // Transparent
        shadowMat.SetFloat("_Surface", 1); // Transparent mode
        ground.GetComponent<Renderer>().material = shadowMat;
        ground.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        ground.GetComponent<Renderer>().receiveShadows = true;
    }
}
```

#### 4.3 Quality Settings
```
Edit → Project Settings → Quality

Shadows:
- Shadow Quality: High
- Shadow Resolution: High (2048)
- Shadow Distance: 50
- Shadow Cascades: Two Cascades

Anti-Aliasing:
- MSAA: 4x (if performance allows)

V-Sync:
- Enabled (for stable framerate)
```

### Deliverables:
- ✅ Customizable background color
- ✅ 3-point lighting with shadows
- ✅ Strong depth perception
- ✅ Avatar looks natural and well-lit

---

## **Phase 5: Deployment** (Day 12)

**Goal**: Build standalone executable for Holobox

### Tasks:

#### 5.1 Build Settings
```
File → Build Settings

Platform: Windows
Architecture: x86_64
Target: Standalone

Player Settings:
- Company Name: Your Company
- Product Name: LiveAvatar
- Fullscreen Mode: Fullscreen Window (or Windowed)
- Default Screen Width: 1920
- Default Screen Height: 1080
- Display Resolution Dialog: Enabled (allow user to choose)
- Resizable Window: Enabled
- Run in Background: Enabled
- Visible in Background: Enabled
```

#### 5.2 Optimization
```csharp
// Add to main script
void Start()
{
    // Target 60fps
    Application.targetFrameRate = 60;
    
    // Disable screen dimming
    Screen.sleepTimeout = SleepTimeout.NeverSleep;
    
    // Quality settings
    QualitySettings.vSyncCount = 1;
}
```

#### 5.3 Auto-Start Script (Optional)
```batch
@echo off
REM LiveAvatar Auto-Start Script
cd /d "%~dp0"

REM Kill any existing instance
taskkill /F /IM "LiveAvatar.exe" 2>nul

REM Wait 2 seconds
timeout /t 2 /nobreak >nul

REM Start application
start "" "LiveAvatar.exe"

REM Log startup
echo %date% %time% - LiveAvatar started >> startup.log
```

#### 5.4 Windows Startup (Optional)
```
1. Press Win+R
2. Type: shell:startup
3. Create shortcut to auto-start.bat
4. Application will start on Windows boot
```

### Deliverables:
- ✅ Standalone .exe file
- ✅ Fullscreen/windowed mode support
- ✅ Auto-start script (optional)
- ✅ Production-ready

---

## 🔧 Technical Specifications

### Performance Targets
- **Render FPS**: 60fps (Unity)
- **Pose Detection**: 30fps (MediaPipe)
- **Latency**: < 100ms (camera to avatar)
- **Memory**: < 2GB RAM

### System Requirements
- **OS**: Windows 10/11 (64-bit)
- **CPU**: Intel i5-8th gen / Ryzen 5 3600 or better
- **RAM**: 8GB minimum, 16GB recommended
- **GPU**: 
  - GTX 1060 6GB or better (recommended)
  - RTX 2060 or better (optimal)
- **Webcam**: 720p @ 30fps minimum
- **Unity**: 2021.3 LTS or 2022.3 LTS

### Model Requirements
- **Format**: VRM (recommended)
- **Rigging**: Humanoid skeleton (Unity standard)
- **Polycount**: < 50k triangles
- **Textures**: < 2048x2048
- **Shaders**: MToon (VRM standard) or URP/Lit

---

## 📦 Dependencies

### Unity Packages
```
1. MediaPipe Unity Plugin
   - Repository: https://github.com/homuler/MediaPipeUnityPlugin
   - Version: Latest stable

2. UniVRM
   - Repository: https://github.com/vrm-c/UniVRM
   - Version: 0.112.0 or later

3. Animation Rigging (Optional)
   - Built-in Unity package
   - For IK constraints
```

### External Resources
```
- VRM Models: https://hub.vroid.com/
- VRoid Studio: https://vroid.com/en/studio (create custom avatars)
```

---

## 📚 Key Algorithms

### One Euro Filter (Anti-Jittering)
```
Purpose: Smooth noisy pose data
Parameters:
- minCutoff: 1.0 (minimum cutoff frequency)
- beta: 0.007 (speed coefficient)
- dCutoff: 1.0 (derivative cutoff frequency)

Reference: http://cristal.univ-lille.fr/~casiez/1euro/
```

### Quaternion Rotation
```
Purpose: Convert position vectors to bone rotations
Method: Vector math → Quaternion.LookRotation()
Advantage: No gimbal lock, smooth interpolation
```

### Inverse Kinematics (Optional)
```
Purpose: Ensure feet touch ground
Method: Two-Bone IK constraint
Advantage: More natural leg movements
```

---

## 🐛 Troubleshooting

### Issue 1: MediaPipe Plugin Not Found
```
Solution:
1. Check Package Manager → Packages: In Project
2. Verify com.github.homuler.mediapipe is listed
3. If not, re-import from git URL
4. Check Unity version compatibility (2021.3+ required)
```

### Issue 2: VRM Model Not Loading
```
Solution:
1. Ensure UniVRM is imported correctly
2. Check model is valid VRM format
3. Try importing via: Assets → VRM → Import VRM
4. Check Console for error messages
```

### Issue 3: Jittery Animations
```
Solution:
1. Verify OneEuroFilter is applied
2. Adjust filter parameters (increase minCutoff)
3. Check camera lighting (poor lighting = noisy data)
4. Reduce camera resolution if CPU bottleneck
```

### Issue 4: Low FPS
```
Solution:
1. Reduce shadow quality (Edit → Project Settings → Quality)
2. Lower camera resolution (640x480)
3. Reduce model polycount
4. Disable post-processing effects
5. Check GPU is being used (not integrated graphics)
```

### Issue 5: Bones Twisting Unnaturally
```
Solution:
1. Check bone mapping is correct
2. Verify rotation calculation logic
3. Add rotation constraints (limit angles)
4. Use Quaternion.Slerp for smooth interpolation
```

---

## 📊 Success Metrics

### Technical
- ✅ 60fps rendering
- ✅ 30fps pose detection
- ✅ < 100ms latency
- ✅ Smooth animations (no visible jitter)
- ✅ Accurate tracking (arms, legs, head)

### Visual Quality
- ✅ Customizable background (black, white, or any color)
- ✅ Strong shadows on ground
- ✅ Clear depth perception
- ✅ Natural lighting (not flat)
- ✅ Avatar centered and scaled correctly

### User Experience
- ✅ Responsive tracking (immediate feedback)
- ✅ Natural-looking movements
- ✅ Works in normal indoor lighting
- ✅ No crashes or freezes
- ✅ Easy to deploy and run

---

## 🎯 Next Steps

### Immediate Actions:
1. ✅ **Check Unity version on your machine**
2. ✅ **Decide: Use existing Unity or install 2021.3/2022.3 LTS**
3. ✅ **Prepare VRM model** (create or download)
4. ✅ **Start Phase 1** - Setup infrastructure

### This Week:
- Day 1-2: Infrastructure setup
- Day 3-5: Backend integration (MediaPipe)
- Day 6-9: Rigging & animation (hardest part)
- Day 10-11: Holobox optimization
- Day 12: Build & deploy

### Questions to Answer:
- ❓ Do you have a VRM model ready?
- ❓ What Unity version is currently installed?
- ❓ GPU specs? (for performance planning)

---

## 💡 Pro Tips

1. **Start Simple**: Get basic tracking working first, then add smoothing
2. **Test Frequently**: Test on target display early to catch visual issues
3. **Lighting is Key**: Spend time on lighting setup - it makes or breaks the visual quality
4. **Version Control**: Use Git to track changes
5. **Backup**: Keep backup of working builds
6. **Performance**: Monitor FPS and optimize early

---

**Estimated Timeline**: 12 days (MVP)  
**Difficulty**: Intermediate to Advanced (C# + Unity + Math required)  
**Key Success Factor**: Proper motion mapping and anti-jittering filters

---

**Ready to check Unity version and start Phase 1! 🚀**
