using System;
using System.Threading;
using System.Threading.Tasks;
using DnDAlignmentVisualization.Core;
using DnDAlignmentVisualization.Rendering;
using SFML.System;

namespace DnDAlignmentVisualization.Input
{
    public class ConsoleInputHandler
    {
        private readonly AlignmentSystem _alignmentSystem;
        private readonly FRTRenderer _frtrRenderer;
        private CancellationTokenSource _internalTokenSource;
        private Task _consoleTask;

        public ConsoleInputHandler(AlignmentSystem alignmentSystem, FRTRenderer frtrRenderer)
        {
            _alignmentSystem = alignmentSystem;
            _frtrRenderer = frtrRenderer;
        }

        public void Start(CancellationToken externalToken = default)
        {
            _internalTokenSource = CancellationTokenSource.CreateLinkedTokenSource(externalToken);
            _consoleTask = Task.Run(ConsoleLoop, _internalTokenSource.Token);
        }

        public void Stop()
        {
            _internalTokenSource?.Cancel();

            try
            {
                _consoleTask?.Wait(500); 
            }
            catch (AggregateException)
            {
            }
        }

        private void ConsoleLoop()
        {
            Console.WriteLine("Консоль визуализации DnD выравнивания");
            Console.WriteLine("Команды: move <x> <y>, clear, exit, show, hide");
            Console.WriteLine();

            while (!_internalTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    Console.Write("> ");
                    string input = Console.ReadLine()?.Trim();

                    if (string.IsNullOrEmpty(input))
                        continue;

                    if (_internalTokenSource.Token.IsCancellationRequested)
                        break;

                    ProcessCommand(input);
                }
                catch (Exception ex)
                {
                    if (!_internalTokenSource.Token.IsCancellationRequested)
                        Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }
        }

        private void ProcessCommand(string command)
        {
            string[] parts = command.Split(' ');

            switch (parts[0].ToLower())
            {
                case "move":
                    if (parts.Length >= 3 && int.TryParse(parts[1], out int x) && int.TryParse(parts[2], out int y))
                    {
                        var result = _alignmentSystem.ProcessMoveCommand(x, y);
                        Console.WriteLine($"Вектор решения (Dv): ({result.Dv.X:F2}, {result.Dv.Y:F2})");
                        Console.WriteLine($"Вектор сопротивления (Rv): ({result.Rv.X:F2}, {result.Rv.Y:F2})");
                        Console.WriteLine($"Вектор памяти (Mv): ({result.Mv.X:F2}, {result.Mv.Y:F2})");

                        Vector2f totalVector = new Vector2f(
                            result.Dv.X + result.Rv.X + result.Mv.X,
                            result.Dv.Y + result.Rv.Y + result.Mv.Y
                        );
                        Console.WriteLine($"Итоговый вектор: ({totalVector.X:F2}, {totalVector.Y:F2})");
                        Console.WriteLine($"Новая позиция: ({result.newPosition.X:F2}, {result.newPosition.Y:F2})");
                    }
                    else
                    {
                        Console.WriteLine("Неверная команда move. Использование: move <x> <y>");
                    }
                    break;

                case "clear":
                    Console.Clear();
                    Console.WriteLine("Консоль визуализации DnD выравнивания");
                    Console.WriteLine("Команды: move <x> <y>, clear, exit, show, hide");
                    Console.WriteLine();
                    break;

                case "exit":
                    Console.WriteLine("Выход из приложения...");
                    Environment.Exit(0);
                    break;

                case "show":
                    _frtrRenderer.ToggleLabels(true);
                    Console.WriteLine("Подписи отображены");
                    break;

                case "hide":
                    _frtrRenderer.ToggleLabels(false);
                    Console.WriteLine("Подписи скрыты");
                    break;

                default:
                    Console.WriteLine("Неизвестная команда. Доступные: move, clear, exit, show, hide");
                    break;
            }
        }
    }
}