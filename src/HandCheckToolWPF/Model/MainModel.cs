using MyMahjong;
using HandCheckToolWPF.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HandCheckToolWPF.Model
{
    /// <summary>
    /// MainViewModelから呼ばれる
    /// </summary>
    public class MainModel
    {
        /// <summary>牌一覧</summary>
        public Tile[] tileArray { get; private set; }

        /// <summary>門前牌</summary>
        public Tile[] concealedTileArray { get; private set; }

        /// <summary>和了牌</summary>
        public Tile winningTile { get; private set; }

        /// <summary>鳴いた牌</summary>
        public TileSet[] openedTileSet { get; private set; }

        /// <summary>ドラ表示牌</summary>
        public Tile[,] doraIndicatorArray { get; private set; }

        /// <summary>
        /// 面前手配の枚数の最大値。
        /// さらした牌の枚数に応じて変化する。
        /// </summary>
        private int concealedTileNumMax;

        /// <summary>
        /// 見た目上の面前手配の枚数。
        /// 牌一覧から追加する度に増加する。
        /// </summary>
        private int concealedTileNum;

        /// <summary>
        /// コンストラクタ
        /// ・メンバ配列の初期化
        /// </summary>
        public MainModel()
        {
            this.tileArray = new Tile[37];
            this.concealedTileArray = new Tile[13];
            this.openedTileSet = new TileSet[4];
            this.doraIndicatorArray = new Tile[2, 5];
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
        public Tile[] InitializeTileArray()
        {
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
            for (int i = 0; i < this.tileArray.Count(); i++)
            {
                this.tileArray[i] = new Tile();
                this.tileArray[i].Kind = TileArrayKindsDictionary[i].kinds;
                this.tileArray[i].FrontImage = this.GetImageSource(TileArrayKindsDictionary[i].frontImagePath);
            }
            this.concealedTileNumMax = 13;
            this.concealedTileNum = 0;

            return this.tileArray;
        }

        /// <summary>
        /// 牌をクリアする。
        /// 対象は以下の牌
        /// ・面前手牌
        /// ・和了牌
        /// ・鳴いた牌
        /// ・ドラ表示牌
        /// </summary>
        /// <returns>
        /// Tuple.item1: 門前手牌
        /// Tuple.item2: 和了牌
        /// Tuple.item3: 鳴いた牌
        /// Tuple.item4: ドラ表示牌
        /// </returns>
        public Tuple<Tile[], Tile, TileSet[], Tile[,]> ClearTiles()
        {
            /// 面前手牌クリア
            for (int i = 0; i < concealedTileArray.Count(); i++)
            {
                concealedTileArray[i] = null;
            }
            concealedTileNum = 0;

            // 和了牌クリア
            winningTile = null;

            // TODO: 鳴いた牌クリア処理

            // TODO: ドラ表示牌クリア処理

            return new Tuple<Tile[], Tile, TileSet[], Tile[,]>(this.concealedTileArray, this.winningTile, this.openedTileSet, this.doraIndicatorArray);
        }

        /// <summary>
        /// 和了牌のクリア
        /// </summary>
        /// <returns>クリア後の和了牌(=null)</returns>
        public Tile ClearWinningTile()
        {
            this.winningTile = null;
            return this.winningTile;
        }

        /// <summary>
        /// 面前手牌に牌を追加する
        /// </summary>
        /// <param name="addedTile">追加される牌</param>
        public Tuple<bool, Tile[]> AddConcealedTile(Tile addedTile)
        {
            bool isSucceeded;
            if (this.concealedTileNum < this.concealedTileNumMax)
            {
                /// 手配に追加
                this.concealedTileArray[this.concealedTileNum] = addedTile;
                Array.Sort(concealedTileArray, 0, this.concealedTileNum + 1, new Tile.TileIndexCompare());
                this.concealedTileNum++;

                isSucceeded = true;
            }
            else
            {
                isSucceeded = false;
            }

            return new Tuple<bool, Tile[]>(isSucceeded, this.concealedTileArray);
        }

        /// <summary>
        /// 和了牌を設定する
        /// </summary>
        /// <param name="drawnTile">新たな和了牌</param>
        public Tile DrawWinningTile(Tile drawnTile)
        {
            this.winningTile = drawnTile;
            return this.winningTile;
        }

        /// <summary>
        /// ドラ表示牌セット
        /// </summary>
        /// <param name="row">セットするドラ表示牌が表か裏か  0:表  1:裏</param>
        /// <param name="column">セットするドラ表示牌が何番目か</param>
        /// <param name="newTile">セットするドラ表示牌</param>
        /// <returns>新しいドラ表示牌</returns>
        public Tile[,] SetDoraIndicator(int row, int column, Tile newTile)
        {
            this.doraIndicatorArray[row, column] = newTile;
            return this.doraIndicatorArray;
        }

        /// <summary>
        /// 手牌を1枚捨てる
        /// </summary>
        /// <param name="index">捨て牌の手牌インデックス</param>
        public Tile[] DiscardConcealedTile(int index)
        {
            if (index < 0 || concealedTileArray.Count() <= index)
            {
                throw new ArgumentOutOfRangeException(nameof(index), string.Format("Argument '{0}'({1}) is out of range.", nameof(index), index));
            }

            for (int i = index; i < concealedTileArray.Count() - 1; i++)
            {
                concealedTileArray[i] = concealedTileArray[i + 1];
            }
            concealedTileArray[concealedTileArray.Count() - 1] = null;
            this.concealedTileNum--;

            return this.concealedTileArray;
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
