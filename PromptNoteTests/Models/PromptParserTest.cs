using PromptNote.Models;

namespace PromptNoteTests.Models
{
    public class PromptParserTest
    {
        [Test]
        [TestCase("test", new[] { "test", })]
        [TestCase("t1, t2", new[] { "t1", "t2", })]
        public void ParseTest_Normal(string input, string[] expectedOutputs)
        {
            var actualOutput = PromptParser.Parse(input).Select(p => p.Phrase);
            CollectionAssert.AreEqual(expectedOutputs, actualOutput);
        }
    }
}