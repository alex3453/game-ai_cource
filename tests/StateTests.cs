using System;
using NUnit.Framework;

namespace bot
{
    [TestFixture]
    public class StateTests
    {
        /*
         * Как отлаживать алгоритм:
         *
         * ConsoleReader после каждого хода пишет в отладочный вывод весь ввод, в котором для удобства
         * переводы строк заменены на "|". Получается одна строка, которую удобно скопировать из интерфейса CG
         * и вставить в этот тест. Аналогично поступить с инизиализационными данными, которые вводятся до первого хода.
         *
         * Если в интерфейсе CG видно, как ваш алгоритм делает странный ход, можно быстро скопировать входные данные,
         * вставить в этот тест, и тем самым повторить проблему в контролируемых условиях.
         * Дальше можно отлаживать проблему привычными способами в IDE.     
         */
        [TestCase("5|42 BREW -1 -1 0 0 6 0 0 0 0|72 BREW 0 -1 -1 -1 19 0 0 0 0|68 BREW 0 0 -1 0 12 0 0 0 0|57 BREW 0 0 -1 -1 14 0 0 0 0|61 BREW 0 0 0 -2 16 0 0 0 0|2 2 3 3 0|2 2 3 3 0")]
        public void Solve(string stepInput)
        {
            var reader = new ConsoleReader(stepInput);
            var state = reader.ReadState();
            Console.WriteLine(state);

            var solver = new Solver();
            var move = solver.GetCommand(state, int.MaxValue);
            Console.WriteLine(move);
        }
    }
}