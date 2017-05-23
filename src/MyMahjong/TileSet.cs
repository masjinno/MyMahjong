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
                return this._tiles;
            }
            set
            {
                this._tiles = value;

                Array.Sort(this._tiles, (a, b) => a.Index - b.Index);
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
                switch (this.Kind)
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
            bool ret = true;

            if (this.Tiles.Count() != this.TileNum)
            {
                ret = false;
            }
            else
            {
                switch (this.Kind)
                {
                    case Kinds.Pair:
                        ret = (this.Tiles[0].Kind == this.Tiles[1].Kind);   /// 同じ値ならtrue
                        break;
                    case Kinds.Chow:
                        if (this.Tiles[2].IsSuit)   /// 数牌でなければならない
                        {
                            if ((this.Tiles[0].IsCharacter == this.Tiles[2].IsCharacter) &&
                                (this.Tiles[0].IsCircle == this.Tiles[2].IsCircle) &&
                                (this.Tiles[0].IsBamboo == this.Tiles[2].IsBamboo))     /// 同じ数牌でなければならない
                            {
                                /// 連番でならtrue
                                ret = ((this.Tiles[0].Number + 1 == this.Tiles[1].Number) && (this.Tiles[0].Number + 2 == this.Tiles[2].Number));
                            }
                            else
                            {
                                ret = false;
                            }
                        }
                        else
                        {
                            ret = false;
                        }
                        break;
                    case Kinds.Pung:
                        ret = (this.Tiles[0].Kind == this.Tiles[1].Kind) && (this.Tiles[0].Kind == this.Tiles[2].Kind);
                        break;
                    case Kinds.Kong:
                        ret = (this.Tiles[0].Kind == this.Tiles[1].Kind) && (this.Tiles[0].Kind == this.Tiles[2].Kind) && (this.Tiles[0].Kind == this.Tiles[3].Kind);
                        break;
                    default:
                        ret = false;
                        break;
                }
            }

            return ret;
        }
    }
}
