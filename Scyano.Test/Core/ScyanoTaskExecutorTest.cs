namespace Scyano.Core
{
    using FluentAssertions;
    using Moq;
    using Xunit;

    public class ScyanoTaskExecutorTest
    {
        private readonly Mock<IScyanoTokenSource> tokenSource;
        private readonly ScyanoTaskExecutor testee;

        public ScyanoTaskExecutorTest()
        {
            this.tokenSource = new Mock<IScyanoTokenSource>();
            this.testee = new ScyanoTaskExecutor(this.tokenSource.Object);
        }

        [Fact]
        public void StartOrResume_MustSetIsRunningToTrue()
        {
            this.testee.StartOrResume();

            this.testee.IsRunning.Should().BeTrue();
        }

        [Fact]
        public void Suspend_MustSetIsRunningToFalse()
        {
            this.testee.Suspend();

            this.testee.IsRunning.Should().BeFalse();
        }

        [Fact]
        public void Dispose_MustCancelTokenSource()
        {
            this.testee.Initialize(Mock.Of<IScyanoTask>());

            this.testee.Dispose();

            this.tokenSource.Verify(x => x.Cancel(), Times.Once());
        }

        [Fact]
        public void Dispose_MustDisposeCancellationTokenSource()
        {
            this.testee.Initialize(Mock.Of<IScyanoTask>());

            this.testee.Dispose();

            this.tokenSource.Verify(x => x.Dispose(), Times.Once());
        }
    }
}