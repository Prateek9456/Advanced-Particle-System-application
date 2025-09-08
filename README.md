# Advanced Particle System - Water, Smoke & Sparks

A sophisticated WPF application featuring realistic particle systems with advanced physics simulation. Create stunning visual effects with Water droplets, Smoke clouds, and Electric sparks using n-particle systems.

![Particle System Demo](https://img.shields.io/badge/WPF-Particle%20System-blue)
![.NET](https://img.shields.io/badge/.NET-6.0%2B-purple)
![Physics](https://img.shields.io/badge/Physics-Real%20Time-green)

## ✨ Features

### 🌊 Water Particle System
- **Realistic Physics**: Gravity-affected water droplets with proper acceleration
- **Fluid Dynamics**: Droplets with natural falling motion and splash effects
- **Visual Effects**: Semi-transparent droplets with light reflection gradients
- **Burst Mode**: Right-click for explosive water fountain effects

### 💨 Smoke Particle System
- **Buoyancy Simulation**: Smoke naturally rises and expands
- **Turbulence Effects**: Random wind and air current simulation
- **Dynamic Expansion**: Smoke puffs grow larger and fade over time
- **Realistic Colors**: Multiple shades of gray with transparency

### ⚡ Spark Particle System
- **High-Speed Physics**: Fast-moving particles with air resistance
- **Color Evolution**: Sparks cool from white/yellow to red/orange over time
- **Shape Variety**: Mix of elongated sparks and round particles
- **Flickering Effect**: Dynamic brightness variation for realism

### 🎛️ Advanced Controls
- **Real-time Intensity Control**: Adjust particle emission rate (10-200 particles)
- **Dynamic Size Scaling**: Control particle size (1-10x multiplier)
- **Play/Pause System**: Freeze simulation while maintaining state
- **Performance Monitoring**: Live FPS and particle count display
- **Interactive Emission**: Left-click to emit, right-click for burst effects

## 🚀 Technical Specifications

### Physics Engine
- **60 FPS Animation**: Smooth 16ms update intervals
- **Vector Mathematics**: Proper velocity and acceleration calculations  
- **Collision Detection**: Screen boundary detection and particle cleanup
- **Performance Optimized**: Efficient particle lifecycle management

### Particle Properties
- Position, Velocity, Acceleration vectors
- Life span and aging simulation
- Dynamic visual property updates
- Individual particle physics state

### Visual Effects
- **Gradient Brushes**: Radial gradients for realistic lighting
- **Alpha Blending**: Smooth transparency transitions
- **Dynamic Sizing**: Real-time particle scaling
- **Color Interpolation**: Smooth color transitions over particle lifetime

## 📋 System Requirements

- **OS**: Windows 10/11
- **.NET**: 6.0 or later
- **RAM**: 4GB minimum (8GB recommended for high particle counts)
- **GPU**: Hardware acceleration recommended for smooth performance

## 🛠️ Installation & Setup

### Using Visual Studio 2022

```bash
1. Create new WPF Application project
2. Name: "ParticleSystemApp"
3. Select .NET 6.0 or later
4. Replace generated files with provided code
5. Build and Run (F5)
```

### Using Visual Studio Code

```bash
# Create project
dotnet new wpf -n ParticleSystemApp
cd ParticleSystemApp

# Replace default files with provided code
# Build and run
dotnet build
dotnet run
```

### Project Structure
```
ParticleSystemApp/
├── MainWindow.xaml          # UI Layout and Controls
├── MainWindow.xaml.cs       # Particle System Logic
├── App.xaml                 # Application Definition
├── App.xaml.cs              # Application Entry Point
└── ParticleSystemApp.csproj # Project Configuration
```

## 🎮 How to Use

### Basic Operation
1. **Launch Application** - Start with Water particles selected
2. **Left Click & Drag** - Emit continuous particle stream
3. **Right Click** - Create burst effect with many particles
4. **Select Particle Type** - Choose Water, Smoke, or Sparks
5. **Adjust Controls** - Use sliders to modify intensity and size

### Advanced Controls
- **Intensity Slider (10-200)**: Controls emission rate and burst size
- **Size Slider (1-10)**: Multiplies base particle size
- **Pause Button**: Freezes simulation for analysis
- **Clear Button**: Removes all active particles

### Performance Tips
- Monitor particle count in status bar
- Use Pause to inspect particle behavior
- Clear particles if FPS drops below 30
- Adjust intensity for your hardware capabilities

## 🔬 Physics Implementation

### Water Particles
```csharp
// Gravity simulation
particle.Acceleration = new Vector(0, 150);
// Initial velocity with randomization  
particle.Velocity = new Vector(cos(angle) * speed, sin(angle) * speed);
// Realistic droplet shapes with reflections
```

### Smoke Particles
```csharp
// Buoyancy (upward force)
particle.Acceleration = new Vector(windX, -30);
// Expansion over time
size
