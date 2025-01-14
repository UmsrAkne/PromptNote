using PromptNote.Models;

namespace PromptNoteTests.Models
{
    [TestFixture]
    public class PromptsFormatterTest
    {
        [Test]
        [TestCase(new[] { "a", "b", }, new[] { 1.0, 1.0, }, "a, b")]
        [TestCase(new[] { "a", "b", }, new[] { 1.0, 0.5, }, "a, (b:0.5)")]
        [TestCase(new[] { "a", "b", "c", }, new[] { 1.0, 0.5, 0.5, }, "a, (b, c:0.5)")]
        [TestCase(new[] { "a", "b", "c", "d", }, new[] { 1.0, 0.5, 0.5, 1.0, }, "a, (b, c:0.5), d")]
        [TestCase(
            new[] { "a", "b", "c", "d", "e", "f", "g", },
            new[] { 1.0, 0.5, 0.5, 1.0, 0.5, 0.5, 1.0, },
            "a, (b, c:0.5), d, (e, f:0.5), g")]
        public void FormatTest(string[] inputs, double[] strengths, string expected)
        {
            var list = inputs.Select((t, i) => new Prompt() { Phrase = new Phrase() { Value = t, }, Strength = strengths[i], }).ToList();
            Assert.That(PromptsFormatter.Format(list), Is.EqualTo(expected));
        }
    }
}