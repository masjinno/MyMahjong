using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMahjongWPF.Model
{
    /// <summary>
    /// 麻雀のロジックに関する定義
    /// </summary>
    public class MahjongLogic
    {
        /// <summary>
        /// 風の情報
        /// </summary>
        public enum WindKinds
        {
            East,
            South,
            West,
            North
        }

        /// <summary>
        /// 牌1枚が持つ情報を定義するクラス
        /// </summary>
        public class TileInfo
        {
            #region 型定義

            /// <summary>
            /// 牌の種類の型定義
            /// </summary>
            public enum Kinds
            {
                Suit,   // 数牌
                Honour  // 字牌
            }

            /// <summary>
            /// 数牌の種類の型定義
            /// </summary>
            public enum SuitKinds
            {
                Character,  // 萬子
                Circle,     // 筒子
                Bamboo      // 索子
            }

            /// <summary>
            /// 字牌の種類の型定義
            /// </summary>
            public enum HonourKinds
            {
                Wind,   // 風牌
                Dragon  // 三元牌
            }

            /// <summary>
            /// 三元牌の種類の型定義
            /// </summary>
            public enum DragonKinds
            {
                WhiteDragon,    // 白
                GreenDragon,    // 發
                RedDragon       // 中
            }

            #endregion

            #region プロパティ

            /// <summary>
            /// 牌の種類
            /// </summary>
            public TileInfo.Kinds Kind { get; set; }

            /// <summary>
            /// 数牌のときの数牌種類。
            /// 数牌でない場合の値は未定義。
            /// </summary>
            public TileInfo.SuitKinds SuitKind { get; set; }

            /// <summary>
            /// 数牌のときの数字。
            /// 数牌でない場合の値は未定義。
            /// </summary>
            public int Number { get; set; }

            /// <summary>
            /// 字牌のときの字牌種類。
            /// 字牌でない場合は未定義。
            /// </summary>
            public TileInfo.HonourKinds HonourKind { get; set; }

            /// <summary>
            /// 風牌のときの風牌種類。
            /// 風牌でない場合は未定義。
            /// </summary>
            public MahjongLogic.WindKinds WindKind { get; set; }

            /// <summary>
            /// 三元牌のときの三元牌種類。
            /// 三元牌でない場合は未定義。
            /// </summary>
            public TileInfo.DragonKinds DragonKind { get; set; }

            /// <summary>
            /// 門前の牌か(⇔鳴いていない牌か)
            /// </summary>
            public bool IsConcealed { get; set; }

            /// <summary>
            /// 表面の画像
            /// </summary>
            public System.Windows.Media.ImageSource FrontImage { get; }

            /// <summary>
            /// 裏面の画像
            /// </summary>
            public System.Windows.Media.ImageSource BackImage { get; }

            #endregion
        }

        /// <summary>
        /// 聴牌判定
        /// </summary>
        /// <returns></returns>
        public static bool IsWaitingHand()
        {
            // 未実装

            return false;
        }
    }
}
