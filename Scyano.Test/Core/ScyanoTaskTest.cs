namespace Scyano.Core
{
    using FluentAssertions;
    using Xunit;

    public class ScyanoTaskTest
    {
        [Fact]
        public void Execute_MustCallAction()
        {
            bool called = false;
            var testee = new ScyanoTask(() => called = true);

            testee.Execute();

            called.Should().BeTrue();
        }
    }
}