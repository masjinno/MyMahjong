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
    }
}
