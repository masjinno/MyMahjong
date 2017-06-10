using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMahjong
{
    /// <summary>
    /// 麻雀ロジックに関するユーティリティ
    /// </summary>
    public class MahjongLogicUtility
    {
        /// <summary>
        /// 聴牌判定
        /// </summary>
        /// <param name="concealedTiles">門前手</param>
        /// <param name="openedSets">鳴いた面子</param>
        /// <returns>聴牌しているか  true:聴牌  false:不聴</returns>
        public static bool IsWaitingHand(Tile[] concealedTiles, TileSet[] openedSets)
        {
            Tile drawnTile = new Tile();
            foreach (Tile.Kinds k in Enum.GetValues(typeof(Tile.Kinds)))
            {
                drawnTile.Kind = k;

                /// 1枚追加して和了となるか
                if (IsWonHand(concealedTiles, openedSets, drawnTile))
                {
                    /// 和了形が見つかったのでtrueを返す
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 和了判定
        /// </summary>
        /// <param name="concealedTiles">門前手</param>
        /// <param name="openedSets">鳴いた面子</param>
        /// <param name="drawnTile">ツモ牌</param>
        /// <returns>和了か  true:和了  false:和了でない</returns>
        public static bool IsWonHand(Tile[] concealedTiles, TileSet[] openedSets, Tile drawnTile)
        {
            /// 確定面子の妥当性チェック
            foreach (TileSet ts in openedSets)
            {
                /// 確定面子が対子はありえない
                if (ts.Kind == TileSet.Kinds.Pair)
                {
                    throw new ArgumentOutOfRangeException(nameof(openedSets), string.Format("Argument {0} must not be Pair.", nameof(openedSets)));
                }
            }

            /// 牌数チェック
            int numTiles = MahjongLogicUtility.CountEntityElements(concealedTiles) + MahjongLogicUtility.CountEntityElements(openedSets) * 3;
            if (drawnTile != null)
            {
                numTiles++;
            }
            if (numTiles != 14)
            {
                throw new ArgumentException("The number of tiles is invalid.");
            }

            Tile[] hands = new Tile[concealedTiles.Count() + 1];
            concealedTiles.CopyTo(hands, 0);
            hands[concealedTiles.Count()] = drawnTile;

            /// 手牌をソート
            Array.Sort(hands, (a, b) => a.Index - b.Index);

            /// 国士無双チェック
            if (MahjongLogicUtility.IsThirteenOrphans(hands))
            {
                return true;
            }

            /// 七対子チェック
            if (MahjongLogicUtility.IsSevenPairs(hands))
            {
                return true;
            }

            /// 一般形(4面子+1雀頭)ができているかチェック

            /// 面子配列の妥当性チェック
            foreach (TileSet ts in openedSets)
            {
                if (!ts.IsValidTileSet())
                {
                    throw new InvalidOperationException("Tile set is invalid.");
                }
            }
            
            /// 面前の手配に対する妥当性チェック
            
            /// 手配探索時に面子構成判定としたかを記憶する配列
            bool[] isUsed = new bool[hands.Count()];
            /// 探索した面子構成
            List<TileSet> tempTileSets = new List<TileSet>();
            for (int pairTileIndex = 0; pairTileIndex < hands.Count() - 1; pairTileIndex++)
            {
                /// 面子構成判定配列の初期化
                for (int i = 0; i < isUsed.Count(); i++) isUsed[i] = false;
                tempTileSets.Clear();

                /// 雀頭でなければ、次の面子判定に移行
                if (!MahjongLogicUtility.IsPair(hands, ref isUsed, ref tempTileSets, pairTileIndex))
                {
                    continue;
                }

                /// 面子判定
                for (int tileSetCheckIndex = 0; tileSetCheckIndex + 2 < hands.Count(); tileSetCheckIndex++)
                {
                    /// 面子判定済みなら、面子判定から除外
                    if (isUsed[tileSetCheckIndex])
                    {
                        continue;
                    }

                    /// 刻子 & 順子チェック
                    TileSet.Kinds[] kindsArray = new TileSet.Kinds[2] { TileSet.Kinds.Pung, TileSet.Kinds.Chow };
                    foreach (TileSet.Kinds k in kindsArray)
                    {
                        if (!isUsed[tileSetCheckIndex])
                        {
                            switch (k)
                            {
                                case TileSet.Kinds.Pung:
                                    if (!IsPung(hands, ref isUsed, ref tempTileSets, tileSetCheckIndex))
                                    {
                                        IsChow(hands, ref isUsed, ref tempTileSets, tileSetCheckIndex);
                                    }
                                    break;
                                case TileSet.Kinds.Chow:
                                    if (!IsChow(hands, ref isUsed, ref tempTileSets, tileSetCheckIndex))
                                    {
                                        IsPung(hands, ref isUsed, ref tempTileSets, tileSetCheckIndex);
                                    }
                                    break;
                            }
                        }
                    }
                }

                /// 暫定面子
                if (tempTileSets.Count == (hands.Count() + 1) / 3)
                {
                    /// 面子構成が見つかったのでtrueを返す
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 国士無双が揃っているか判定する。
        /// 門前手のみ。
        /// </summary>
        /// <param name="hands">門前手牌</param>
        /// <returns>国士無双が揃っているか  true:揃っている  false:揃っていない</returns>
        private static bool IsThirteenOrphans(Tile[] hands)
        {
            /// 引数妥当性チェック
            if (hands.Count() != 14)
            {
                throw new ArgumentException(string.Format("Argument {0} is invalid.", nameof(hands)), nameof(hands));
            }

            int nPair = 0;  // 対子数チェック
            Tile prevTile = null;   // ループの前の周の牌を保持。対子判定のため。
            foreach (Tile t in hands)
            {
                /// 対子の数をチェック
                if (prevTile != null)
                {
                    if (t.Kind == prevTile.Kind)
                    {
                        nPair++;
                        if (nPair > 1) return false;
                    }
                }

                switch (t.Kind)
                {
                    case Tile.Kinds.Character1:
                    case Tile.Kinds.Character9:
                    case Tile.Kinds.Circle1:
                    case Tile.Kinds.Circle9:
                    case Tile.Kinds.Bamboo1:
                    case Tile.Kinds.Bamboo9:
                    case Tile.Kinds.East:
                    case Tile.Kinds.South:
                    case Tile.Kinds.West:
                    case Tile.Kinds.North:
                    case Tile.Kinds.WhiteDragon:
                    case Tile.Kinds.GreenDragon:
                    case Tile.Kinds.RedDragon:
                        /// 指定牌であれば、次の牌チェック
                        break;
                    default:
                        /// 指定牌以外があれば即return
                        return false;
                }

                prevTile = t;
            }

            return true;
        }

        /// <summary>
        /// 七対子が揃っているか判定する。
        /// 門前手のみ。
        /// </summary>
        /// <param name="hands">門前手牌。呼び出し元でソート済み。</param>
        /// <returns>七対子が揃っているか  true:揃っている  false:揃っていない</returns>
        private static bool IsSevenPairs(Tile[] hands)
        {
            if (hands.Count() != 14)
            {
                throw new ArgumentException(string.Format("Argument {0} is invalid.", nameof(hands)), nameof(hands));
            }
            
            /// 手牌を精査
            for (int i = 0; i < hands.Count(); i++)
            {
                if (i == 0)
                {
                    /// do nothing
                }
                else if (i % 2 == 0)
                {
                    if (hands[i - 1].Kind == hands[i].Kind)
                    {
                        /// 偶数個目は、1つ前と同じ牌ではいけない
                        return false;
                    }
                }
                else //if (i % 2 == 1) のとき
                {
                    if (hands[i - 1].Kind != hands[i].Kind)
                    {
                        /// 奇数個目は、1つ前と違う牌ではいけない
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 対子になりえるかチェックする
        /// </summary>
        /// <param name="hands">手牌の配列</param>
        /// <param name="isUsed">使用済みか記憶する配列。対子になれば、該当の2牌のindexに対応する要素をtrueにする。</param>
        /// <param name="tempTileSets">暫定手牌。対子になった場合はこの配列に追加する。</param>
        /// <param name="index">hands, isUsedの配列のindex</param>
        /// <returns>対子になりえるか  true:なりえる  false:なりえない</returns>
        private static bool IsPair(Tile[] hands, ref bool[] isUsed, ref List<TileSet> tempTileSets, int index)
        {
            /// 引数妥当性チェック
            if (hands.Count() != isUsed.Count())
            {
                throw new ArgumentException("Array Arguments 'hands' and 'isUsed' count are not equaled.");
            }
            if (index < 0 || index + 1 >= hands.Count())
            {
                throw new ArgumentOutOfRangeException(string.Format("Argument 'index'{0} is invalid.", index));
            }

            /// 対子チェック
            if (hands[index].Kind == hands[index + 1].Kind)
            {
                isUsed[index] = true;
                isUsed[index + 1] = true;
                Tile[] t = new Tile[2] { hands[index], hands[index + 1] };
                TileSet ts = new TileSet();
                ts.Kind = TileSet.Kinds.Pair;
                ts.Tiles = t;
                if (!ts.IsValidTileSet())
                {
                    throw new Exception("The implementation is NG.");
                }
                tempTileSets.Add(ts);

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 刻子になりえるかチェックする
        /// </summary>
        /// <param name="hands">手牌の配列</param>
        /// <param name="isUsed">使用済みか記憶する配列。刻子になれば、該当の3牌のindexに対応する要素をtrueにする。</param>
        /// <param name="tempTileSets">暫定手牌。刻子になった場合はこの配列に追加する。</param>
        /// <param name="index">hands, isUsedの配列のindex</param>
        /// <returns>刻子になりえるか  true:なりえる  false:なりえない</returns>
        private static bool IsPung(Tile[] hands, ref bool[] isUsed, ref List<TileSet> tempTileSets, int index)
        {
            /// 引数妥当性チェック
            if (hands.Count() != isUsed.Count())
            {
                throw new ArgumentException("Array Arguments 'hands' and 'isUsed' count are not equaled.");
            }
            if (index < 0 || index + 2 >= hands.Count())
            {
                throw new ArgumentOutOfRangeException(string.Format("Argument 'index'{0} is invalid.", index));
            }
            if (isUsed[index])
            {
                return false;
            }

            /// 牌チェック
            int num = 1;
            int[] pungIndices = new int[3];
            pungIndices[0] = index;
            for (int checkIndex = index+1; checkIndex < hands.Count(); checkIndex++)
            {
                if (hands[index].Kind == hands[checkIndex].Kind && !isUsed[checkIndex])
                {
                    pungIndices[num] = checkIndex;
                    num++;
                    if (num == 3)
                    {
                        break;
                    }
                }
            }

            /// 刻子チェック
            if (num == 3)
            {
                Tile[] t = new Tile[3];
                for (int i = 0; i < pungIndices.Count(); i++)
                {
                    isUsed[pungIndices[i]] = true;
                    t[i] = hands[pungIndices[i]];
                }
                TileSet ts = new TileSet();
                ts.Kind = TileSet.Kinds.Pung;
                ts.Tiles = t;
                if (!ts.IsValidTileSet())
                {
                    throw new Exception("The implementation is NG.");
                }
                tempTileSets.Add(ts);

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 順子になりえるかチェックする
        /// </summary>
        /// <param name="hands">手牌の配列</param>
        /// <param name="isUsed">使用済みか記憶する配列。順子になれば、該当の3牌のindexに対応する要素をtrueにする。</param>
        /// <param name="tempTileSets">暫定手牌。順子になった場合はこの配列に追加する。</param>
        /// <param name="index">hands, isUsedの配列のindex</param>
        /// <returns>順子になりえるか  true:なりえる  false:なりえない</returns>
        private static bool IsChow(Tile[] hands, ref bool[] isUsed, ref List<TileSet> tempTileSets, int index)
        {
            /// 引数妥当性チェック
            if (hands.Count() != isUsed.Count())
            {
                throw new ArgumentException("Array Arguments 'hands' and 'isUsed' count are not equaled.");
            }
            if (index < 0 || index + 2 >= hands.Count())
            {
                throw new ArgumentOutOfRangeException(string.Format("Argument 'index'{0} is invalid.", index));
            }
            if (isUsed[index])
            {
                return false;
            }

            /// 数牌でなければ即return false
            if (!hands[index].IsSuit)
            {
                return false;
            }

            /// 数牌でもチェック対象の数が8以上であれば、即return false
            if (hands[index].Number >= 8)
            {
                return false;
            }

            /// 牌チェック
            int num = 1;
            int[] chowIndices = new int[3];
            chowIndices[0] = index;
            for (int checkIndex = index + 1; checkIndex < hands.Count(); checkIndex++)
            {
                if (MahjongLogicUtility.IsSameType(hands[checkIndex], hands[index]) && !isUsed[checkIndex])
                {
                    if (hands[checkIndex].Number == hands[index].Number + num && !isUsed[checkIndex])
                    {
                        chowIndices[num] = checkIndex;
                        num++;
                        if (num == 3)
                        {
                            break;
                        }
                    }
                }
            }

            ///順子チェック
            if (num == 3)
            {
                Tile[] t = new Tile[3];
                for (int i = 0; i < chowIndices.Count(); i++)
                {
                    isUsed[chowIndices[i]] = true;
                    t[i] = hands[chowIndices[i]];
                }
                TileSet ts = new TileSet();
                ts.Kind = TileSet.Kinds.Chow;
                ts.Tiles = t;
                if (!ts.IsValidTileSet())
                {
                    throw new Exception("The implementation is NG.");
                }
                tempTileSets.Add(ts);

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 配列の非nullの要素の個数を数える
        /// </summary>
        /// <param name="array">調査対象の配列</param>
        /// <returns>非nullの要素の個数</returns>
        private static int CountEntityElements(Array array)
        {
            int num = 0;
            foreach (object o in array)
            {
                if (o != null)
                {
                    num++;
                }
            }
            return num;
        }

        /// <summary>
        /// 同じ数牌の種類かチェックする
        /// </summary>
        /// <param name="a">チェック対象数牌A</param>
        /// <param name="b">チェック対象数牌B</param>
        /// <returns>チェック対象数牌Aとチェック対象数牌Bが同じ種類の数牌か  true:同じ種類  false:異なる種類</returns>
        private static bool IsSameType(Tile a, Tile b)
        {
            bool ret = false;
            ret |= a.IsCharacter && b.IsCharacter;
            ret |= a.IsCircle && b.IsCircle;
            ret |= a.IsBamboo && b.IsBamboo;
            return ret;
        }
    }
}
