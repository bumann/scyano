namespace Scyano.Core
{
    using System;
    using Moq;
    using Xunit;

    public class ScyanoTaskExecutorTest
    {
        private readonly Mock<IScyanoTokenSource> cancellationTokenSource;
        private readonly Mock<IScyanoFireAndForgetTask> fireAndForgetTask;
        private readonly ScyanoTaskExecutor testee;

        public ScyanoTaskExecutorTest()
        {
            this.cancellationTokenSource = new Mock<IScyanoTokenSource>();
            this.fireAndForgetTask = new Mock<IScyanoFireAndForgetTask>();
            this.testee = new ScyanoTaskExecutor(this.cancellationTokenSource.Object, this.fireAndForgetTask.Object);
        }

        [Fact]
        public void Terminate_WhenTaskRunning_MustCancelCancellationTokenSource()
        {
            this.testee.Start(Mock.Of<IScyanoTask>());

            this.testee.Terminate(TimeSpan.FromMilliseconds(2));

            this.cancellationTokenSource.Verify(x => x.Cancel(), Times.Once());
        }

        [Fact]
        public void Terminate_WhenTaskAlreadyStopped_MustNotCancelCancellationTokenSource()
        {
            this.testee.Terminate(TimeSpan.FromMilliseconds(2));

            this.cancellationTokenSource.Verify(x => x.Cancel(), Times.Never());
        }

        [Fact]
        public void Dispose_MustDisposeCancellationTokenSource()
        {
            this.testee.Dispose();

            this.cancellationTokenSource.Verify(x => x.Dispose(), Times.Once());
        }
    }
}