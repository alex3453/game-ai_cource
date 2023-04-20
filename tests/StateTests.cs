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
        [TestCase("13|46 BREW -2 -3 0 0 8 0 0 0 0|64 BREW 0 0 -2 -3 18 0 0 0 0|43 BREW -3 -2 0 0 7 0 0 0 0|59 BREW -2 0 0 -3 14 0 0 0 0|61 BREW 0 0 0 -4 16 0 0 0 0|78 CAST 2 0 0 0 0 -1 -1 0 0|79 CAST -1 1 0 0 0 -1 -1 1 0|80 CAST 0 -1 1 0 0 -1 -1 1 0|81 CAST 0 0 -1 1 0 -1 -1 1 0|82 OPPONENT_CAST 2 0 0 0 0 -1 -1 0 0|83 OPPONENT_CAST -1 1 0 0 0 -1 -1 1 0|84 OPPONENT_CAST 0 -1 1 0 0 -1 -1 1 0|85 OPPONENT_CAST 0 0 -1 1 0 -1 -1 1 0|5 0 0 0 0|5 0 0 0 0")]
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