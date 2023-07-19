using EditableTimer.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EditableTimer.Tests
{
    [TestClass]
    public class TimerManagerTests
    {
        private MockRepository _mockRepo;
        private Mock<ILogger> _fakeLogger;
        private Mock<IDateTimeWrapper> _fakeTimeWrapper;
        private Mock<ITaskFactory> _taskFactory;
        private Mock<ITimerExecuter> _fakeExecuter;

        private TimerManager manager;
        private DateTime mockNow = new DateTime(2000, 1, 1, 0, 0 , 0, DateTimeKind.Utc);

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepo = new MockRepository(MockBehavior.Strict);
            _fakeLogger = _mockRepo.Create<ILogger>();
            _fakeTimeWrapper = _mockRepo.Create<IDateTimeWrapper>();
            _taskFactory = _mockRepo.Create<ITaskFactory>();
            _fakeExecuter = _mockRepo.Create<ITimerExecuter>();

            manager = new TimerManager(_fakeLogger.Object);
            manager.DateWrapper = _fakeTimeWrapper.Object;
            manager.TaskFactory = _taskFactory.Object;
        }

        [TestMethod]
        public void RegisterTimer_WhenExecuterNull_ThrowInvalidOperation()
        {
            try
            {
                manager.RegisterTimer(null, TimeSpan.FromSeconds(10));
                Assert.Fail("Should fail with null executer");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentException));
            }
            _mockRepo.VerifyAll();
        }

        [TestMethod]
        public void RegisterTimer_WhenExecuterAlreadyAdded_ThrowException()
        {
            try
            {
                _fakeExecuter
                    .Setup(t => t.Identifier)
                    .Returns(0);
                manager._timers.Add(0, new TimerItem());
                manager.RegisterTimer(_fakeExecuter.Object, TimeSpan.FromSeconds(10));
                Assert.Fail("Should fail with null executer");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(InvalidOperationException));
            }
            _mockRepo.VerifyAll();
        }

        [TestMethod]
        public void RegisterTimer_WhenTimeSpanNegative_ThrowException()
        {
            try
            {
                _fakeExecuter
                    .Setup(t => t.Identifier)
                    .Returns(0);
                manager.RegisterTimer(_fakeExecuter.Object, TimeSpan.FromSeconds(-10));
                Assert.Fail("Should fail with negative value");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(Exception));
                Assert.IsTrue(ex.Message.Contains("Not possible to manage negative periods"));
            }
            _mockRepo.VerifyAll();
        }

        [TestMethod]
        public void RegisterTimer_ValidParams_CreateATask()
        {
            _fakeExecuter
                .Setup(t => t.Identifier)
                .Returns(0);
            _fakeLogger
                .Setup(t => t.Log(It.Is<string>(text => text.Contains(" RegisterTimer "))));
            _fakeTimeWrapper
                .Setup(t => t.UtcNow)
                .Returns(mockNow);
            _taskFactory
                .Setup(t => t.StartNew(It.IsAny<Func<object, It.IsAnyType>>(), It.IsAny<object>()))
                .Returns<Task>(null);
                
            TimeSpan timeSpan = TimeSpan.FromSeconds(10);

            manager.RegisterTimer(_fakeExecuter.Object, timeSpan);

            //manager._timers
            Assert.AreEqual(1, manager._timers.Count);
            TimerItem item = manager._timers[0];
            Assert.AreEqual(_fakeExecuter.Object, item.Executer);
            Assert.IsNotNull(item.Source);
            Assert.IsNotNull(item.WaitUntil);
            _mockRepo.VerifyAll();
        }

        [TestMethod]
        public void UnregisterTimer_NotExistTimer_ThrowException()
        {
            try
            {
                _fakeExecuter
                    .Setup(t => t.Identifier)
                    .Returns(0);

                manager.UnregisterTimer(_fakeExecuter.Object);

                Assert.Fail("Should fail with not found");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(Exception));
            }
            _mockRepo.VerifyAll();
        }

        [TestMethod]
        public void UnregisterTimer_ItemExist_CancelTokenRemoveFromList()
        {
            _fakeExecuter
                .Setup(t => t.Identifier)
                .Returns(0);
            _fakeLogger
                .Setup(t => t.Log(It.Is<string>(text => text.Contains(" UnregisterTimer"))));

            CancellationTokenSource prevSource = new CancellationTokenSource();
            manager._timers.Add(0, new TimerItem() {
                Executer = _fakeExecuter.Object,
                Source = prevSource,
                WaitUntil = mockNow,
            });

            manager.UnregisterTimer(_fakeExecuter.Object);

            Assert.AreEqual(0, manager._timers.Count);
            Assert.IsTrue(prevSource.IsCancellationRequested);
            _mockRepo.VerifyAll();
        }

        [TestMethod]
        public void ChangeWaitTime_NotExistTimer_ThrowException()
        {
            try
            {
                _fakeExecuter
                    .Setup(t => t.Identifier)
                    .Returns(0);

                manager.ChangeWaitTime(_fakeExecuter.Object, TimeSpan.FromSeconds(10));

                Assert.Fail("Should fail with not found");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(Exception));
            }
            _mockRepo.VerifyAll();
        }

        [TestMethod]
        public void ChangeWaitTime_WhenTimeSpanNegative_ThrowException()
        {
            try
            {
                _fakeExecuter
                    .Setup(t => t.Identifier)
                    .Returns(0);
                manager._timers.Add(0, new TimerItem());

                manager.ChangeWaitTime(_fakeExecuter.Object, TimeSpan.FromSeconds(-10));

                Assert.Fail("Should fail with negative value");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(Exception));
                Assert.IsTrue(ex.Message.Contains("Not possible to manage negative periods"));
            }
            _mockRepo.VerifyAll();
        }

        [TestMethod]
        public void ChangeWaitTime_ValidParams_CancelPreviousTokenUpdatesItem()
        {
            _fakeExecuter
                .Setup(t => t.Identifier)
                .Returns(0);
            _fakeLogger
                .Setup(t => t.Log(It.Is<string>(text => text.Contains(" ChangeWaitTime 10s"))));
            _fakeTimeWrapper
                .Setup(t => t.UtcNow)
                .Returns(mockNow);
            _taskFactory
                .Setup(t => t.StartNew(It.IsAny<Func<object, It.IsAnyType>>(), It.IsAny<object>()))
                .Returns<Task>(null);
            CancellationTokenSource prevSource = new CancellationTokenSource();
            manager._timers.Add(0, new TimerItem()
            {
                Executer = _fakeExecuter.Object,
                Source = prevSource,
                WaitUntil = mockNow,
            });


            manager.ChangeWaitTime(_fakeExecuter.Object, TimeSpan.FromSeconds(10));

            //manager._timers
            Assert.AreEqual(1, manager._timers.Count);
            TimerItem item = manager._timers[0];
            Assert.AreEqual(_fakeExecuter.Object, item.Executer);
            Assert.IsTrue(prevSource.IsCancellationRequested);
            Assert.IsNotNull(item.WaitUntil);
            Assert.AreNotEqual(item.Source, prevSource);
            _mockRepo.VerifyAll();
        }

    }
}