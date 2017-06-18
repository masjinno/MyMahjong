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
            South,          // 南
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
        /// 
        /// </summary>
        private Dictionary<Tile.Kinds, int> kind2IndexMap = new Dictionary<Kinds, int>()
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
            { Kinds.East, 27 }, { Kinds.South, 28 }, { Kinds.West, 29 }, { Kinds.North, 30 },
            { Kinds.WhiteDragon, 31 }, { Kinds.GreenDragon, 32 }, { Kinds.RedDragon, 33 }
        };

        /// <summary>ソート用の牌種によるインデックス</summary>
        public int Index { get { return kind2IndexMap[this.Kind]; } }

        /// <summary>数牌か</summary>
        public bool IsSuit { get { return (IsCharacter || IsCircle || IsBamboo); } }

        /// <summary>字牌か</summary>
        public bool IsHonour { get { return (IsWind || IsDragon); } }

        /// <summary>萬子か</summary>
        public bool IsCharacter { get { return (kind2IndexMap[Kinds.Character1] <= this.Index && this.Index <= kind2IndexMap[Kinds.Character9]); } }

        /// <summary>筒子か</summary>
        public bool IsCircle { get { return (kind2IndexMap[Kinds.Circle1] <= this.Index && this.Index <= kind2IndexMap[Kinds.Circle9]); } }

        /// <summary>索子か</summary>
        public bool IsBamboo { get { return (kind2IndexMap[Kinds.Bamboo1] <= this.Index && this.Index <= kind2IndexMap[Kinds.Bamboo9]); } }

        /// <summary>風牌か</summary>
        public bool IsWind { get { return (kind2IndexMap[Kinds.East] <= this.Index && this.Index <= kind2IndexMap[Kinds.North]); } }

        /// <summary>三元牌か</summary>
        public bool IsDragon { get { return (kind2IndexMap[Kinds.WhiteDragon] <= this.Index && this.Index <= kind2IndexMap[Kinds.RedDragon]); } }

        /// <summary>赤牌か</summary>
        public bool IsRed { get; set; }

        /// <summary>
        /// 数牌の値を取得する
        /// </summary>
        public int Number
        {
            get
            {
                if (!this.IsSuit)
                {
                    throw new InvalidOperationException("Tile must be SuitTile.");
                }
                if (IsCharacter) return this.Index - kind2IndexMap[Kinds.Character1] + 1;
                else if (IsCircle) return this.Index - kind2IndexMap[Kinds.Circle1] + 1;
                else if (IsBamboo) return this.Index - kind2IndexMap[Kinds.Bamboo1] + 1;
                else throw new InvalidOperationException("Kind is invalid.");
            }
        }

        /// <summary>
        /// ドラ
        /// </summary>
        public int Dora { get; set; }

        /// <summary>表面の画像</summary>
        public System.Windows.Media.ImageSource FrontImage { get; set; }

        /// <summary>右から真っ直ぐ見える表面の画像</summary>
        public System.Windows.Media.ImageSource RightFrontImage { get; set; }
        
        /// <summary>裏面の画像</summary>
        public System.Windows.Media.ImageSource BackImage { get; set; }

        /// <summary>右から真っ直ぐ見える裏面の画像</summary>
        public System.Windows.Media.ImageSource RightBackImage { get; set; }

        #endregion

        /// <summary>
        /// 牌ソート用比較クラス
        /// </summary>
        public class TileIndexCompare : System.Collections.IComparer
        {
            /// <summary>
            /// Array.Sortで使用する
            /// </summary>
            /// <param name="x">比較対象1</param>
            /// <param name="y">比較対象2</param>
            /// <returns>正:<paramref name="x"/>が後ろ  負:<paramref name="y"/>が後ろ</returns>
            public int Compare(object x, object y)
            {
                if (!(x is Tile) || !(y is Tile))
                {
                    throw new ArgumentException("Argument type is invalid.");
                }
                return (x as Tile).Index - (y as Tile).Index;
            }
        }
    }
}
