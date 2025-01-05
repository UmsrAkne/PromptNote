using PromptNote.Models;

namespace PromptNoteTests.Models
{
    [TestFixture]
    public class PromptMergerTest
    {
        [Test]
        [TestCase( new[] { "a", }, new[] { "b", }, new[] { "b", "a", })]
        [TestCase( new[] { "a", "b", }, new[] { "c", "d", }, new[] { "c", "a", "d", "b", })]
        public void MergePromptsTest_Normal(string[] basePrompts, string[] additionalPrompts, string[] expectedPrompts)
        {
            var bps = basePrompts.Select(p => new Prompt() { Phrase = p, }).ToList();
            var aps = additionalPrompts.Select(p => new Prompt() { Phrase = p, }).ToList();
            var actualOutput = PromptMerger.MergePrompts(bps, aps).Select(p => p.Phrase).ToArray();
            CollectionAssert.AreEqual(expectedPrompts, actualOutput);
        }
    }
}