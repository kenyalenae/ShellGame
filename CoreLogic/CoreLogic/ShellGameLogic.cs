using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLogic
{
    // used with MatchEventArgs, will provide the assist in score for player
    public class ItemEventArgs: EventArgs 
    {
        public int Id = int.MinValue;
    }

    // used to with class above /\ to provide a points (score) for player in the event of a correct match. 
    public class MatchEventArgs : ItemEventArgs
    {
        public int Score;
    }

    // If no match? was it a strike
    public class NoMatchEventArgs : EventArgs
    {
        public bool IsStrike;
    }

    public class ShellGameLogic
    {
        public event EventHandler GameOver;
        public event EventHandler<MatchEventArgs> MatchMade;
        public event EventHandler<NoMatchEventArgs> MatchNotMade;

        public event EventHandler StartTurn;
        public event EventHandler ItemReset;
        public event EventHandler ResetComplete;

        public event EventHandler<ItemEventArgs> CheckingItem;
        public event EventHandler<ItemEventArgs> SelectedItem;


        public Item[] Items;

        private readonly int numberOfItems;

        private int itemLocation;  // This is for which shell (little boxs on the table) is the item (pea) located. 

        private LMRRandom rand;

        private int totalStrikes;  // Used to keep track of how many strikes you currently have in the game 

        private int missedCount;  // Used per round to assist in keeping track is you got something wrong. 

        public int Strikes = 0;  // This will cause you to get less points if you guess wrong. 

        // This is if you create a new game logic class you only need to pass in the number of items 
        public ShellGameLogic(int numberOfItems, int totalStrikes) : this(new NetRandom(), numberOfItems, totalStrikes)
        {

        }

        // Part of the ShellGameLogic that calls methods to genereate a random number, also used in testing should give back the number 2
        public ShellGameLogic(LMRRandom rand, int numberOfItems, int totalStrikes)
        {
            this.rand = rand;
            this.numberOfItems = numberOfItems;
            this.totalStrikes = totalStrikes;
            Items = new Item[numberOfItems];

            CreateItems();

            ResetItems();
        }

        // Will create random numbers for game use
        private void GenerateRandomNumber()
        {
            itemLocation = rand.Next(0, numberOfItems);
        }

        private void CreateItems()
        {
            for (int i=0; i< numberOfItems; i++)
            {
                Items[i] = new Item();
                Items[i].Id = i;
                Items[i].AlreadyChecked = false;
            }
        }

        // This is for making the small table boxes (shells) disappear. 
        private void CloseItems()
        {
            for (int i = 0; i < numberOfItems; i++)
            {
                Items[i].AlreadyChecked = false;
            }
        }

        // for reseting game or round
        public void ResetItems()
        {
            // Raise Event ItemReset
            ItemReset?.Invoke(this, EventArgs.Empty);

            // This checks if you have three or more strikes to end your game 
            if (Strikes >= totalStrikes)
            {
                // Raise Game Over Event
                GameOver?.Invoke(this, EventArgs.Empty);

                Strikes = 0;  // Fresh start, for a new game 
            }

            missedCount = 0;  // In case you missed any will reset this for next round or next game 

            GenerateRandomNumber();

            // Raise Event RaiseComplete
            ResetComplete?.Invoke(this, EventArgs.Empty);
        }

        // Did we already check this item for this turn? find out here
        public bool CheckForItem(int itemId)
        {
            CheckingItem?.Invoke(this, new ItemEventArgs() { Id = itemId });

            if (Items[itemId].Id == itemLocation)
            {
                SelectedItem?.Invoke(this, new ItemEventArgs() { Id = itemId });

                // 1st try = 3 points
                // 2nd try = 2 points
                // 3rd try = 0 points
                int score = (numberOfItems - missedCount);  // By doing number of items minus misses we get the score for the player if 1 make it zero
                if (score == 1)  // if the score is one we need to make it zero instead. 
                {
                    Strikes++;  // This adds a game strike for the player if three...Your out of here!

                    // Raise Event No Match 
                    MatchNotMade?.Invoke(this, new NoMatchEventArgs() { IsStrike = true });
                }
                else  // If a correct match is made take action below to determine score for player 
                {
                    // Raise Match Made Event
                    MatchMade?.Invoke(this, new MatchEventArgs() { Id = Items[itemId].Id, Score = score });
                }

                CloseItems();  // makes the little table boxes disappear

                StartTurn?.Invoke(this, EventArgs.Empty);

                return true;
            }

            // if player guess wrong twice this should add a strike 
            if (!Items[itemId].AlreadyChecked)
            {
                // Raise Event Match Not Made
                MatchNotMade?.Invoke(this, new NoMatchEventArgs() { IsStrike = false });

                missedCount++; 

                Items[itemId].AlreadyChecked = true;
            }

            return false;
        }

    }
}
