using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Helpers
{
    /// <summary>
    /// Для удобной работы с консолью (удобное указание цветов и т. д.)
    /// </summary>
    public static class ConsolePlus
    {
        /// <summary>
        /// Разметка текста
        /// </summary>
        public enum TextAlign
        {
            left,
            center
        }

        /// <summary>
        /// Ширина окна
        /// </summary>
        public static int Weight { get; private set; }

        /// <summary>
        /// Размер шрифта
        /// </summary>
        public static double FontSize { get; private set; } = 0.8;

        /// <summary>
        /// Задать размер окну
        /// </summary>
        /// <param name="weight"></param>
        public static void SetSizeWindow(int weight)
        {
            Weight = weight;
        }

        /// <summary>
        /// Задать размер шрифту
        /// </summary>
        /// <param name="weight"></param>
        public static void SetSizeFont(int weight)
        {
            Weight = weight;
        }

        public static void Write<T>(T message, ConsoleColor foregroundColor = ConsoleColor.White,
            ConsoleColor backkgroundColor = ConsoleColor.Black, TextAlign textAlign = TextAlign.left, bool isLog = true)
        {
            RenderTextCenter(message.ToString().Count(), textAlign);
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backkgroundColor;
            Console.Write(message);
            Console.ResetColor();

            if (isLog)
                Log.Information(message.ToString());
        }

        public static void WriteLine<T>(T message, ConsoleColor foregroundColor = ConsoleColor.White,
            ConsoleColor backkgroundColor = ConsoleColor.Black, TextAlign textAlign = TextAlign.left, bool isLog = true)
        {
            RenderTextCenter(message.ToString().Count(), textAlign);
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backkgroundColor;
            Console.WriteLine(message);
            Console.ResetColor();

            if (isLog)
                Log.Information(message.ToString());
        }

        /// <summary>
        /// Разметка текста по центру
        /// </summary>
        private static void RenderTextCenter(int lengtMessage, TextAlign textAlign)
        {
            switch (textAlign)
            {
                case TextAlign.left:
                    return;
                default:
                    var sizeName = (Weight / 2) - (lengtMessage * FontSize);
                    for (int i = 0; i < sizeName; i++)
                        Console.Write(" ");
                    break;
            }
        }
    }
}
