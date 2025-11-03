// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.MemoryPools;

#endregion

namespace FortitudeTests.FortitudeCommon.DataStructures.Memory;

[TestClass]
public class GarbageAndLockFreePooledFactoryTests
{
    private const int MaxNumberThreads       = 8;
    private const int BorrowReturnIterations = 1_000;

    private readonly LendItemTrackingTimes[] itemTracking = new LendItemTrackingTimes[MaxNumberThreads + 1];

    private readonly Random   randomNumberGen = new();
    private readonly Thread[] workerThreads   = new Thread[MaxNumberThreads];

    private volatile bool allGood;

    private GarbageAndLockFreePooledFactory<LendableItem> garbageAndLockFreePooledFactory = new(() => new LendableItem());

    [TestInitialize]
    public void SetUp()
    {
        LendableItem.ResetInstanceCounter();
        allGood                         = true;
        garbageAndLockFreePooledFactory = new GarbageAndLockFreePooledFactory<LendableItem>(() => new LendableItem());
        for (var i = 0; i < MaxNumberThreads; i++)
        {
            var workerThread = new Thread(RunExercise) { Name = "WorkerThread_" + i };
            workerThreads[i] = workerThread;
        }

        for (var i = 0; i < itemTracking.Length; i++) itemTracking[i] = new LendItemTrackingTimes();
    }

    [TestMethod]
    public void MultipleElementsEnquedRemovingSomeElementsStillRetainsExistingItems()
    {
        var pool = new GarbageAndLockFreePooledFactory<string>(() => throw new ArgumentException());

        var firstString  = "firstString";
        var secondString = "secondString";
        var thirdString  = "thirdString";
        var fourthString = "fourthString";
        var fifthString  = "fifthString";
        var sixthString  = "sixthString";

        var allStrings = new List<string>
            { firstString, secondString, thirdString, fourthString, fifthString, sixthString };

        pool.ReturnBorrowed(firstString);
        pool.ReturnBorrowed(secondString);
        pool.ReturnBorrowed(thirdString);
        pool.ReturnBorrowed(fourthString);
        pool.ReturnBorrowed(fifthString);
        pool.ReturnBorrowed(sixthString);

        foreach (var queuedString in pool) Assert.IsTrue(allStrings.Contains(queuedString));

        pool.Remove(fourthString);

        foreach (var queuedString in pool) Assert.AreNotSame(fourthString, queuedString);

        pool.Remove(sixthString);

        foreach (var queuedString in pool) Assert.AreNotSame(sixthString, queuedString);

        pool.Remove(firstString);

        foreach (var queuedString in pool) Assert.AreNotSame(firstString, queuedString);
    }

    [TestMethod]
    public void MultipleReadersAndWritersEnqueingNoElementIsDoubleBooked()
    {
        for (var i = 0; i < MaxNumberThreads; i++) workerThreads[i].Start();
        for (var i = 0; i < MaxNumberThreads; i++) workerThreads[i].Join();
        Thread.Sleep(50);

        Assert.IsTrue(allGood);

        var totalBorrowAndReturns = 0;
        for (var i = 0; i < itemTracking.Length; i++)
        {
            var lendItemTrackingTimes = itemTracking[i];
            var maxNumberOfSlots      = lendItemTrackingTimes.SlotCounter;
            for (var j = 0; j < maxNumberOfSlots; j++)
            {
                totalBorrowAndReturns++;
                var currentLendTime = lendItemTrackingTimes.TrackTime[j];
                for (var k = j - 1; k > 0 && k > j - MaxNumberThreads * 2; k--)
                {
                    var olderLendTime = lendItemTrackingTimes.TrackTime[k];
                    Assert.IsTrue(currentLendTime.BorrowTime > olderLendTime.ReturnTime,
                                  $"itemTracking[{i}].TrackTime[{j}].ReturnTime = {currentLendTime.BorrowTime:O} < " +
                                  $"itemTracking[{i}].TrackTime[{k}].BorrowTime = {olderLendTime.ReturnTime:O}");
                }

                for (var k = j + 1; k < maxNumberOfSlots && k < j + MaxNumberThreads * 2; k++)
                {
                    var newLendTime = lendItemTrackingTimes.TrackTime[k];
                    Assert.IsTrue(currentLendTime.ReturnTime < newLendTime.BorrowTime,
                                  $"itemTracking[{i}].TrackTime[{j}].ReturnTime = {currentLendTime.ReturnTime:O} > " +
                                  $"itemTracking[{i}].TrackTime[{k}].BorrowTime = {newLendTime.BorrowTime:O}");
                }
            }
        }

        Assert.AreEqual(BorrowReturnIterations * MaxNumberThreads, totalBorrowAndReturns);
    }

    private void RunExercise()
    {
        var i = 0;
        try
        {
            for (i = 0; i < BorrowReturnIterations; i++)
            {
                if (i % 100 == 0) Console.Out.WriteLine($"Thread.Name = \"{Thread.CurrentThread.Name}\" at stage {i}");
                var lendableItem = garbageAndLockFreePooledFactory.Borrow();
                lendableItem.AssertAndStampThreadId();
                var borrowTime = TimeContext.LocalTimeNow;
                if (lendableItem.InstanceNumber < itemTracking.Length)
                {
                    var borrowSlot = itemTracking[lendableItem.InstanceNumber].TrackBorrowTime(borrowTime);

                    var numOfSpins = randomNumberGen.Next(0, 800);
                    // ReSharper disable once EmptyForStatement
                    for (var j = 0; j < numOfSpins; j++)
                    {
                        // spin baby spin
                    }

                    lendableItem.ClearThreadId();
                    var returnTime = TimeContext.LocalTimeNow;
                    garbageAndLockFreePooledFactory.ReturnBorrowed(lendableItem);
                    itemTracking[lendableItem.InstanceNumber].SetReturnTime(borrowSlot, returnTime);
                } // don't return items not being tracked
            }
        }
        catch (Exception ex)
        {
            Console.Out.WriteLine($"{Thread.CurrentThread.Name} caught exception " + ex);
            allGood = false;
        }

        if (BorrowReturnIterations != i)
        {
            Console.Out.WriteLine($"{Thread.CurrentThread.Name} did not complete all iterations only did " + i);
            allGood = false;
        }
    }

    [TestMethod]
    public void SingleThreadAskingForMultipleItemsAndReturnsMultipleItems()
    {
        RunExercise();
    }

    private class LendableItem
    {
        private static   int allInstanceCounter;
        private volatile int threadId;

        public int InstanceNumber { get; } = Interlocked.Increment(ref allInstanceCounter);

        public void AssertAndStampThreadId()
        {
            Assert.AreEqual(0, threadId);
            threadId = Thread.CurrentThread.ManagedThreadId;
        }

        public void ClearThreadId()
        {
            threadId = 0;
        }

        internal static void ResetInstanceCounter()
        {
            allInstanceCounter = 0;
        }
    }

    private struct LendTimes
    {
        public LendTimes(DateTime borrowTime) : this() => BorrowTime = borrowTime;

        public readonly DateTime BorrowTime;

        public DateTime ReturnTime;
    }

    private class LendItemTrackingTimes
    {
        public readonly LendTimes[] TrackTime = new LendTimes[BorrowReturnIterations * MaxNumberThreads];

        private int nextSlotCounter;
        public  int SlotCounter => nextSlotCounter;

        public int TrackBorrowTime(DateTime borrowTime)
        {
            var slotNum = Interlocked.Increment(ref nextSlotCounter);
            TrackTime[slotNum] = new LendTimes(borrowTime);
            return slotNum;
        }

        public void SetReturnTime(int slotNum, DateTime returnTime)
        {
            var previousEntry = TrackTime[slotNum];
            previousEntry.ReturnTime = returnTime;
            TrackTime[slotNum]       = previousEntry;
        }
    }
}
