using MyMahjong;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HandCheckToolWPF.ViewModel
{
    class MainViewModel : BindableBase
    {
        /// <summary>
        /// 【Bindingプロパティ】
        /// 全種の牌データ
        /// </summary>
        public Tile[] TileArray
        {
            get { return this._tileArray; }
            set { SetProperty(ref this._tileArray, value); }
        }
        private Tile[] _tileArray;

        /// <summary>
        /// 和了牌
        /// </summary>
        public Tile WinningTile
        {
            get { return this._winningTile; }
            set { SetProperty(ref this._winningTile, value); }
        }
        private Tile _winningTile;

        /// <summary>
        /// 門前手牌
        /// </summary>
        public Tile[] ConcealedTileArray
        {
            get { return this._concealedTileArray; }
            set { SetProperty(ref this._concealedTileArray, value); }
        }
        private Tile[] _concealedTileArray;

        /// <summary>
        /// ドラ表示牌
        /// </summary>
        public Tile[,] DoraTileArray
        {
            get { return this._doraTileArray; }
            set { SetProperty(ref this._doraTileArray, value); }
        }
        private Tile[,] _doraTileArray;

        /// <summary>
        /// コンストラクタ
        /// ・変数の初期化
        /// </summary>
        public MainViewModel()
        {
            /// TileArrayメンバ初期化
            this.InitializeTileArray();

            /// デバッグ用の代入
            this.WinningTile = this.TileArray[0];

            this.ConcealedTileArray = new Tile[13];
            for (int i=0; i<this.ConcealedTileArray.Count(); i++)
            {
                /// デバッグ用の代入
                this.ConcealedTileArray[i] = this.TileArray[i + 10];
            }

            /// ドラ表示牌の初期化
            this.DoraTileArray = new Tile[2, 5];
            for (int i = 0; i < this.DoraTileArray.GetLength(0); i++)
            {
                for (int j = 0; j < this.DoraTileArray.GetLength(1); j++)
                {
                    /// 
                    this.DoraTileArray[i, j] = this.TileArray[0];
                }
            }
        }

        /// <summary>
        /// Tile型初期化用のデータ郡
        /// </summary>
        struct TileDatas
        {
            public Tile.Kinds kinds;
            public string frontImagePath;
        }
        /// <summary>
        /// TileArrayメンバの初期化
        /// </summary>
        private void InitializeTileArray()
        {
            const int TILE_NUM = 37;
            Dictionary<int, TileDatas> TileArrayKindsDictionary = new Dictionary<int, TileDatas>()
            {
                { 0, new TileDatas{kinds = Tile.Kinds.Character1 , frontImagePath = @"..\..\Resource\Image\Tile\tile_00_1m.png"   } },
                { 1, new TileDatas{kinds = Tile.Kinds.Character2 , frontImagePath = @"..\..\Resource\Image\Tile\tile_01_2m.png"   } },
                { 2, new TileDatas{kinds = Tile.Kinds.Character3 , frontImagePath = @"..\..\Resource\Image\Tile\tile_02_3m.png"   } },
                { 3, new TileDatas{kinds = Tile.Kinds.Character4 , frontImagePath = @"..\..\Resource\Image\Tile\tile_03_4m.png"   } },
                { 4, new TileDatas{kinds = Tile.Kinds.Character5 , frontImagePath = @"..\..\Resource\Image\Tile\tile_04_5m.png"   } },
                { 5, new TileDatas{kinds = Tile.Kinds.Character5 , frontImagePath = @"..\..\Resource\Image\Tile\tile_04_5m_r.png" } },
                { 6, new TileDatas{kinds = Tile.Kinds.Character6 , frontImagePath = @"..\..\Resource\Image\Tile\tile_05_6m.png"   } },
                { 7, new TileDatas{kinds = Tile.Kinds.Character7 , frontImagePath = @"..\..\Resource\Image\Tile\tile_06_7m.png"   } },
                { 8, new TileDatas{kinds = Tile.Kinds.Character8 , frontImagePath = @"..\..\Resource\Image\Tile\tile_07_8m.png"   } },
                { 9, new TileDatas{kinds = Tile.Kinds.Character9 , frontImagePath = @"..\..\Resource\Image\Tile\tile_08_9m.png"   } },
                {10, new TileDatas{kinds = Tile.Kinds.Circle1    , frontImagePath = @"..\..\Resource\Image\Tile\tile_09_1p.png"   } },
                {11, new TileDatas{kinds = Tile.Kinds.Circle2    , frontImagePath = @"..\..\Resource\Image\Tile\tile_10_2p.png"   } },
                {12, new TileDatas{kinds = Tile.Kinds.Circle3    , frontImagePath = @"..\..\Resource\Image\Tile\tile_11_3p.png"   } },
                {13, new TileDatas{kinds = Tile.Kinds.Circle4    , frontImagePath = @"..\..\Resource\Image\Tile\tile_12_4p.png"   } },
                {14, new TileDatas{kinds = Tile.Kinds.Circle5    , frontImagePath = @"..\..\Resource\Image\Tile\tile_13_5p.png"   } },
                {15, new TileDatas{kinds = Tile.Kinds.Circle5    , frontImagePath = @"..\..\Resource\Image\Tile\tile_13_5p_r.png" } },
                {16, new TileDatas{kinds = Tile.Kinds.Circle6    , frontImagePath = @"..\..\Resource\Image\Tile\tile_14_6p.png"   } },
                {17, new TileDatas{kinds = Tile.Kinds.Circle7    , frontImagePath = @"..\..\Resource\Image\Tile\tile_15_7p.png"   } },
                {18, new TileDatas{kinds = Tile.Kinds.Circle8    , frontImagePath = @"..\..\Resource\Image\Tile\tile_16_8p.png"   } },
                {19, new TileDatas{kinds = Tile.Kinds.Circle9    , frontImagePath = @"..\..\Resource\Image\Tile\tile_17_9p.png"   } },
                {20, new TileDatas{kinds = Tile.Kinds.Bamboo1    , frontImagePath = @"..\..\Resource\Image\Tile\tile_18_1s.png"   } },
                {21, new TileDatas{kinds = Tile.Kinds.Bamboo2    , frontImagePath = @"..\..\Resource\Image\Tile\tile_19_2s.png"   } },
                {22, new TileDatas{kinds = Tile.Kinds.Bamboo3    , frontImagePath = @"..\..\Resource\Image\Tile\tile_20_3s.png"   } },
                {23, new TileDatas{kinds = Tile.Kinds.Bamboo4    , frontImagePath = @"..\..\Resource\Image\Tile\tile_21_4s.png"   } },
                {24, new TileDatas{kinds = Tile.Kinds.Bamboo5    , frontImagePath = @"..\..\Resource\Image\Tile\tile_22_5s.png"   } },
                {25, new TileDatas{kinds = Tile.Kinds.Bamboo5    , frontImagePath = @"..\..\Resource\Image\Tile\tile_22_5s_r.png" } },
                {26, new TileDatas{kinds = Tile.Kinds.Bamboo6    , frontImagePath = @"..\..\Resource\Image\Tile\tile_23_6s.png"   } },
                {27, new TileDatas{kinds = Tile.Kinds.Bamboo7    , frontImagePath = @"..\..\Resource\Image\Tile\tile_24_7s.png"   } },
                {28, new TileDatas{kinds = Tile.Kinds.Bamboo8    , frontImagePath = @"..\..\Resource\Image\Tile\tile_25_8s.png"   } },
                {29, new TileDatas{kinds = Tile.Kinds.Bamboo9    , frontImagePath = @"..\..\Resource\Image\Tile\tile_26_9s.png"   } },
                {30, new TileDatas{kinds = Tile.Kinds.East       , frontImagePath = @"..\..\Resource\Image\Tile\tile_27_e.png"    } },
                {31, new TileDatas{kinds = Tile.Kinds.South      , frontImagePath = @"..\..\Resource\Image\Tile\tile_28_s.png"    } },
                {32, new TileDatas{kinds = Tile.Kinds.West       , frontImagePath = @"..\..\Resource\Image\Tile\tile_29_w.png"    } },
                {33, new TileDatas{kinds = Tile.Kinds.North      , frontImagePath = @"..\..\Resource\Image\Tile\tile_30_n.png"    } },
                {34, new TileDatas{kinds = Tile.Kinds.WhiteDragon, frontImagePath = @"..\..\Resource\Image\Tile\tile_31_wd.png"   } },
                {35, new TileDatas{kinds = Tile.Kinds.GreenDragon, frontImagePath = @"..\..\Resource\Image\Tile\tile_32_gd.png"   } },
                {36, new TileDatas{kinds = Tile.Kinds.RedDragon  , frontImagePath = @"..\..\Resource\Image\Tile\tile_33_rd.png"   } }
            };

            /// 選択牌一覧
            this.TileArray = new Tile[TILE_NUM];
            for (int i = 0; i < this.TileArray.Count(); i++)
            {
                this.TileArray[i] = new Tile();
                this.TileArray[i].Kind = TileArrayKindsDictionary[i].kinds;
                this.TileArray[i].FrontImage = this.GetImageSource(TileArrayKindsDictionary[i].frontImagePath);
            }
        }

        /// <summary>
        /// パスの画像をImageSourceとして返す
        /// </summary>
        /// <param name="path">ファイルパス</param>
        /// <returns>ImageSource</returns>
        private ImageSource GetImageSource(string path)
        {
            Image image = new Image();
            BitmapImage bmpImage = new BitmapImage();
            using (FileStream fs = File.OpenRead(path))
            {
                bmpImage.BeginInit();
                bmpImage.StreamSource = fs;
                bmpImage.CacheOption = BitmapCacheOption.OnLoad;
                bmpImage.EndInit();
            }
            return bmpImage;
        }
    }
}
