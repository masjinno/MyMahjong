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

            /// ソート
            Array.Sort(concealedTiles, (a, b) => a.Index - b.Index);

            /// 牌数チェック
            int nTiles = 0;
            nTiles += concealedTiles.Count() + openedSets.Count() * 3 + 1;
            if (nTiles != 14) throw new ArgumentException("The number of tiles is invalid.");

            // 未実装

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
    }
}
