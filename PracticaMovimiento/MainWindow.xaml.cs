using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Diagnostics;

namespace PracticaMovimiento {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        Stopwatch stopwatch;
        TimeSpan tiempoAnterior;

        enum EstadoJuego { Gameplay, Gameover };
        EstadoJuego estadoActual = EstadoJuego.Gameplay;

        enum Direccion { Arriba, Abajo, Izquierda, Derecha, Ninguna };
        Direccion direccionJugador = Direccion.Ninguna;

        double velocidadRana = 80;

        public MainWindow() {
            InitializeComponent();
            miCanvas.Focus();

            stopwatch = new Stopwatch();
            stopwatch.Start();
            tiempoAnterior = stopwatch.Elapsed;

            ThreadStart threadStart = new ThreadStart(actualizar);
            Thread threadMoverEnemigos = new Thread(threadStart);
            threadMoverEnemigos.Start();
        }

        void moverJugador(TimeSpan deltaTime) {
            double topGatoActual = Canvas.GetTop(imgGato);
            double leftGatoActual = Canvas.GetLeft(imgGato);
            switch (direccionJugador) {
                case Direccion.Arriba:
                    Canvas.SetTop(imgGato, topGatoActual - (velocidadRana * deltaTime.TotalSeconds));
                    break;
                case Direccion.Abajo:
                    Canvas.SetTop(imgGato, topGatoActual + (velocidadRana * deltaTime.TotalSeconds));
                    break;
                case Direccion.Izquierda:
                    if (leftGatoActual - velocidadRana * deltaTime.TotalSeconds >= 0) {
                        Canvas.SetLeft(imgGato, leftGatoActual - (velocidadRana * deltaTime.TotalSeconds));
                    }
                    break;
                case Direccion.Derecha:
                    double nuevaPosicion = leftGatoActual + velocidadRana * deltaTime.TotalSeconds;
                    if (nuevaPosicion + imgGato.Width <= 800) {
                        Canvas.SetLeft(imgGato, nuevaPosicion);
                    }
                    break;
                case Direccion.Ninguna:
                    break;
            }
        }

        void actualizar() {
            while (true) {
                Dispatcher.Invoke(
                () => {
                    var tiempoActual = stopwatch.Elapsed;
                    var deltaTime = tiempoActual - tiempoAnterior;

                    if (estadoActual == EstadoJuego.Gameplay) {
                        moverJugador(deltaTime);
                        double leftCarroActual = Canvas.GetLeft(imgCarro);
                        Canvas.SetLeft(imgCarro, leftCarroActual + 120 * deltaTime.TotalSeconds);
                        if (Canvas.GetLeft(imgCarro) >= 850) {
                            Canvas.SetLeft(imgCarro, -100);
                        }

                        // Intersección en X
                        double xCarro = Canvas.GetLeft(imgCarro);
                        double xGato = Canvas.GetLeft(imgGato);
                        if (xGato + imgGato.Width >= xCarro && xGato <= xCarro + imgCarro.Width) {
                            lblInterseccionX.Text = "SI HAY INTERSECCIÓN EN X!!!";
                        }
                        else {
                            lblInterseccionX.Text = "No hay intersección en X";
                        }

                        // Intersección en Y
                        double yCarro = Canvas.GetTop(imgCarro);
                        double yGato = Canvas.GetTop(imgGato);
                        if (yGato + imgGato.Height >= yCarro && yGato <= yCarro + imgCarro.Height) {
                            lblInterseccionY.Text = "SI HAY INTERSECCIÓN EN Y!!!";
                        }
                        else {
                            lblInterseccionY.Text = "No hay intersección en Y";
                        }

                        if ((xGato + imgGato.Width >= xCarro && xGato <= xCarro + imgCarro.Width) &&
                                (yGato + imgGato.Height >= yCarro && yGato <= yCarro + imgCarro.Height)) {
                            lblColision.Text = "HAY COLISIÓN!!!!";
                            estadoActual = EstadoJuego.Gameover;
                        }
                        else {
                            lblColision.Text = "No hay colisión";
                        }

                        tiempoAnterior = tiempoActual;
                    }
                    else if (estadoActual == EstadoJuego.Gameover) {
                        canvasGameOver.Visibility = Visibility.Visible;
                        miCanvas.Visibility = Visibility.Collapsed;
                    }
                });
                    
            }
        }

        private void miCanvas_KeyDown(object sender, KeyEventArgs e) {
            if (estadoActual == EstadoJuego.Gameplay) {
                if (e.Key == Key.Up || e.Key == Key.W) {
                    direccionJugador = Direccion.Arriba;
                }
                if (e.Key == Key.Down || e.Key == Key.S) {
                    direccionJugador = Direccion.Abajo;
                }
                if (e.Key == Key.Left || e.Key == Key.A) {
                    direccionJugador = Direccion.Izquierda;
                }
                if (e.Key == Key.Right || e.Key == Key.D) {
                    direccionJugador = Direccion.Derecha;
                }
            }
        }

        private void miCanvas_KeyUp(object sender, KeyEventArgs e) {
            if (estadoActual == EstadoJuego.Gameplay) {
                if (e.Key == Key.Up && direccionJugador == Direccion.Arriba) {
                    direccionJugador = Direccion.Ninguna;
                }
                if (e.Key == Key.Down && direccionJugador == Direccion.Abajo) {
                    direccionJugador = Direccion.Ninguna;
                }
                if (e.Key == Key.Left && direccionJugador == Direccion.Izquierda) {
                    direccionJugador = Direccion.Ninguna;
                }
                if (e.Key == Key.Right && direccionJugador == Direccion.Derecha) {
                    direccionJugador = Direccion.Ninguna;
                }
            }
        }
    }
}
