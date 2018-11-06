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

        public MainWindow() {
            InitializeComponent();
            miCanvas.Focus();

            stopwatch = new Stopwatch();
            stopwatch.Start();
            tiempoAnterior = stopwatch.Elapsed;

            ThreadStart threadStart = new ThreadStart(moverEnemigos);
            Thread threadMoverEnemigos = new Thread(threadStart);
            threadMoverEnemigos.Start();
        }

        void moverEnemigos() {
            while(true) {
                Dispatcher.Invoke(
                () => {
                    var tiempoActual = stopwatch.Elapsed;
                    var deltaTime = tiempoActual - tiempoAnterior;

                    double leftCarroActual = Canvas.GetLeft(imgCarro);
                    Canvas.SetLeft(imgCarro, leftCarroActual + 90 * deltaTime.TotalSeconds);
                    if(Canvas.GetLeft(imgCarro) >= 850) {
                        Canvas.SetLeft(imgCarro, -100);
                    }
                    tiempoAnterior = tiempoActual;
                }
                );
            }
        }

        private void miCanvas_KeyDown(object sender, KeyEventArgs e) {
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
