using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyMahjong
{
    /// <summary>
    /// 牌1枚が持つ情報を定義するクラス
    /// </summary>
    public class Tile
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
        public Tile.Kinds Kind { get; set; }

        /// <summary>
        /// 数牌のときの数牌種類。
        /// 数牌でない場合の値は未定義。
        /// </summary>
        public Tile.SuitKinds SuitKind { get; set; }

        /// <summary>
        /// 数牌のときの数字。
        /// 数牌でない場合の値は未定義。
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 字牌のときの字牌種類。
        /// 字牌でない場合は未定義。
        /// </summary>
        public Tile.HonourKinds HonourKind { get; set; }

        /// <summary>
        /// 風牌のときの風牌種類。
        /// 風牌でない場合は未定義。
        /// </summary>
        public MahjongLogic.WindKinds WindKind { get; set; }

        /// <summary>
        /// 三元牌のときの三元牌種類。
        /// 三元牌でない場合は未定義。
        /// </summary>
        public Tile.DragonKinds DragonKind { get; set; }

        /// <summary>
        /// ドラ
        /// </summary>
        public int Dora { get; set; }

        /// <summary>
        /// ソート用の牌種によるインデックス
        /// </summary>
        public int Index
        {
            get
            {
                int tmpIndex = -1;
                switch (this.Kind)
                {
                    case Tile.Kinds.Suit:
                        switch (this.SuitKind)
                        {
                            case SuitKinds.Character: tmpIndex = this.Number + 0; break;
                            case SuitKinds.Circle   : tmpIndex = this.Number + 10; break;
                            case SuitKinds.Bamboo   : tmpIndex = this.Number + 20; break;
                        }
                        break;
                    case Tile.Kinds.Honour:
                        switch (this.HonourKind)
                        {
                            case HonourKinds.Wind:
                                switch (this.WindKind)
                                {
                                    case MahjongLogic.WindKinds.East : tmpIndex = 31; break;
                                    case MahjongLogic.WindKinds.South: tmpIndex = 32; break;
                                    case MahjongLogic.WindKinds.West: tmpIndex = 33; break;
                                    case MahjongLogic.WindKinds.North: tmpIndex = 34; break;
                                }
                                break;
                            case HonourKinds.Dragon:
                                switch (this.DragonKind)
                                {
                                    case DragonKinds.WhiteDragon: tmpIndex = 41; break;
                                    case DragonKinds.GreenDragon: tmpIndex = 42; break;
                                    case DragonKinds.RedDragon: tmpIndex = 43; break;
                                }
                                break;
                        }
                        break;
                }
                if (tmpIndex == -1)
                {
                    throw new ArgumentException(string.Format("{0}, {1}, {2}, {3} or {4} is invalid.",
                        nameof(this.Kind), nameof(this.SuitKind), nameof(this.HonourKind), nameof(this.WindKind), nameof(this.DragonKind)));
                }

                return tmpIndex;
            }
        }

        /// <summary>
        /// ドラ
        /// </summary>
        public int Dora { get; set; }

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
}
