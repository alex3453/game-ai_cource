namespace bot
{
    public static class StateReader
    {
        public static State ReadState(this ConsoleReader console)
        {
            var actionCount = int.Parse(console.ReadLine()); // the number of spells and recipes in play
            var actions = new GameAction[actionCount];
        
            for (int i = 0; i < actionCount; i++)
            {
                var inputs = console.ReadLine().Split(' ');
                actions[i] = ReadGameAction(inputs);
            }
        
            var myInputs = console.ReadLine().Split(' ');
            var me = ReadPlayer(myInputs);
        
            var enemyInputs = console.ReadLine().Split(' ');
            var enemy = ReadPlayer(enemyInputs);
        
            return new State(me, enemy, actions);
        }

        private static Player ReadPlayer(string[] inputs)
        {
            var inv0 = int.Parse(inputs[0]);
            var inv1 = int.Parse(inputs[1]);
            var inv2 = int.Parse(inputs[2]);
            var inv3 = int.Parse(inputs[3]);
            var inventory = new IngredientsVolume(inv0, inv1, inv2, inv3);

            var score = int.Parse(inputs[4]); // amount of rupees

            return new Player(inventory, score);
        }

        private static GameAction ReadGameAction(string[] inputs)
        {
            var actionId = int.Parse(inputs[0]); // the unique ID of this spell or recipe

            var actionTypeStr = inputs[1];
            var actionType = actionTypeStr == "BREW" ? GameActionType.Brew 
                : GameActionType.Brew; // in the first league: BREW; later: CAST, OPPONENT_CAST, LEARN, BREW

            var delta0 = int.Parse(inputs[2]); // tier-0 ingredient change
            var delta1 = int.Parse(inputs[3]); // tier-1 ingredient change
            var delta2 = int.Parse(inputs[4]); // tier-2 ingredient change
            var delta3 = int.Parse(inputs[5]); // tier-3 ingredient change
            var delta = new IngredientsVolume(delta0, delta1, delta2, delta3);

            var price = int.Parse(inputs[6]); // the price in rupees if this is a potion

            var
                tomeIndex = int.Parse(
                    inputs[7]); // in the first two leagues: always 0; later: the index in the tome if this is a tome spell, equal to the read-ahead tax; For brews, this is the value of the current urgency bonus
            var
                taxCount = int.Parse(
                    inputs[8]); // in the first two leagues: always 0; later: the amount of taxed tier-0 ingredients you gain from learning this spell; For brews, this is how many times you can still gain an urgency bonus
            var castable = inputs[9] != "0"; // in the first league: always 0; later: 1 if this is a castable player spell
            var repeatable =
                inputs[10] != "0"; // for the first two leagues: always 0; later: 1 if this is a repeatable player spell

            return new GameAction(actionId, actionType, delta, price, tomeIndex, taxCount, castable, repeatable);
        }
    }
}