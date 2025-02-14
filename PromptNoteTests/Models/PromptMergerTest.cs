using PromptNote.Models;

namespace PromptNoteTests.Models
{
    [TestFixture]
    public class PromptMergerTest
    {
        [Test]

        // ベースリストと追加リストが同数（要素数１）
        [TestCase(new[] { "a", }, new[] { "b", }, new[] { "b", "a", })]

        // ベースリストと追加リストが同数（要素数２）
        [TestCase(new[] { "a", "b", }, new[] { "c", "d", }, new[] { "c", "a", "d", "b", })]

        // 追加するリストのほうが要素数が多いケース
        [TestCase(new[] { "a", "b", }, new[] { "c", "d", "e", }, new[] { "c", "a", "d", "b", "e", })]

        // 追加するリストのほうが要素数が少ないケース
        [TestCase(new[] { "a", "b", }, new[] { "c", }, new[] { "c", "a", "b", })]

        // ベースリストと追加リストが同数で、一部一致しているケース
        [TestCase(new[] { "a", "b", }, new[] { "a", "c", }, new[] { "a", "c", "b", })]

        // ベースリストと追加リストが同数で、順番が変化しているケース
        [TestCase(new[] { "a", "b", "c", }, new[] { "a", "c", "b", }, new[] { "a", "c", "b", })]
        public void MergePromptsTest_Normal(string[] basePrompts, string[] additionalPrompts, string[] expectedPrompts)
        {
            var bps = basePrompts.Select(p => new Prompt() { Phrase = new Phrase(p), }).ToList();
            var aps = additionalPrompts.Select(p => new Prompt() { Phrase = new Phrase(p), }).ToList();
            var actualOutput = PromptMerger.MergePrompts(bps, aps).Select(p => p.Phrase.Value).ToArray();
            CollectionAssert.AreEqual(expectedPrompts, actualOutput);
        }

        // ベースリストと追加リストが同数（要素数１）
        [TestCase(new[] { "a", }, new[] { "b", }, new[] { true, false, })]

        // ベースリストと追加リストが同数（要素数２）
        [TestCase(new[] { "a", "b", }, new[] { "c", "d", }, new[] { true, false, true, false, })]

        // 追加するリストのほうが要素数が多いケース
        [TestCase(new[] { "a", "b", }, new[] { "c", "d", "e", }, new[] { true, false, true, false, true, })]

        // 追加するリストのほうが要素数が少ないケース
        [TestCase(new[] { "a", "b", }, new[] { "c", }, new[] { true, false, false, })]

        // ベースリストと追加リストが同数で、一部一致しているケース
        [TestCase(new[] { "a", "b", }, new[] { "a", "c", }, new[] { true, true, false, })]

        // ベースリストと追加リストが同数で、順番が変化しているケース
        [TestCase(new[] { "a", "b", "c", }, new[] { "a", "c", "b", }, new[] { true, true, true, })]
        public void MergePromptsTest_Check_OutputFlags(string[] basePrompts, string[] additionalPrompts, bool[] expectedPrompts)
        {
            var bps = basePrompts.Select(p => new Prompt() { Phrase = new Phrase(p), }).ToList();
            var aps = additionalPrompts.Select(p => new Prompt() { Phrase = new Phrase(p), }).ToList();
            var actualOutput = PromptMerger.MergePrompts(bps, aps).Select(p => p.ContainsOutput).ToArray();
            CollectionAssert.AreEqual(expectedPrompts, actualOutput);
        }

        [TestCase(new[] { "a", "\r", }, new[] { "b", "\r", }, new[] { "b", "a", "\r\n", })]
        [TestCase(
            new[] { "a", "\r", "c", "\r", },
            new[] { "a", "\r", "c", "d", "\r", },
            new[] { "a", "\r\n", "c", "d", "\r\n", })]

        [TestCase(
                new[] { "a", "\r", "b", "\r", "c", "\r", },
                new[] { "a", "\r", "b", "d", "\r", "e", "\r", },
                new[] { "a", "\r\n", "b", "d", "\r\n", "e", "c", "\r\n", })]

        [TestCase(
            new[] { "a", },
            new[] { "a", "b", "\r", "c", },
            new[] { "a", "b", "\r\n", "c", })]

        [TestCase(
            new[] { "a", "b", "\r", "c", },
            new[] { "a", },
            new[] { "a", "b", "\r\n", "c", })]
        public void MergePromptsTest_ContainsNewLine(string[] basePrompts, string[] additionalPrompts, string[] expectedPrompts)
        {
            var bps = basePrompts.Select(p => new Prompt(p)).ToList();
            var aps = additionalPrompts.Select(p => new Prompt(p)).ToList();
            var actualOutput = PromptMerger.MergePrompts(bps, aps).Select(p => p.ToString()).ToArray();
            CollectionAssert.AreEqual(expectedPrompts, actualOutput);
        }

        [Test]
        public void AreOutputEquivalentTest()
        {
            var bps = new[] { "a", "b", "c", }.Select(p => new Prompt(p)).ToList();
            var aps = new[] { "a", "b", "c", }.Select(p => new Prompt(p)).ToList();
            Assert.That(PromptMerger.AreOutputsEquivalent(bps, aps), Is.True);
        }

        [Test]
        public void AreOutputEquivalentTest_outputFlagChanged()
        {
            var bps = new[] { "a", "bTestB", "d", }.Select(p => new Prompt(p)).ToList();
            var aps = new[] { "a", "aTestA", "d", }.Select(p => new Prompt(p)).ToList();

            bps[1].ContainsOutput = false;
            aps[1].ContainsOutput = false;

            // ２番目の要素のフレーズは違うが、出力されないので、等価と判定されるはず。
            Assert.That(PromptMerger.AreOutputsEquivalent(bps, aps), Is.True);
        }
    }
}