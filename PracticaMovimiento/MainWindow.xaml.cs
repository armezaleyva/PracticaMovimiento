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

        void actualizar() {
            while (true) {
                Dispatcher.Invoke(
                () => {
                    var tiempoActual = stopwatch.Elapsed;
                    var deltaTime = tiempoActual - tiempoAnterior;

                    if (estadoActual == EstadoJuego.Gameplay) {
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
                    double topGatoActual = Canvas.GetTop(imgGato);
                    Canvas.SetTop(imgGato, topGatoActual - 15);
                }
                if (e.Key == Key.Down || e.Key == Key.S) {
                    double topGatoActual = Canvas.GetTop(imgGato);
                    Canvas.SetTop(imgGato, topGatoActual + 15);
                }
                if (e.Key == Key.Left || e.Key == Key.A) {
                    double leftGatoActual = Canvas.GetLeft(imgGato);
                    Canvas.SetLeft(imgGato, leftGatoActual - 15);
                }
                if (e.Key == Key.Right || e.Key == Key.D) {
                    double leftGatoActual = Canvas.GetLeft(imgGato);
                    Canvas.SetLeft(imgGato, leftGatoActual + 15);
                }
            }
        }
    }
}
