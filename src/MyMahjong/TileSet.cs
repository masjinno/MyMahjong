using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMahjong
{
    /// <summary>
    /// 面子1つを定義するクラス
    /// </summary>
    public class TileSet
    {
        #region 型定義

        /// <summary>
        /// 面子種類の定義
        /// </summary>
        public enum Kinds
        {
            Pair,   // 対子
            Chow,   // 順子
            Pung,   // 刻子
            Kong    // 槓子
        }

        #endregion

        /// <summary>
        /// 面子種類
        /// </summary>
        public TileSet.Kinds Kind { get; set; }

        /// <summary>
        /// 面子の構成
        /// </summary>
        public Tile[] Tiles
        {
            get
            {
                return _tiles;
            }
            set
            {
                _tiles = value;

                // TODO: ソート処理
            }
        }
        private Tile[] _tiles;

        /// <summary>
        /// 面子の枚数
        /// </summary>
        public int TileNum
        {
            get
            {
                switch (Kind)
                {
                    case Kinds.Pair:
                        return 2;
                    case Kinds.Chow:
                    case Kinds.Pung:
                        return 3;
                    case Kinds.Kong:
                        return 4;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(Kind), string.Format("Parameter '{0}'({1}) is out of range.", nameof(Kind), Kind));
                }
            }
        }

        /// <summary>
        /// 面子の妥当性チェック
        /// </summary>
        /// <returns>面子が妥当か  true:妥当  false:妥当でない</returns>
        public bool IsValidTileSet()
        {
            // 未実装

            return false;
        }
    }
}
