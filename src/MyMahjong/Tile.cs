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
            Character1,     // 一萬
            Character2,     // 二萬
            Character3,     // 三萬
            Character4,     // 四萬
            Character5,     // 五萬
            Character6,     // 六萬
            Character7,     // 七萬
            Character8,     // 八萬
            Character9,     // 九萬
            Circle1,        // 一筒
            Circle2,        // 二筒
            Circle3,        // 三筒
            Circle4,        // 四筒
            Circle5,        // 五筒
            Circle6,        // 六筒
            Circle7,        // 七筒
            Circle8,        // 八筒
            Circle9,        // 九筒
            Bamboo1,        // 一索
            Bamboo2,        // 二索
            Bamboo3,        // 三索
            Bamboo4,        // 四索
            Bamboo5,        // 五索
            Bamboo6,        // 六索
            Bamboo7,        // 七索
            Bamboo8,        // 八索
            Bamboo9,        // 九索
            East,           // 東
            Sourth,         // 南
            West,           // 西
            North,          // 北
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
        /// ソート用の牌種によるインデックス
        /// </summary>
        public int Index
        {
            get
            {
                Dictionary<Tile.Kinds, int> kind2IndexMap = new Dictionary<Kinds, int>()
                {
                    { Kinds.Character1, 0 }, { Kinds.Character2, 1 }, { Kinds.Character3, 2 },
                    { Kinds.Character4, 3 }, { Kinds.Character5, 4 }, { Kinds.Character6, 5 },
                    { Kinds.Character7, 6 }, { Kinds.Character8, 7 }, { Kinds.Character9, 8 },
                    { Kinds.Circle1, 9 }, { Kinds.Circle2, 10 }, { Kinds.Circle3, 11 },
                    { Kinds.Circle4, 12 }, { Kinds.Circle5, 13 }, { Kinds.Circle6, 14 },
                    { Kinds.Circle7, 15 }, { Kinds.Circle8, 16 }, { Kinds.Circle9, 17 },
                    { Kinds.Bamboo1, 18 }, { Kinds.Bamboo2, 19 }, { Kinds.Bamboo3, 20 },
                    { Kinds.Bamboo4, 21 }, { Kinds.Bamboo5, 22 }, { Kinds.Bamboo6, 23 },
                    { Kinds.Bamboo7, 24 }, { Kinds.Bamboo8, 25 }, { Kinds.Bamboo9, 26 },
                    { Kinds.East, 27 }, { Kinds.Sourth, 28 }, { Kinds.West, 29 }, { Kinds.North, 30 },
                    { Kinds.WhiteDragon, 31 }, { Kinds.GreenDragon, 32 }, { Kinds.RedDragon, 33 }
                };
                return kind2IndexMap[this.Kind];
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
