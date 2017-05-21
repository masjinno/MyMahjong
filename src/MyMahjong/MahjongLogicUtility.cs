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
        /// 国士無双が揃っているか判定する
        /// </summary>
        /// <param name="concealedTiles">門前手牌</param>
        /// <returns>国士無双が揃っているか  true:揃っている  false:揃っていない</returns>
        private static bool IsThirteenOrphans(Tile[] concealedTiles)
        {
            /// 引数妥当性チェック
            if (concealedTiles.Count() != 14)
            {
                throw new ArgumentException(string.Format("Argument {0} is invalid.", nameof(concealedTiles), nameof(concealedTiles)));
            }

            // 未実装

            return false;
        }
    }
}
