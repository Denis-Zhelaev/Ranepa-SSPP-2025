using System;
using System.Threading;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;
using DnDAlignmentVisualization.Core;
using DnDAlignmentVisualization.Rendering;
using DnDAlignmentVisualization.Input;

namespace DnDAlignmentVisualization
{
    class Program
    {
        private static RenderWindow window;
        private static AlignmentSystem alignmentSystem;
        private static AlignmentRenderer renderer;
        private static ConsoleInputHandler inputHandler;
        private static bool isRunning = true;
        private static CancellationTokenSource cancellationTokenSource;

        static void Main(string[] args)
        {
            cancellationTokenSource = new CancellationTokenSource();

            try
            {
                // Создаем контекстные настройки для OpenGL
                var contextSettings = new ContextSettings
                {
                    DepthBits = 24,
                    StencilBits = 8,
                    AntialiasingLevel = 4,
                    MajorVersion = 3,
                    MinorVersion = 0
                };

                // Полноэкранный режим с настройками контекста
                var videoMode = VideoMode.DesktopMode;
                window = new RenderWindow(videoMode, "DnD Alignment Visualization", Styles.Fullscreen, contextSettings);
                window.SetVerticalSyncEnabled(true);
                window.SetFramerateLimit(60);

                // Обработка событий окна
                window.Closed += OnWindowClosed;
                window.KeyPressed += OnKeyPressed;
                window.Resized += OnWindowResized;

                // Инициализация систем
                alignmentSystem = new AlignmentSystem();
                renderer = new AlignmentRenderer(window);
                inputHandler = new ConsoleInputHandler(alignmentSystem, renderer.GetFRTRenderer());

                // Запуск консольного ввода с токеном отмены
                inputHandler.Start(cancellationTokenSource.Token);

                Console.WriteLine("Приложение запущено. Нажмите ESC в графическом окне для выхода.");

                // Главный игровой цикл
                while (isRunning && window.IsOpen)
                {
                    window.DispatchEvents();
                    alignmentSystem.Update();
                    renderer.Draw(alignmentSystem);

                    // Небольшая задержка для снижения нагрузки на CPU
                    Thread.Sleep(10);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Критическая ошибка: {ex.Message}");
                Console.WriteLine("Нажмите любую клавишу для выхода...");
                Console.ReadKey();
            }
            finally
            {
                // Корректное освобождение ресурсов
                isRunning = false;
                cancellationTokenSource?.Cancel();
                inputHandler?.Stop();
                Thread.Sleep(100); // Даем время для завершения консольного потока
                window?.Close();
                window?.Dispose();
                cancellationTokenSource?.Dispose();

                Console.WriteLine("Приложение завершено.");
            }
        }

        private static void OnWindowClosed(object sender, EventArgs e)
        {
            isRunning = false;
        }

        private static void OnKeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Escape)
            {
                isRunning = false;
            }
        }

        private static void OnWindowResized(object sender, SizeEventArgs e)
        {
            // Обновляем viewport при изменении размера окна
            window.SetView(new View(new FloatRect(0, 0, e.Width, e.Height)));
        }
    }
}