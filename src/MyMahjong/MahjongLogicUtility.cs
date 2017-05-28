﻿using System;
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
            // 未実装

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
            
            Tile[] hands = new Tile[concealedTiles.Count() + 1];
            concealedTiles.CopyTo(hands, 0);
            hands[concealedTiles.Count()] = drawnTile;

            /// 手牌をソート
            Array.Sort(hands, (a, b) => a.Index - b.Index);

            /// 牌数チェック
            int nTiles = 0;
            nTiles += concealedTiles.Count() + openedSets.Count() * 3 + 1;
            if (nTiles != 14) throw new ArgumentException("The number of tiles is invalid.");

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
            bool[] isPicked = new bool[concealedTiles.Count()];
            for (int tileIndex = 0; tileIndex<concealedTiles.Count(); tileIndex++)
            {
                /// 面子構成判定配列の初期化
                for (int i = 0; i < isPicked.Count(); i++) isPicked[i] = false;

                /// 雀頭候補を決定

                /// 刻子チェック

                /// 順子チェック
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
                if (i % 2 == 0)
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
    }
}
