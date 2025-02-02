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

        /// <summary>
        /// リストに入っているプロンプトが Lora 一つだけ。
        /// </summary>
        [Test]
        public void FormatTest_ContainsLora_Single()
        {
            var list = new List<Prompt>()
            {
                new () { Phrase = new Phrase() { Value = "<lora:la>", }, Type = PromptType.Lora, Strength = 0.5, },
            };

            Assert.That(PromptsFormatter.Format(list), Is.EqualTo("<lora:la:0.5>"));
        }

        /// <summary>
        /// リストに入っているプロンプトが Lora とその他の２つ。
        /// </summary>
        [Test]
        public void FormatTest_ContainsLora_double()
        {
            var list = new List<Prompt>()
            {
                new () { Phrase = new Phrase() { Value = "a", }, Type = PromptType.Normal, Strength = 1.0, },
                new () { Phrase = new Phrase() { Value = "<lora:lb>", }, Type = PromptType.Lora, Strength = 0.5, },
            };

            Assert.That(PromptsFormatter.Format(list), Is.EqualTo("a, <lora:lb:0.5>"));
        }

        /// <summary>
        /// リストに入っている Lora が一つ含まれていて、それがその他のプロンプトの間に挟まっている。
        /// </summary>
        [Test]
        public void FormatTest_ContainsLora_Triple()
        {
            var list = new List<Prompt>()
            {
                new () { Phrase = new Phrase() { Value = "a", }, Type = PromptType.Normal, Strength = 1.0, },
                new () { Phrase = new Phrase() { Value = "<lora:lb>", }, Type = PromptType.Lora, Strength = 0.5, },
                new () { Phrase = new Phrase() { Value = "c", }, Type = PromptType.Normal, Strength = 1.0, },
            };

            Assert.That(PromptsFormatter.Format(list), Is.EqualTo("a, <lora:lb:0.5>, c"));
        }

        /// <summary>
        /// リストに入っている Lora が一つ含まれていて、それがグループ強調文のプロンプトの間に挟まっている。
        /// </summary>
        [Test]
        public void FormatTest_ContainsLora_Five()
        {
            var list = new List<Prompt>()
            {
                new () { Phrase = new Phrase() { Value = "a", }, Type = PromptType.Normal, Strength = 1.2, },
                new () { Phrase = new Phrase() { Value = "b", }, Type = PromptType.Normal, Strength = 1.2, },
                new () { Phrase = new Phrase() { Value = "<lora:lc>", }, Type = PromptType.Lora, Strength = 0.5, },
                new () { Phrase = new Phrase() { Value = "d", }, Type = PromptType.Normal, Strength = 1.2, },
                new () { Phrase = new Phrase() { Value = "e", }, Type = PromptType.Normal, Strength = 1.2, },
            };

            Assert.That(PromptsFormatter.Format(list), Is.EqualTo("(a, b:1.2), <lora:lc:0.5>, (d, e:1.2)"));
        }

        [Test]
        public void FormatTest_MixedType()
        {
            var list = new List<Prompt>()
            {
                new ("a"),
                new ("\r\n"), // LineBreak
                new ("b"),
                new (" "), // Empty
                new ("<lora:lc>"), // lora
                new ("_"), // Empty
                new ("d"),
            };

            var actual = PromptsFormatter.Format(list);
            Assert.That(actual, Is.EqualTo("a, \r\nb, , <lora:lc:1>, , d"));
        }
    }
}