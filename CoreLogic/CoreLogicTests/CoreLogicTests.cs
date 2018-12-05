using System;
using CoreLogic;  //This calls CoreLogic for use in this class, add the reference by right clicking on references and then under projects find it
using Xunit;  //Xunit and Xunit visual studio library to help us. 
using Moq;  // This makes it easier to set up tests in your program 

namespace CoreLogicTests
{
    public class CoreLogicTests
    {
        private ShellGameLogic CreateCoreLogic()
        {
            Mock<LMRRandom> rand = new Mock<LMRRandom>();  // the creation of random and what class it should use
            rand.Setup(r => r.Next(0, 3)).Returns(2); // Used for testing, the number should be Two otherwise your programs broke yo'
            return new ShellGameLogic(rand.Object, 3, 3);  // returning it 
        }

        // To make sure we have Xunit set up correctly Add "Fact" and a "." to test working correctly
        [Fact]

        // These are used to check if an item is there test
        public void GivenPlayer_SelectsCorrectItemOnFirstTry_ShouldReturnTrue()
        {
            // Arrange 
            ShellGameLogic sut = CreateCoreLogic();

            // Act... This test is checking for the number 2 from line 13 above and should return true. 
            bool result = sut.CheckForItem(2);

            // Assert
            Assert.True(result);
        }

        [Fact]

        public void GivenPlayer_SelectsIncorrectItemOnFirstTry_ShouldReturnFalse()
        {
            // Arrange 
            ShellGameLogic sut = CreateCoreLogic();

            // Act
            bool result = sut.CheckForItem(1);

            // Assert
            Assert.False(result);
        }

        [Fact]

        public void GivenPlayer_SelectsIncorrectItemOnFirstTryAndSecondTry_ShouldReturnFalse()
        {
            // Arrange 
            ShellGameLogic sut = CreateCoreLogic();

            // Act
            bool result = sut.CheckForItem(1);
            if (!result)
            {
                result = sut.CheckForItem(0);
            }

            // Assert
            Assert.False(result);
        }

        [Fact]

        public void GivenPlayer_LosesRound_ShouldHaveStrikeCountAndItShouldBeOne()
        {
            // Arrange 
            ShellGameLogic sut = CreateCoreLogic();

            // Act
            sut.CheckForItem(0);
            sut.CheckForItem(1);
            sut.CheckForItem(2);

            // Assert
            Assert.Equal(1, sut.Strikes);
        }

        [Fact]

        public void GivenPlayer_LosesRound_ShouldHaveAStrikeCountEvenIfReselectingPreviouslySelectedItem()
        {
            // Arrange 
            ShellGameLogic sut = CreateCoreLogic();

            // Act
            sut.CheckForItem(0);
            sut.CheckForItem(1);
            sut.CheckForItem(0);
            sut.CheckForItem(2);

            // Assert
            Assert.Equal(1, sut.Strikes);
        }

        [Fact]

        public void GivenPlayer_StrikesOutThenWinsOnSecondTryAndStrikesOutAgain_ThenTheStrikeCountShouldBeTwo()
        {
            // Arrange 
            ShellGameLogic sut = CreateCoreLogic();

            // Act

            // Strike 1
            sut.CheckForItem(0);
            sut.CheckForItem(1);
            sut.CheckForItem(2);
            sut.ResetItems();

            // Right on 2nd try 
            sut.CheckForItem(1);
            sut.CheckForItem(2);
            sut.ResetItems();

            // Strike 2
            sut.CheckForItem(0);
            sut.CheckForItem(1);
            sut.CheckForItem(2);
            sut.ResetItems();

            // Assert
            Assert.Equal(2, sut.Strikes);
        }

        [Fact]

        public void GivenPlayer_StrikesOut_ThenTheGameShouldBeOver()
        {
            // Arrange 
            bool called = false;
            var sut = CreateCoreLogic();
            sut.GameOver += (object sender, EventArgs e) => called = true;

            // Act

            // Strike 1
            sut.CheckForItem(0);
            sut.CheckForItem(1);
            sut.CheckForItem(2);
            sut.ResetItems();

            // Strike 2
            sut.CheckForItem(1);
            sut.CheckForItem(0);
            sut.CheckForItem(2);
            sut.ResetItems();

            // Strike 3
            sut.CheckForItem(0);
            sut.CheckForItem(1);
            sut.CheckForItem(2);
            sut.ResetItems();

            // Assert
            Assert.True(called);
        }

        [Fact]

        public void GivenPlayer_FindsAMatch_ShouldRaiseMatchFoundEventWithId()
        {
            // Arrange 
            bool idSet = false;
            ShellGameLogic sut = CreateCoreLogic();
            sut.MatchMade += (object sender, MatchEventArgs e) => idSet = e.Id >= 0;

            // Act
            sut.CheckForItem(2);

            // Assert
            Assert.True(idSet);
        }

        [Fact]

        public void GivenPlayer_DoesntFindAMatch_ShouldRaiseMatchNotFoundWithIsStrikeFalse()
        {
            // Arrange 
            bool isStrike = true;
            ShellGameLogic sut = CreateCoreLogic();
            sut.MatchNotMade += (object sender, NoMatchEventArgs e) => isStrike = e.IsStrike;

            // Act
            sut.CheckForItem(0);

            // Assert
            Assert.False(isStrike);
        }

        [Fact]

        public void GivenPlayer_StrikesOut_ShouldRaiseMatchNotFoundWithIsStrikeTrue()
        {
            // Arrange 
            bool isStrike = false;
            ShellGameLogic sut = CreateCoreLogic();
            sut.MatchNotMade += (object sender, NoMatchEventArgs e) => isStrike = e.IsStrike;

            // Act
            sut.CheckForItem(0);
            sut.CheckForItem(1);
            sut.CheckForItem(2);

            // Assert
            Assert.True(isStrike);
        }

        [Fact]

        public void GivenPlayer_ChecksAnItem_StartTurnEventShouldBeRaised()
        {
            // Arrange 
            bool called = false;
            ShellGameLogic sut = CreateCoreLogic();
            sut.StartTurn += (object sender, EventArgs e) => called = true;

            // Act
            sut.CheckForItem(2);

            // Assert
            Assert.True(called);
        }

        [Fact]

        public void GivenPlayer_SelectsItem_ShouldRaiseCheckingItemEvent()
        {
            // Arrange 
            bool called = false;
            var sut = CreateCoreLogic();
            sut.CheckingItem += (object sender, ItemEventArgs e) => called = e.Id >= 0;

            // Act
            sut.CheckForItem(1);

            // Assert
            Assert.True(called);
        }

        [Fact]

        public void GivenPlayer_SelectsRightItem_ShouldRaiseSelectedItemEvent()
        {
            // Arrange 
            bool called = false;
            var sut = CreateCoreLogic();
            sut.SelectedItem += (object sender, ItemEventArgs e) => called = e.Id >= 0;

            // Act
            sut.CheckForItem(2);

            // Assert
            Assert.True(called);
        }

        [Fact]

        public void GiveResetItems_IsCalled_ThenItemResetEventShouldBeRaised()
        {
            // Arrange 
            bool called = false;
            var sut = CreateCoreLogic();
            sut.ItemReset += (object sender, EventArgs e) => called = true;

            // Act
            sut.ResetItems();

            // Assert
            Assert.True(called);
        }

        [Fact]

        public void GiveResetItems_IsCalled_ThenResetCompleteEventShouldBeRaised()
        {
            // Arrange 
            bool called = false;
            var sut = CreateCoreLogic();
            sut.ResetComplete += (object sender, EventArgs e) => called = true;

            // Act
            sut.ResetItems();

            // Assert
            Assert.True(called);
        }
    }
}
