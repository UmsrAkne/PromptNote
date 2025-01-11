using PromptNote.Models;

namespace PromptNoteTests.Models
{
    public class PromptParserTest
    {
        [Test]
        [TestCase("test", new[] { "test", })] // 単一フレーズ。
        [TestCase("t1, t2", new[] { "t1", "t2", })] // 複数のフレーズ。
        [TestCase("t0, (t:1.5)", new[] { "t0", "t", })] // 括弧を含むフレーズを含むケース。
        [TestCase("t0, (t, t1:1.5)", new[] { "t0", "t", "t1", })] // 括弧の中に複数のフレーズを含むケース。
        [TestCase("t0, (t, t1:1.5), (t3, t4:1.5)", new[] { "t0", "t", "t1", "t3", "t4", })] // 複数のフレーズを含む括弧が複数あるケース。
        public void ParseTest_Normal(string input, string[] expectedOutputs)
        {
            var actualOutput = PromptParser.Parse(input).Select(p => p.Phrase);
            CollectionAssert.AreEqual(expectedOutputs, actualOutput);
        }

        [TestCase(@"\(test\)", new[] { @"\(test\)", })] // 単一フレーズ。
        [TestCase(@"\(t1\), \(t2\)", new[] { @"\(t1\)", @"\(t2\)", })] // 複数のフレーズ。
        [TestCase(@"\(t0\), t", new[] { @"\(t0\)", "t", })] // 括弧付きと括弧なしのフレーズ
        [TestCase(@"\(t1\), (t2:0.5), t3", new[] { @"\(t1\)", "t2", "t3", })] // エスケープ・通常括弧・括弧なし
        public void ParseTest_With_Escape(string input, string[] expectedOutputs)
        {
            var actualOutput = PromptParser.Parse(input).Select(p => p.Phrase);
            CollectionAssert.AreEqual(expectedOutputs, actualOutput);
        }

        [TestCase(@"<lora:test:1>", new[] { @"<lora:test:1>", }, 1.0)]
        [TestCase(@"<lora:test:1.5>", new[] { @"<lora:test:1.5>", }, 1.5)]
        public void ParseTest_Lora(string input, string[] expectedOutputs, double expectedStrength)
        {
            var actualOutput = PromptParser.Parse(input).ToList();
            CollectionAssert.AreEqual(expectedOutputs, actualOutput.Select(p => p.Phrase));
            Assert.That(actualOutput.First().Strength, Is.EqualTo(expectedStrength));
        }
    }
}