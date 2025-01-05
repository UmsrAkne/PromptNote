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
        public void MergePromptsTest_Normal(string[] basePrompts, string[] additionalPrompts, string[] expectedPrompts)
        {
            var bps = basePrompts.Select(p => new Prompt() { Phrase = p, }).ToList();
            var aps = additionalPrompts.Select(p => new Prompt() { Phrase = p, }).ToList();
            var actualOutput = PromptMerger.MergePrompts(bps, aps).Select(p => p.Phrase).ToArray();
            CollectionAssert.AreEqual(expectedPrompts, actualOutput);
        }
    }
}