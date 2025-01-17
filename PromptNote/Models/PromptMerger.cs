using System.Collections.Generic;
using System.Linq;

namespace PromptNote.Models
{
    public static class PromptMerger
    {
        /// <summary>
        /// ２つの Prompt のリストを、カスタムロジックに従ってマージし、新しいリストを返します。
        /// </summary>
        /// <param name="basePrompts">既存のリスト。</param>
        /// <param name="additionalPrompts">既存のリストに情報を加えるための新しいリスト</param>
        /// <returns>引数に入力されたリストをマージしたリスト。</returns>
        public static List<Prompt> MergePrompts(List<Prompt> basePrompts, List<Prompt> additionalPrompts)
        {
            var tempBases = basePrompts.ToList();
            var tempAdditions = additionalPrompts.ToList();
            var comparer = new PromptComparer();

            for (var i = 0; i < tempBases.Count; i++)
            {
                if (tempBases[i].Type == PromptType.LineBreak)
                {
                    continue;
                }

                if (tempAdditions.Contains(tempBases[i], comparer))
                {
                    tempBases[i] = null;
                }
            }

            var baseLines = SplitPrompts(tempBases);
            var additionalLines = SplitPrompts(tempAdditions);

            while (baseLines.Count < additionalLines.Count)
            {
                baseLines.Add(new List<Prompt>());
            }

            var result = new List<Prompt>();
            for (var i = 0; i < baseLines.Count; i++)
            {
                var b = baseLines[i];
                var a = i <= additionalLines.Count - 1 ? additionalLines[i] : new List<Prompt>();

                result.AddRange(Merge(b, a));
            }

            return result;
        }

        private static List<Prompt> Merge(List<Prompt> basePrompts, List<Prompt> additionalPrompts)
        {
            var list = new List<Prompt>();
            var tempBases = basePrompts.ToList();
            var tempAdditions = additionalPrompts.ToList();

            var comparer = new PromptComparer();

            // 追加側に存在しないプロンプトは出力に含まれないよう設定する
            foreach (var p in tempBases.Except(tempAdditions, comparer))
            {
                if (p == null)
                {
                    continue;
                }

                p.ContainsOutput = false;
            }

            for (var i = 0; i < tempBases.Count; i++)
            {
                if (tempAdditions.Contains(tempBases[i], comparer))
                {
                    tempBases[i] = null;
                }
            }

            while (tempAdditions.Count <= tempBases.Count)
            {
                tempAdditions.Add(null);
            }

            for (var i = 0; i < tempAdditions.Count; i++)
            {
                Prompt bp = null;
                if (tempBases.Count > i)
                {
                    bp = tempBases[i];
                }

                var ap = tempAdditions[i];
                list.Add(ap);

                if (ap == null)
                {
                    list.Add(bp);
                    continue;
                }

                if (bp == null || bp.Phrase.Value == ap.Phrase.Value)
                {
                    continue;
                }

                list.Add(bp);
            }

            return list.Where(p => p != null).ToList();
        }

        /// <summary>
        /// 入力されたプロンプトのリストを LineBreak の箇所で分割し、Prompt のリストのリストを生成します。
        /// </summary>
        /// <param name="target">分割するリストを入力します。</param>
        /// <returns>入力されたリストを改行の地点で分割したリスト。改行部分は分割後のリストの末尾に配置されます。</returns>
        private static List<List<Prompt>> SplitPrompts(List<Prompt> target)
        {
            var lists = new List<List<Prompt>>();
            var l = new List<Prompt>();
            foreach (var p in target)
            {
                l.Add(p);
                if (p == null)
                {
                    continue;
                }

                if (p.Type == PromptType.LineBreak)
                {
                    lists.Add(l.ToList());
                    l.Clear();
                }
            }

            if (l.Count != 0)
            {
                lists.Add(l);
            }

            return lists;
        }

        private class PromptComparer : IEqualityComparer<Prompt>
        {
            public bool Equals(Prompt a, Prompt b)
            {
                if (a == null || b == null)
                {
                    return false;
                }

                // 比較基準: Id が同じ
                return a.Phrase.Value == b.Phrase.Value;
            }

            public int GetHashCode(Prompt obj)
            {
                return obj.Phrase.Value.GetHashCode();
            }
        }
    }
}