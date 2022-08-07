using CeelySharp.ListMonad;
using CeelySharp.MaybeMonad;
using CeelySharp.TaskMonad;
using NUnit.Framework;

namespace CeelySharp.Test;

[TestFixture]
public class UnitTest // heh
{
    [Test]
    public void ListUnit()
    {
        CollectionAssert.AreEqual(new[] { 1 }, MList.Unit(1));
    }

    [Test]
    public void MaybeUnit()
    {
        Assert.AreEqual(new Some<int>(1), Maybe.Unit(1));
    }

    [Test]
    public void TaskUnit()
    {
        Assert.AreEqual(1, MTask.Unit(1).Result);
    }
}
