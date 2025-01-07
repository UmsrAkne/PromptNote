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
            var list = new List<Prompt>();
            var tempBases = basePrompts.ToList();
            var tempAdditions = additionalPrompts.ToList();

            var comparer = new PromptComparer();

            // 追加側に存在しないプロンプトは出力に含まれないよう設定する
            foreach (var p in tempBases.Except(tempAdditions, comparer))
            {
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

                if (bp == null || bp.Phrase == ap.Phrase)
                {
                    continue;
                }

                list.Add(bp);
            }

            return list.Where(p => p != null).ToList();
        }
    }

    class PromptComparer : IEqualityComparer<Prompt>
    {
        public bool Equals(Prompt a, Prompt b)
        {
            if (a == null || b == null)
                return false;

            // 比較基準: Id が同じ
            return a.Phrase == b.Phrase;
        }

        public int GetHashCode(Prompt obj)
        {
            return obj.Phrase.GetHashCode();
        }
    }
}