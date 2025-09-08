using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Diagnostics;

namespace ParticleSystemApp
{
    public partial class MainWindow : Window
    {
        private ParticleSystemType currentSystem = ParticleSystemType.Water;
        private List<Particle> particles = new List<Particle>();
        private DispatcherTimer animationTimer;
        private Random random = new Random();
        private bool isEmitting = false;
        private bool isPaused = false;
        private Point lastMousePosition;
        private Stopwatch fpsStopwatch = new Stopwatch();
        private int frameCount = 0;

        public enum ParticleSystemType
        {
            Water,
            Smoke,
            Sparks
        }

        public MainWindow()
        {
            InitializeComponent();
            InitializeParticleSystem();
            UpdateButtonStates();
            fpsStopwatch.Start();
        }

        private void InitializeParticleSystem()
        {
            animationTimer = new DispatcherTimer();
            animationTimer.Interval = TimeSpan.FromMilliseconds(16); // ~60 FPS
            animationTimer.Tick += AnimationTimer_Tick;
            animationTimer.Start();
        }

        #region Button Event Handlers

        private void WaterButton_Click(object sender, RoutedEventArgs e)
        {
            currentSystem = ParticleSystemType.Water;
            UpdateButtonStates();
            StatusText.Text = "Water particles selected - Creates flowing droplets affected by gravity";
        }

        private void SmokeButton_Click(object sender, RoutedEventArgs e)
        {
            currentSystem = ParticleSystemType.Smoke;
            UpdateButtonStates();
            StatusText.Text = "Smoke particles selected - Creates rising, dissipating smoke clouds";
        }

        private void SparksButton_Click(object sender, RoutedEventArgs e)
        {
            currentSystem = ParticleSystemType.Sparks;
            UpdateButtonStates();
            StatusText.Text = "Spark particles selected - Creates explosive, bright spark effects";
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            isPaused = !isPaused;
            PlayPauseButton.Content = isPaused ? "▶️ Play" : "⏸️ Pause";
            PlayPauseButton.Background = isPaused ? 
                new SolidColorBrush(Color.FromRgb(39, 174, 96)) : 
                new SolidColorBrush(Color.FromRgb(231, 76, 60));
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            particles.Clear();
            ParticleCanvas.Children.Clear();
            StatusText.Text = "Canvas cleared - Ready to create new particle effects";
        }

        #endregion

        #region Mouse Event Handlers

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isEmitting = true;
            lastMousePosition = e.GetPosition(ParticleCanvas);
            EmitParticles(lastMousePosition, false);
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isEmitting && e.LeftButton == MouseButtonState.Pressed)
            {
                lastMousePosition = e.GetPosition(ParticleCanvas);
                EmitParticles(lastMousePosition, false);
            }
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isEmitting = false;
        }

        private void Canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(ParticleCanvas);
            EmitParticles(position, true); // Burst effect
        }

        #endregion

        private void UpdateButtonStates()
        {
            // Reset all button backgrounds
            WaterButton.Background = new SolidColorBrush(Color.FromRgb(52, 152, 219));
            SmokeButton.Background = new SolidColorBrush(Color.FromRgb(127, 140, 141));
            SparksButton.Background = new SolidColorBrush(Color.FromRgb(243, 156, 18));

            // Highlight selected button
            switch (currentSystem)
            {
                case ParticleSystemType.Water:
                    WaterButton.Background = new SolidColorBrush(Color.FromRgb(41, 128, 185));
                    break;
                case ParticleSystemType.Smoke:
                    SmokeButton.Background = new SolidColorBrush(Color.FromRgb(149, 165, 166));
                    break;
                case ParticleSystemType.Sparks:
                    SparksButton.Background = new SolidColorBrush(Color.FromRgb(230, 126, 34));
                    break;
            }
        }

        private void EmitParticles(Point position, bool isBurst)
        {
            if (isPaused) return;

            int particleCount = isBurst ? (int)(IntensitySlider.Value * 2) : (int)(IntensitySlider.Value / 3);
            
            for (int i = 0; i < particleCount; i++)
            {
                Particle particle = CreateParticle(position, isBurst);
                particles.Add(particle);
                ParticleCanvas.Children.Add(particle.Shape);
            }
        }

        private Particle CreateParticle(Point position, bool isBurst)
        {
            Particle particle = new Particle();
            particle.Position = position;
            particle.Size = SizeSlider.Value;
            particle.Life = 1.0;
            particle.MaxLife = 1.0;

            switch (currentSystem)
            {
                case ParticleSystemType.Water:
                    CreateWaterParticle(particle, isBurst);
                    break;
                case ParticleSystemType.Smoke:
                    CreateSmokeParticle(particle, isBurst);
                    break;
                case ParticleSystemType.Sparks:
                    CreateSparkParticle(particle, isBurst);
                    break;
            }

            return particle;
        }

        private void CreateWaterParticle(Particle particle, bool isBurst)
        {
            // Water properties
            double angle = isBurst ? random.NextDouble() * Math.PI * 2 : (random.NextDouble() - 0.5) * Math.PI * 0.5;
            double speed = isBurst ? 50 + random.NextDouble() * 100 : 20 + random.NextDouble() * 40;
            
            particle.Velocity = new Vector(
                Math.Cos(angle) * speed,
                Math.Sin(angle) * speed - (isBurst ? 20 : 5) // Slight upward initial velocity
            );
            
            particle.Acceleration = new Vector(0, 150); // Gravity
            particle.MaxLife = 2.0 + random.NextDouble() * 3.0;
            particle.Life = particle.MaxLife;

            // Create water droplet shape
            Ellipse droplet = new Ellipse();
            droplet.Width = particle.Size * (0.8 + random.NextDouble() * 0.4);
            droplet.Height = droplet.Width * (1.2 + random.NextDouble() * 0.6); // Slightly elongated

            // Water colors - blues and cyans
            Color[] waterColors = {
                Color.FromArgb(200, 52, 152, 219),   // Blue
                Color.FromArgb(180, 46, 204, 113),   // Cyan
                Color.FromArgb(160, 26, 188, 156),   // Turquoise
                Color.FromArgb(220, 41, 128, 185)    // Dark blue
            };

            Color waterColor = waterColors[random.Next(waterColors.Length)];
            
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.Center = new Point(0.3, 0.3); // Light reflection
            brush.GradientStops.Add(new GradientStop(Colors.White, 0.0));
            brush.GradientStops.Add(new GradientStop(waterColor, 0.7));
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(100, waterColor.R, waterColor.G, waterColor.B), 1.0));

            droplet.Fill = brush;
            particle.Shape = droplet;
        }

        private void CreateSmokeParticle(Particle particle, bool isBurst)
        {
            // Smoke properties
            double angle = isBurst ? random.NextDouble() * Math.PI * 2 : (random.NextDouble() - 0.5) * Math.PI * 0.3;
            double speed = isBurst ? 30 + random.NextDouble() * 60 : 10 + random.NextDouble() * 25;
            
            particle.Velocity = new Vector(
                Math.Cos(angle) * speed,
                -Math.Abs(Math.Sin(angle) * speed) - 20 // Always upward
            );
            
            particle.Acceleration = new Vector(
                (random.NextDouble() - 0.5) * 20, // Random horizontal drift
                -30 // Buoyancy (upward acceleration)
            );
            
            particle.MaxLife = 3.0 + random.NextDouble() * 4.0;
            particle.Life = particle.MaxLife;

            // Create smoke puff
            Ellipse smokePuff = new Ellipse();
            double baseSize = particle.Size * (1.5 + random.NextDouble() * 2.0);
            smokePuff.Width = baseSize;
            smokePuff.Height = baseSize * (0.8 + random.NextDouble() * 0.4);

            // Smoke colors - grays and dark colors
            Color[] smokeColors = {
                Color.FromArgb(80, 44, 62, 80),      // Dark gray
                Color.FromArgb(60, 127, 140, 141),   // Medium gray
                Color.FromArgb(40, 149, 165, 166),   // Light gray
                Color.FromArgb(70, 52, 73, 94)       // Blue-gray
            };

            Color smokeColor = smokeColors[random.Next(smokeColors.Length)];
            
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientStops.Add(new GradientStop(smokeColor, 0.0));
            brush.GradientStops.Add(new GradientStop(Colors.Transparent, 1.0));

            smokePuff.Fill = brush;
            particle.Shape = smokePuff;
        }

        private void CreateSparkParticle(Particle particle, bool isBurst)
        {
            // Spark properties
            double angle = isBurst ? random.NextDouble() * Math.PI * 2 : (random.NextDouble() - 0.5) * Math.PI;
            double speed = isBurst ? 80 + random.NextDouble() * 150 : 40 + random.NextDouble() * 80;
            
            particle.Velocity = new Vector(
                Math.Cos(angle) * speed,
                Math.Sin(angle) * speed
            );
            
            particle.Acceleration = new Vector(0, 80); // Light gravity
            particle.MaxLife = 0.5 + random.NextDouble() * 1.5;
            particle.Life = particle.MaxLife;

            // Create spark - can be rectangle or ellipse
            Shape spark;
            if (random.NextDouble() < 0.7) // 70% chance for elongated sparks
            {
                Rectangle rect = new Rectangle();
                rect.Width = particle.Size * 0.3;
                rect.Height = particle.Size * (2 + random.NextDouble() * 3);
                spark = rect;
            }
            else
            {
                Ellipse ellipse = new Ellipse();
                ellipse.Width = particle.Size;
                ellipse.Height = particle.Size;
                spark = ellipse;
            }

            // Spark colors - bright oranges, yellows, whites
            Color[] sparkColors = {
                Color.FromArgb(255, 255, 255, 255),  // White
                Color.FromArgb(255, 255, 255, 0),    // Yellow
                Color.FromArgb(255, 255, 165, 0),    // Orange
                Color.FromArgb(255, 255, 69, 0),     // Red-Orange
                Color.FromArgb(255, 255, 140, 0)     // Dark Orange
            };

            Color sparkColor = sparkColors[random.Next(sparkColors.Length)];
            
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.Center = new Point(0.5, 0.5);
            brush.GradientStops.Add(new GradientStop(sparkColor, 0.0));
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(0, sparkColor.R, sparkColor.G, sparkColor.B), 1.0));

            spark.Fill = brush;
            particle.Shape = spark;
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            if (isPaused) return;

            double deltaTime = 0.016; // 16ms = 60fps

            // Update particles
            for (int i = particles.Count - 1; i >= 0; i--)
            {
                Particle particle = particles[i];
                UpdateParticle(particle, deltaTime);

                if (particle.Life <= 0)
                {
                    ParticleCanvas.Children.Remove(particle.Shape);
                    particles.RemoveAt(i);
                }
            }

            // Update UI
            ParticleCountText.Text = particles.Count.ToString();
            
            // Update FPS
            frameCount++;
            if (fpsStopwatch.ElapsedMilliseconds >= 1000)
            {
                double fps = frameCount / (fpsStopwatch.ElapsedMilliseconds / 1000.0);
                FpsText.Text = fps.ToString("F0");
                frameCount = 0;
                fpsStopwatch.Restart();
            }
        }

        private void UpdateParticle(Particle particle, double deltaTime)
        {
            // Update physics
            particle.Velocity += particle.Acceleration * deltaTime;
            particle.Position += particle.Velocity * deltaTime;
            particle.Life -= deltaTime;

            // Update visual properties based on particle type
            double lifeRatio = particle.Life / particle.MaxLife;
            
            switch (currentSystem)
            {
                case ParticleSystemType.Water:
                    UpdateWaterParticle(particle, lifeRatio);
                    break;
                case ParticleSystemType.Smoke:
                    UpdateSmokeParticle(particle, lifeRatio);
                    break;
                case ParticleSystemType.Sparks:
                    UpdateSparkParticle(particle, lifeRatio);
                    break;
            }

            // Update position on canvas
            Canvas.SetLeft(particle.Shape, particle.Position.X - particle.Shape.Width / 2);
            Canvas.SetTop(particle.Shape, particle.Position.Y - particle.Shape.Height / 2);

            // Remove particles that go off screen (except smoke which goes up)
            if (particle.Position.X < -50 || particle.Position.X > ParticleCanvas.ActualWidth + 50 ||
                particle.Position.Y > ParticleCanvas.ActualHeight + 50 ||
                (currentSystem != ParticleSystemType.Smoke && particle.Position.Y < -100))
            {
                particle.Life = 0;
            }
        }

        private void UpdateWaterParticle(Particle particle, double lifeRatio)
        {
            // Water particles fade and become more transparent
            particle.Shape.Opacity = lifeRatio * 0.9;
            
            // Add some randomness to movement (wind effect)
            if (random.NextDouble() < 0.1)
            {
                particle.Acceleration = new Vector(
                    particle.Acceleration.X + (random.NextDouble() - 0.5) * 10,
                    150 // Maintain gravity
                );
            }
        }

        private void UpdateSmokeParticle(Particle particle, double lifeRatio)
        {
            // Smoke expands and fades
            double expansion = 1 + (1 - lifeRatio) * 2;
            particle.Shape.Width = particle.Size * expansion;
            particle.Shape.Height = particle.Size * expansion * (0.8 + random.NextDouble() * 0.4);
            particle.Shape.Opacity = lifeRatio * 0.6;
            
            // Add turbulence
            if (random.NextDouble() < 0.2)
            {
                particle.Velocity += new Vector((random.NextDouble() - 0.5) * 20, 0);
            }
        }

        private void UpdateSparkParticle(Particle particle, double lifeRatio)
        {
            // Sparks fade quickly and may flicker
            particle.Shape.Opacity = lifeRatio * (0.7 + random.NextDouble() * 0.3);
            
            // Sparks lose velocity due to air resistance
            particle.Velocity *= 0.98;
            
            // Color shift from white/yellow to red/orange as they cool
            if (lifeRatio < 0.5)
            {
                Color[] coolColors = {
                    Color.FromArgb(255, 255, 69, 0),     // Red-Orange
                    Color.FromArgb(255, 255, 0, 0),      // Red
                    Color.FromArgb(255, 139, 0, 0)       // Dark Red
                };
                
                Color coolColor = coolColors[random.Next(coolColors.Length)];
                RadialGradientBrush brush = new RadialGradientBrush();
                brush.Center = new Point(0.5, 0.5);
                brush.GradientStops.Add(new GradientStop(coolColor, 0.0));
                brush.GradientStops.Add(new GradientStop(Colors.Transparent, 1.0));
                particle.Shape.Fill = brush;
            }
        }
    }

    // Particle class to hold particle data
    public class Particle
    {
        public Point Position { get; set; }
        public Vector Velocity { get; set; }
        public Vector Acceleration { get; set; }
        public double Size { get; set; }
        public double Life { get; set; }
        public double MaxLife { get; set; }
        public Shape Shape { get; set; }
    }
}