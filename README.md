# 🎭 LiveAvatar - Real-time Motion Capture System

[![Unity](https://img.shields.io/badge/Unity-2022.3%20LTS-black?logo=unity)](https://unity.com/)
[![MediaPipe](https://img.shields.io/badge/MediaPipe-0.16.3-blue)](https://github.com/homuler/MediaPipeUnityPlugin)
[![UniVRM](https://img.shields.io/badge/UniVRM-0.131.1-green)](https://github.com/vrm-c/UniVRM)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

> Real-time markerless motion capture system using Unity + MediaPipe to control 3D avatars with live body movements.

![LiveAvatar Demo](https://via.placeholder.com/800x400/000000/FFFFFF?text=LiveAvatar+Demo)

---

## 📖 Overview

**LiveAvatar** captures your body movements through a webcam and mirrors them onto a 3D avatar in real-time. Perfect for VTuber streaming, interactive displays, holographic exhibitions (Holobox), virtual presentations, and more.

### ✨ Key Features

- 🎥 **Real-time pose tracking** - 33 body landmarks detection via MediaPipe
- 🎯 **Smooth animations** - Anti-jittering filters for natural movements
- 🎭 **VRM avatar support** - Industry-standard VRM format
- 💡 **Optimized lighting** - 3-point lighting system with shadows
- 🎨 **Customizable background** - Black, white, or any color
- ⚡ **High performance** - 60fps rendering, 30fps pose detection
- 🖥️ **Cross-display support** - PC, laptop, projector, Holobox

---

## 🎯 Use Cases

- 🎥 VTuber streaming
- 🎭 Interactive exhibitions
- 📺 Virtual presentations
- 🎪 Holographic displays (Holobox)
- 🎮 Gaming/VR applications
- 🎬 Virtual production

---

## 🚀 Quick Start

### Prerequisites

- **OS**: Windows 10/11 (64-bit)
- **Unity**: 2021.3 LTS or 2022.3 LTS
- **Webcam**: 720p @ 30fps minimum
- **GPU**: GTX 1060 6GB or better (recommended)
- **RAM**: 8GB minimum, 16GB recommended

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/KietCT-0863/LiveAvatar.git
   cd LiveAvatar
   ```

2. **Open in Unity**
   - Open Unity Hub
   - Click "Add" → Select `LiveAvatar` folder
   - Open project with Unity 2022.3 LTS

3. **Packages are auto-installed**
   - MediaPipe Unity Plugin (0.16.3)
   - UniVRM (0.131.1)
   - All dependencies resolved automatically

4. **Import VRM Avatar** (Optional)
   - Download VRM model from [VRoid Hub](https://hub.vroid.com/)
   - Drag `.vrm` file into `Assets/Models/` folder
   - Unity will auto-import via UniVRM

5. **Run the project**
   - Open `Assets/Scenes/MainScene.unity`
   - Click Play ▶️

---

## 📁 Project Structure

```
LiveAvatar/
├── Assets/
│   ├── Models/              # VRM avatar models
│   ├── Scenes/              # Unity scenes
│   │   └── MainScene.unity  # Main scene
│   ├── Scripts/             # C# scripts
│   │   ├── Core/            # Camera, MediaPipe, Data processing
│   │   ├── Animation/       # Pose controller, Bone mapping
│   │   ├── Filters/         # OneEuroFilter, Kalman filter
│   │   └── Display/         # Lighting, Camera control
│   ├── Materials/           # Materials and shaders
│   └── Prefabs/             # Reusable prefabs
├── Packages/
│   ├── com.github.homuler.mediapipe/  # MediaPipe Unity Plugin
│   ├── com.vrmc.gltf/                 # UniGLTF
│   ├── com.vrmc.vrm/                  # VRM 0.x
│   └── com.vrmc.vrm10/                # VRM 1.0
└── ProjectSettings/         # Unity project settings
```

---

## 🛠️ Tech Stack

| Component | Technology | Version |
|-----------|-----------|---------|
| **Platform** | Unity | 2022.3 LTS |
| **Motion Capture** | MediaPipe Pose | 0.16.3 |
| **Avatar Format** | VRM (UniVRM) | 0.131.1 |
| **Language** | C# | .NET Standard 2.1 |
| **Rendering** | URP | 17.4.0 |

---

## 📊 Architecture

```
┌─────────────┐
│   Webcam    │
└──────┬──────┘
       │
       ▼
┌─────────────────────┐
│  MediaPipe Pose     │  ← 33 body landmarks
│  (Pose Detection)   │
└──────┬──────────────┘
       │
       ▼
┌─────────────────────┐
│  OneEuroFilter      │  ← Anti-jittering
│  (Data Smoothing)   │
└──────┬──────────────┘
       │
       ▼
┌─────────────────────┐
│  Bone Mapper        │  ← Landmark → Bone
│  (Rotation Calc)    │
└──────┬──────────────┘
       │
       ▼
┌─────────────────────┐
│  Unity Avatar       │  ← Real-time animation
│  (VRM Model)        │
└─────────────────────┘
```

---

## 🎨 Core Components

### 1. Camera Manager
- Webcam initialization and management
- Resolution: 640x480 or 1280x720
- Frame rate: 30fps

### 2. MediaPipe Manager
- Pose detection using MediaPipe
- 33 body landmarks extraction
- Real-time processing

### 3. Pose Data Processor
- OneEuroFilter for smoothing
- 99 filters (33 landmarks × 3 coordinates)
- Noise reduction

### 4. Bone Mapper
- Maps MediaPipe landmarks to Unity bones
- Supports Humanoid rig
- Head, arms, legs, spine tracking

### 5. Rotation Calculator
- Converts positions to quaternions
- Calculates bone rotations
- Smooth interpolation

### 6. Lighting Controller
- 3-point lighting system
- Key, Fill, Rim lights
- Shadow rendering

---

## ⚙️ Configuration

### Performance Settings

Edit `ProjectSettings/QualitySettings.asset`:

```
Shadows:
- Quality: High
- Resolution: 2048
- Distance: 50
- Cascades: Two Cascades

Anti-Aliasing: MSAA 4x
V-Sync: Enabled
Target FPS: 60
```

### Camera Settings

```csharp
Resolution: 640x480 (balanced) or 1280x720 (high quality)
Frame Rate: 30fps
Field of View: 45°
Position: (0, 1.5, 5)
```

### Filter Parameters

```csharp
OneEuroFilter:
- minCutoff: 1.0
- beta: 0.007
- dCutoff: 1.0
```

---

## 🎯 Development Roadmap

### Phase 1: Infrastructure Setup ✅
- [x] Unity project setup
- [x] MediaPipe Unity Plugin integration
- [x] UniVRM package installation
- [x] Git repository initialization

### Phase 2: Backend Integration (In Progress)
- [ ] Camera manager implementation
- [ ] MediaPipe pose detection
- [ ] OneEuroFilter integration
- [ ] Pose data processor

### Phase 3: Rigging & Animation
- [ ] Bone mapper
- [ ] Rotation calculator
- [ ] Pose controller
- [ ] IK constraints (optional)

### Phase 4: Display Optimization
- [ ] Lighting setup
- [ ] Camera controller
- [ ] Background customization
- [ ] Quality settings

### Phase 5: Deployment
- [ ] Build settings
- [ ] Performance optimization
- [ ] Standalone executable
- [ ] Auto-start script

---

## 📈 Performance Targets

| Metric | Target | Status |
|--------|--------|--------|
| Render FPS | 60fps | 🎯 |
| Pose Detection | 30fps | 🎯 |
| Latency | < 100ms | 🎯 |
| Memory Usage | < 2GB | 🎯 |

---

## 🐛 Troubleshooting

### MediaPipe Plugin Not Found
```bash
# Re-import via Package Manager
Window → Package Manager → "+" → Add package from git URL
https://github.com/homuler/MediaPipeUnityPlugin.git?path=/Packages/com.github.homuler.mediapipe
```

### VRM Model Not Loading
```bash
# Check UniVRM installation
Assets → VRM → Import VRM
# Ensure model is valid VRM format
```

### Jittery Animations
```csharp
// Adjust OneEuroFilter parameters
minCutoff = 1.5f;  // Increase for more smoothing
beta = 0.01f;      // Adjust responsiveness
```

### Low FPS
```
1. Reduce shadow quality (Project Settings → Quality)
2. Lower camera resolution (640x480)
3. Disable post-processing effects
4. Check GPU usage (not integrated graphics)
```

---

## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'feat: add amazing feature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Commit Convention

Follow [Conventional Commits](https://www.conventionalcommits.org/):

```
feat: add new feature
fix: bug fix
docs: documentation changes
refactor: code refactoring
style: code style changes
test: add tests
chore: maintenance tasks
```

---

## 📚 Resources

### Documentation
- [MediaPipe Pose](https://google.github.io/mediapipe/solutions/pose.html)
- [UniVRM Documentation](https://vrm.dev/en/univrm/)
- [Unity Manual](https://docs.unity3d.com/Manual/index.html)

### VRM Models
- [VRoid Hub](https://hub.vroid.com/) - Free VRM models
- [VRoid Studio](https://vroid.com/en/studio) - Create custom avatars

### Algorithms
- [One Euro Filter](http://cristal.univ-lille.fr/~casiez/1euro/) - Smoothing algorithm

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 👤 Author

**KietCT-0863**
- GitHub: [@KietCT-0863](https://github.com/KietCT-0863)
- Email: kietvipro123456789@gmail.com

---

## 🙏 Acknowledgments

- [MediaPipe Unity Plugin](https://github.com/homuler/MediaPipeUnityPlugin) by homuler
- [UniVRM](https://github.com/vrm-c/UniVRM) by VRM Consortium
- [One Euro Filter](http://cristal.univ-lille.fr/~casiez/1euro/) by Géry Casiez

---

## 📞 Support

If you have any questions or issues:

1. Check [Troubleshooting](#-troubleshooting) section
2. Search [existing issues](https://github.com/KietCT-0863/LiveAvatar/issues)
3. Create a [new issue](https://github.com/KietCT-0863/LiveAvatar/issues/new)

---

**⭐ Star this repo if you find it useful!**

---

*Last updated: May 20, 2026*
