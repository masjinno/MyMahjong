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
            // 未実装

            return false;
        }
    }
}
