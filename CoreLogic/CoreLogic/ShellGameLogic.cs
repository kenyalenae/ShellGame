using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLogic
{
    // Was Orginally called Class1
    public class ItemEventArgs: EventArgs 
    {
        public int Id = int.MinValue;
    }

    public class MatchEventArgs : ItemEventArgs
    {
        public int Score;
    }

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

        private void CloseItems()
        {
            for (int i = 0; i < numberOfItems; i++)
            {
                Items[i].AlreadyChecked = false;
            }
        }

        public void ResetItems()
        {
            // Raise Event ItemReset
            ItemReset?.Invoke(this, EventArgs.Empty);

            if (Strikes >= totalStrikes)
            {
                // Raise Game Over Event
                GameOver?.Invoke(this, EventArgs.Empty);

                Strikes = 0;
            }

            missedCount = 0;

            GenerateRandomNumber();

            // Raise Event RaiseComplete
            ResetComplete?.Invoke(this, EventArgs.Empty);
        }

        // Did we already check this item? find out here
        public bool CheckForItem(int itemId)
        {
            CheckingItem?.Invoke(this, new ItemEventArgs() { Id = itemId });

            if (Items[itemId].Id == itemLocation)
            {
                SelectedItem?.Invoke(this, new ItemEventArgs() { Id = itemId });

                // 1st try = 3 points
                // 2nd try = 2 points
                // 3rd try = 0 points
                int score = (numberOfItems - missedCount);
                if (score == 1)
                {
                    Strikes++;

                    // Raise Event No Match 
                    MatchNotMade?.Invoke(this, new NoMatchEventArgs() { IsStrike = true });
                }
                else
                {
                    // Raise Match Made Event
                    MatchMade?.Invoke(this, new MatchEventArgs() { Id = Items[itemId].Id, Score = score });
                }

                CloseItems();

                StartTurn?.Invoke(this, EventArgs.Empty);

                return true;
            }

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
