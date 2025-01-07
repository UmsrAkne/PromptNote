using PromptNote.Models;

namespace PromptNoteTests.Models
{
    [TestFixture]
    public class PromptMergerTest
    {
        [Test]

        // ベースリストと追加リストが同数（要素数１）
        [TestCase( new[] { "a", }, new[] { "b", }, new[] { "b", "a", })]

        // ベースリストと追加リストが同数（要素数２）
        [TestCase( new[] { "a", "b", }, new[] { "c", "d", }, new[] { "c", "a", "d", "b", })]

        // 追加するリストのほうが要素数が多いケース
        [TestCase( new[] { "a", "b", }, new[] { "c", "d", "e", }, new[] { "c", "a", "d", "b", "e", })]

        // 追加するリストのほうが要素数が少ないケース
        [TestCase( new[] { "a", "b", }, new[] { "c", }, new[] { "c", "a", "b", })]

        // ベースリストと追加リストが同数で、一部一致しているケース
        [TestCase( new[] { "a", "b", }, new[] { "a", "c", }, new[] { "a", "c", "b", })]

        // ベースリストと追加リストが同数で、順番が変化しているケース
        [TestCase( new[] { "a", "b", "c", }, new[] { "a", "c", "b",}, new[] { "a", "c", "b", })]
        public void MergePromptsTest_Normal(string[] basePrompts, string[] additionalPrompts, string[] expectedPrompts)
        {
            var bps = basePrompts.Select(p => new Prompt() { Phrase = p, }).ToList();
            var aps = additionalPrompts.Select(p => new Prompt() { Phrase = p, }).ToList();
            var actualOutput = PromptMerger.MergePrompts(bps, aps).Select(p => p.Phrase).ToArray();
            CollectionAssert.AreEqual(expectedPrompts, actualOutput);
        }

        // ベースリストと追加リストが同数（要素数１）
        [TestCase( new[] { "a", }, new[] { "b", }, new[] { true, false, })]

        // ベースリストと追加リストが同数（要素数２）
        [TestCase( new[] { "a", "b", }, new[] { "c", "d", }, new[] { true, false, true, false, })]

        // 追加するリストのほうが要素数が多いケース
        [TestCase( new[] { "a", "b", }, new[] { "c", "d", "e", }, new[] { true, false, true, false, true, })]

        // 追加するリストのほうが要素数が少ないケース
        [TestCase( new[] { "a", "b", }, new[] { "c", }, new[] { true, false, false, })]

        // ベースリストと追加リストが同数で、一部一致しているケース
        [TestCase( new[] { "a", "b", }, new[] { "a", "c", }, new[] { true, true, false, })]

        // ベースリストと追加リストが同数で、順番が変化しているケース
        [TestCase( new[] { "a", "b", "c", }, new[] { "a", "c", "b",}, new[] { true, true, true, })]
        public void MergePromptsTest_Check_OutputFlags(string[] basePrompts, string[] additionalPrompts, bool[] expectedPrompts)
        {
            var bps = basePrompts.Select(p => new Prompt() { Phrase = p, }).ToList();
            var aps = additionalPrompts.Select(p => new Prompt() { Phrase = p, }).ToList();
            var actualOutput = PromptMerger.MergePrompts(bps, aps).Select(p => p.ContainsOutput).ToArray();
            CollectionAssert.AreEqual(expectedPrompts, actualOutput);
        }
    }
}