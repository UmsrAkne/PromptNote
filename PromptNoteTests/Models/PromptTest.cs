using PromptNote.Models;

namespace PromptNoteTests.Models
{
    [TestFixture]
    public class PromptTest
    {
        [Test]
        [TestCase("test", PromptType.Normal, "test")]
        [TestCase("<lora:test>", PromptType.Lora, "<lora:test:1>")]
        public void ToStringTest(string phrase, PromptType type, string expected)
        {
            var actual = new Prompt() { Phrase = new Phrase() { Value = phrase, }, Type = type, }.ToString();
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("test", PromptType.Normal)]
        [TestCase("<lora:test>", PromptType.Lora)]
        [TestCase("{p1|p2}", PromptType.DynamicPrompt)]
        [TestCase("{p1|p2} p3", PromptType.DynamicPrompt)]
        public void TypeDetectionTest(string phrase, PromptType expectedType)
        {
            var actual = new Prompt(phrase).Type;
            Assert.That(actual, Is.EqualTo(expectedType));
        }
    }
}