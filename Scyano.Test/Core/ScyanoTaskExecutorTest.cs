namespace Scyano.Core
{
    using Moq;

    public class ScyanoTaskExecutorTest
    {
        private readonly Mock<IScyanoTokenSourceFactory> tokenSourceFactory;
        private readonly Mock<IScyanoFireAndForgetTask> fireAndForgetTask;
        private readonly ScyanoTaskExecutor testee;

        public ScyanoTaskExecutorTest()
        {
            this.tokenSourceFactory = new Mock<IScyanoTokenSourceFactory>();
            this.fireAndForgetTask = new Mock<IScyanoFireAndForgetTask>();
            this.testee = new ScyanoTaskExecutor(this.tokenSourceFactory.Object, this.fireAndForgetTask.Object);
        }

        //[Fact]
        //public void Start_WhenRestartedAfterTerminate_MustNotThrow()
        //{
        //    this.testee.Start(Mock.Of<IScyanoTask>());
        //    this.testee.Terminate(TimeSpan.FromMilliseconds(2));
        //    var token = new CancellationToken(true);
        //    this.tokenSourceFactory.Setup(x => x.Token).Returns(token);

        //    this.testee.Invoking(x => x.Start(Mock.Of<IScyanoTask>()))
        //        .ShouldNotThrow<Exception>();
        //}

        //[Fact]
        //public void Terminate_WhenTaskRunning_MustCancelCancellationTokenSource()
        //{
        //    this.testee.Start(Mock.Of<IScyanoTask>());

        //    this.testee.Terminate(TimeSpan.FromMilliseconds(2));

        //    this.tokenSourceFactory.Verify(x => x.Cancel(), Times.Once());
        //}

        //[Fact]
        //public void Terminate_WhenTaskAlreadyStopped_MustNotCancelCancellationTokenSource()
        //{
        //    this.testee.Terminate(TimeSpan.FromMilliseconds(2));

        //    this.tokenSourceFactory.Verify(x => x.Cancel(), Times.Never());
        //}

        //[Fact]
        //public void Dispose_MustDisposeCancellationTokenSource()
        //{
        //    this.testee.Dispose();

        //    this.tokenSourceFactory.Verify(x => x.Dispose(), Times.Once());
        //}
    }
}