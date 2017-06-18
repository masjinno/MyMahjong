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
        public Tile[] TileArray { get; private set; }

        /// <summary>門前牌</summary>
        public Tile[] ConcealedTileArray { get; private set; }

        /// <summary>和了牌</summary>
        public Tile WinningTile { get; private set; }

        /// <summary>鳴いた牌</summary>
        public TileSet[] MeldedTileSetArray { get; private set; }

        /// <summary>ドラ表示牌</summary>
        public Tile[,] DoraIndicatorArray { get; private set; }

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
            this.TileArray = new Tile[37];
            this.ConcealedTileArray = new Tile[13];
            this.MeldedTileSetArray = new TileSet[4];
            this.DoraIndicatorArray = new Tile[2, 5];
        }

        /// <summary>
        /// Tile型初期化用のデータ郡
        /// </summary>
        struct TileDatas
        {
            public Tile.Kinds kinds;
            public bool isRed;
            public string frontImagePath;
            public string rightFrontImagePath;
        }
        /// <summary>
        /// TileArrayメンバの初期化
        /// </summary>
        public Tile[] InitializeTileArray()
        {
            Dictionary<int, TileDatas> TileArrayKindsDictionary = new Dictionary<int, TileDatas>()
            {
                { 0, new TileDatas{kinds = Tile.Kinds.Character1 , isRed = false, frontImagePath = @"..\..\Resource\Image\Tile\tile_00_1m.png"    , rightFrontImagePath = @"..\..\Resource\Image\Tile\tile_00_1m_right.png"     } },
                { 1, new TileDatas{kinds = Tile.Kinds.Character2 , isRed = false, frontImagePath = @"..\..\Resource\Image\Tile\tile_01_2m.png"    , rightFrontImagePath = @"..\..\Resource\Image\Tile\tile_01_2m_right.png"     } },
                { 2, new TileDatas{kinds = Tile.Kinds.Character3 , isRed = false, frontImagePath = @"..\..\Resource\Image\Tile\tile_02_3m.png"    , rightFrontImagePath = @"..\..\Resource\Image\Tile\tile_02_3m_right.png"     } },
                { 3, new TileDatas{kinds = Tile.Kinds.Character4 , isRed = false, frontImagePath = @"..\..\Resource\Image\Tile\tile_03_4m.png"    , rightFrontImagePath = @"..\..\Resource\Image\Tile\tile_03_4m_right.png"     } },
                { 4, new TileDatas{kinds = Tile.Kinds.Character5 , isRed = false, frontImagePath = @"..\..\Resource\Image\Tile\tile_04_5m.png"    , rightFrontImagePath = @"..\..\Resource\Image\Tile\tile_04_5m_right.png"     } },
                { 5, new TileDatas{kinds = Tile.Kinds.Character5 , isRed = true , frontImagePath = @"..\..\Resource\Image\Tile\tile_04_5m_red.png", rightFrontImagePath = @"..\..\Resource\Image\Tile\tile_04_5m_red.png" } },
                { 6, new TileDatas{kinds = Tile.Kinds.Character6 , isRed = false, frontImagePath = @"..\..\Resource\Image\Tile\tile_05_6m.png"    , rightFrontImagePath = @"..\..\Resource\Image\Tile\tile_05_6m_right.png"     } },
                { 7, new TileDatas{kinds = Tile.Kinds.Character7 , isRed = false, frontImagePath = @"..\..\Resource\Image\Tile\tile_06_7m.png"    , rightFrontImagePath = @"..\..\Resource\Image\Tile\tile_06_7m_right.png"     } },
                { 8, new TileDatas{kinds = Tile.Kinds.Character8 , isRed = false, frontImagePath = @"..\..\Resource\Image\Tile\tile_07_8m.png"    , rightFrontImagePath = @"..\..\Resource\Image\Tile\tile_07_8m_right.png"     } },
                { 9, new TileDatas{kinds = Tile.Kinds.Character9 , isRed = false, frontImagePath = @"..\..\Resource\Image\Tile\tile_08_9m.png"    , rightFrontImagePath = @"..\..\Resource\Image\Tile\tile_08_9m_right.png"     } },
                {10, new TileDatas{kinds = Tile.Kinds.Circle1    , isRed = false, frontImagePath = @"..\..\Resource\Image\Tile\tile_09_1p.png"    , rightFrontImagePath = @"..\..\Resource\Image\Tile\tile_09_1p_right.png"     } },
                {11, new TileDatas{kinds = Tile.Kinds.Circle2    , isRed = false, frontImagePath = @"..\..\Resource\Image\Tile\tile_10_2p.png"    , rightFrontImagePath = @"..\..\Resource\Image\Tile\tile_10_2p_right.png"     } },
                {12, new TileDatas{kinds = Tile.Kinds.Circle3    , isRed = false, frontImagePath = @"..\..\Resource\Image\Tile\tile_11_3p.png"    , rightFrontImagePath = @"..\..\Resource\Image\Tile\tile_11_3p_right.png"     } },
                {13, new TileDatas{kinds = Tile.Kinds.Circle4    , isRed = false, frontImagePath = @"..\..\Resource\Image\Tile\tile_12_4p.png"    , rightFrontImagePath = @"..\..\Resource\Image\Tile\tile_12_4p_right.png"     } },
                {14, new TileDatas{kinds = Tile.Kinds.Circle5    , isRed = false, frontImagePath = @"..\..\Resource\Image\Tile\tile_13_5p.png"    , rightFrontImagePath = @"..\..\Resource\Image\Tile\tile_13_5p_right.png"     } },
                {15, new TileDatas{kinds = Tile.Kinds.Circle5    , isRed = true , frontImagePath = @"..\..\Resource\Image\Tile\tile_13_5p_red.png", rightFrontImagePath = @"..\..\Resource\Image\Tile\tile_13_5p_red_right.png" } },
                {16, new TileDatas{kinds = Tile.Kinds.Circle6    , isRed = false, frontImagePath = @"..\..\Resource\Image\Tile\tile_14_6p.png"    , rightFrontImagePath = @"..\..\Resource\Image\Tile\tile_14_6p_right.png"     } },
                {17, new TileDatas{kinds = Tile.Kinds.Circle7    , isRed = false, frontImagePath = @"..\..\Resource\Image\Tile\tile_15_7p.png"    , rightFrontImagePath = @"..\..\Resource\Image\Tile\tile_15_7p_right.png"     } },
                {18, new TileDatas{kinds = Tile.Kinds.Circle8    , isRed = false, frontImagePath = @"..\..\Resource\Image\Tile\tile_16_8p.png"    , rightFrontImagePath = @"..\..\Resource\Image\Tile\tile_16_8p_right.png"     } },
                {19, new TileDatas{kinds = Tile.Kinds.Circle9    , isRed = false, frontImagePath = @"..\..\Resource\Image\Tile\tile_17_9p.png"    , rightFrontImagePath = @"..\..\Resource\Image\Tile\tile_17_9p_right.png"     } },
                {20, new TileDatas{kinds = Tile.Kinds.Bamboo1    , isRed = false, frontImagePath = @"..\..\Resource\Image\Tile\tile_18_1s.png"    , rightFrontImagePath = @"..\..\Resource\Image\Tile\tile_18_1s_right.png"     } },
                {21, new TileDatas{kinds = Tile.Kinds.Bamboo2    , isRed = false, frontImagePath = @"..\..\Resource\Image\Tile\tile_19_2s.png"    , rightFrontImagePath = @"..\..\Resource\Image\Tile\tile_19_2s_right.png"     } },
                {22, new TileDatas{kinds = Tile.Kinds.Bamboo3    , isRed = false, frontImagePath = @"..\..\Resource\Image\Tile\tile_20_3s.png"    , rightFrontImagePath = @"..\..\Resource\Image\Tile\tile_20_3s_right.png"     } },
                {23, new TileDatas{kinds = Tile.Kinds.Bamboo4    , isRed = false, frontImagePath = @"..\..\Resource\Image\Tile\tile_21_4s.png"    , rightFrontImagePath = @"..\..\Resource\Image\Tile\tile_21_4s_right.png"     } },
                {24, new TileDatas{kinds = Tile.Kinds.Bamboo5    , isRed = false, frontImagePath = @"..\..\Resource\Image\Tile\tile_22_5s.png"    , rightFrontImagePath = @"..\..\Resource\Image\Tile\tile_22_5s_right.png"     } },
                {25, new TileDatas{kinds = Tile.Kinds.Bamboo5    , isRed = true , frontImagePath = @"..\..\Resource\Image\Tile\tile_22_5s_red.png", rightFrontImagePath = @"..\..\Resource\Image\Tile\tile_22_5s_red_right.png" } },
                {27, new TileDatas{kinds = Tile.Kinds.Bamboo7    , isRed = false, frontImagePath = @"..\..\Resource\Image\Tile\tile_24_7s.png"    , rightFrontImagePath = @"..\..\Resource\Image\Tile\tile_24_7s_right.png"     } },
                {28, new TileDatas{kinds = Tile.Kinds.Bamboo8    , isRed = false, frontImagePath = @"..\..\Resource\Image\Tile\tile_25_8s.png"    , rightFrontImagePath = @"..\..\Resource\Image\Tile\tile_25_8s_right.png"     } },
                {29, new TileDatas{kinds = Tile.Kinds.Bamboo9    , isRed = false, frontImagePath = @"..\..\Resource\Image\Tile\tile_26_9s.png"    , rightFrontImagePath = @"..\..\Resource\Image\Tile\tile_26_9s_right.png"     } },
                {30, new TileDatas{kinds = Tile.Kinds.East       , isRed = false, frontImagePath = @"..\..\Resource\Image\Tile\tile_27_e.png"     , rightFrontImagePath = @"..\..\Resource\Image\Tile\tile_27_e_right.png"      } },
                {31, new TileDatas{kinds = Tile.Kinds.South      , isRed = false, frontImagePath = @"..\..\Resource\Image\Tile\tile_28_s.png"     , rightFrontImagePath = @"..\..\Resource\Image\Tile\tile_28_s_right.png"      } },
                {32, new TileDatas{kinds = Tile.Kinds.West       , isRed = false, frontImagePath = @"..\..\Resource\Image\Tile\tile_29_w.png"     , rightFrontImagePath = @"..\..\Resource\Image\Tile\tile_29_w_right.png"      } },
                {33, new TileDatas{kinds = Tile.Kinds.North      , isRed = false, frontImagePath = @"..\..\Resource\Image\Tile\tile_30_n.png"     , rightFrontImagePath = @"..\..\Resource\Image\Tile\tile_30_n_right.png"      } },
                {34, new TileDatas{kinds = Tile.Kinds.WhiteDragon, isRed = false, frontImagePath = @"..\..\Resource\Image\Tile\tile_31_wd.png"    , rightFrontImagePath = @"..\..\Resource\Image\Tile\tile_31_wd_right.png"     } },
                {35, new TileDatas{kinds = Tile.Kinds.GreenDragon, isRed = false, frontImagePath = @"..\..\Resource\Image\Tile\tile_32_gd.png"    , rightFrontImagePath = @"..\..\Resource\Image\Tile\tile_32_gd_right.png"     } },
                {36, new TileDatas{kinds = Tile.Kinds.RedDragon  , isRed = false, frontImagePath = @"..\..\Resource\Image\Tile\tile_33_rd.png"    , rightFrontImagePath = @"..\..\Resource\Image\Tile\tile_33_rd_right.png"     } }
            };
            string backImagePath = @"..\..\Resource\Image\Tile\tile_any_back.png";
            string rightBackImagePath = @"..\..\Resource\Image\Tile\tile_any_back_right.png";

            /// 選択牌一覧
            for (int i = 0; i < this.TileArray.Count(); i++)
            {
                this.TileArray[i] = new Tile();
                this.TileArray[i].Kind = TileArrayKindsDictionary[i].kinds;
                this.TileArray[i].FrontImage = this.GetImageSource(TileArrayKindsDictionary[i].frontImagePath);
                this.TileArray[i].RightFrontImage = this.GetImageSource(TileArrayKindsDictionary[i].rightFrontImagePath);
                this.TileArray[i].BackImage = this.GetImageSource(backImagePath);
                this.TileArray[i].RightBackImage = this.GetImageSource(rightBackImagePath);
            }
            this.concealedTileNumMax = 13;
            this.concealedTileNum = 0;

            return this.TileArray;
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
        /// Tuple.Item1: 門前手牌
        /// Tuple.Item2: 和了牌
        /// Tuple.Item3: 鳴いた牌
        /// Tuple.Item4: ドラ表示牌
        /// </returns>
        public Tuple<Tile[], Tile, TileSet[], Tile[,]> ClearTiles()
        {
            /// 面前手牌クリア
            for (int i = 0; i < ConcealedTileArray.Count(); i++)
            {
                ConcealedTileArray[i] = null;
            }
            concealedTileNum = 0;

            // 和了牌クリア
            WinningTile = null;

            // TODO: 鳴いた牌クリア処理

            // TODO: ドラ表示牌クリア処理

            return new Tuple<Tile[], Tile, TileSet[], Tile[,]>(this.ConcealedTileArray, this.WinningTile, this.MeldedTileSetArray, this.DoraIndicatorArray);
        }

        /// <summary>
        /// 和了牌のクリア
        /// </summary>
        /// <returns>クリア後の和了牌(=null)</returns>
        public Tile ClearWinningTile()
        {
            this.WinningTile = null;
            return this.WinningTile;
        }

        /// <summary>
        /// 面前手牌に牌を追加する
        /// </summary>
        /// <param name="addedTile">追加される牌</param>
        /// <returns>
        /// Tuple.Item1: 牌追加が成功したか  true:成功  false:失敗
        /// Tuple.Item2: 追加後の門前手牌
        /// </returns>
        public Tuple<bool, Tile[]> AddConcealedTile(Tile addedTile)
        {
            bool isSucceeded;
            if (this.concealedTileNum < this.concealedTileNumMax)
            {
                // TODO: 枚数上限チェック

                /// 手配に追加
                this.ConcealedTileArray[this.concealedTileNum] = addedTile;
                Array.Sort(ConcealedTileArray, 0, this.concealedTileNum + 1, new Tile.TileIndexCompare());
                this.concealedTileNum++;

                isSucceeded = true;
            }
            else
            {
                isSucceeded = false;
            }

            return new Tuple<bool, Tile[]>(isSucceeded, this.ConcealedTileArray);
        }

        /// <summary>
        /// 和了牌を設定する
        /// </summary>
        /// <param name="drawnTile">新たな和了牌</param>
        public Tile DrawWinningTile(Tile drawnTile)
        {
            // TODO: 枚数上限チェック

            this.WinningTile = drawnTile;
            return this.WinningTile;
        }

        /// <summary>
        /// 鳴いた面子を追加する
        /// </summary>
        /// <param name="addedTileSet">追加される面子</param>
        /// <returns>
        /// Tuple.Item1: 面子追加が成功したか
        /// Tuple.Item2: 追加後の面子
        /// </returns>
        public Tuple<bool, TileSet[]> AddMeldedTileSet(int topTileArrayIndex, TileSet.Kinds tileSetKind)
        {
            if (topTileArrayIndex < 0 || this.TileArray.Count() <= topTileArrayIndex )
            {
                throw new ArgumentOutOfRangeException(nameof(topTileArrayIndex), string.Format("Argument '{0}' is out of range.", nameof(topTileArrayIndex)));
            }

            if (this.concealedTileNum + 3 >= this.concealedTileNumMax || this.MeldedTileSetArray.Count() >= 4)
            {
                return new Tuple<bool, TileSet[]>(false, this.MeldedTileSetArray);
            }

            bool isSucceeded = false;

            switch (tileSetKind)
            {
                case TileSet.Kinds.Pung:
                    {
                        // TODO: 枚数上限チェック

                        TileSet ts = new TileSet();
                        Tile[] t = new Tile[] { this.TileArray[topTileArrayIndex], this.TileArray[topTileArrayIndex], this.TileArray[topTileArrayIndex] };
                        ts.Kind = tileSetKind;
                        ts.Tiles = t;
                        isSucceeded = true;
                    }
                    break;
                case TileSet.Kinds.Chow:
                    if (this.TileArray[topTileArrayIndex].IsSuit)
                    {
                        // TODO: 枚数上限チェック

                        TileSet ts = new TileSet();
                        Tile[] t = this.GetChowTiles(topTileArrayIndex);
                        ts.Kind = tileSetKind;
                        ts.Tiles = t;
                        isSucceeded = true;
                    }
                    else
                    {
                        throw new ArgumentException(
                            string.Format(
                                "Argument '{0}' must be suit tile if argument '{1}' is {2}.",
                                nameof(topTileArrayIndex), nameof(tileSetKind), tileSetKind));
                    }
                    break;
                case TileSet.Kinds.ConcealedKong:
                case TileSet.Kinds.MeldedKongFromDiscard:
                case TileSet.Kinds.MeldedKongFromHand:
                    {
                        // TODO: 枚数上限チェック

                        TileSet ts = new TileSet();
                        Tile[] t = new Tile[] { this.TileArray[topTileArrayIndex], this.TileArray[topTileArrayIndex], this.TileArray[topTileArrayIndex], this.TileArray[topTileArrayIndex] };
                        ts.Kind = tileSetKind;
                        ts.Tiles = t;
                        isSucceeded = true;
                    }
                    break;
                case TileSet.Kinds.Pair:
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(tileSetKind),
                        string.Format(
                            "Argument '{0}' must be {1}, {2}, {3}, {4} or {5}.",
                            nameof(tileSetKind), TileSet.Kinds.Pung, TileSet.Kinds.Chow, TileSet.Kinds.ConcealedKong,
                            TileSet.Kinds.MeldedKongFromDiscard, TileSet.Kinds.MeldedKongFromHand));
            }

            if (isSucceeded)
            {
                this.concealedTileNumMax -= 3;
            }

            return new Tuple<bool, TileSet[]>(isSucceeded, this.MeldedTileSetArray);
        }

        /// <summary>
        /// チーの面子を取得する。
        /// 条件は以下の通り。
        /// ・this.TileArray[<paramref name="index"/>].Numberが1～6であれば、その牌から始まる3つの牌を返す
        ///   ただし、2～3番目の牌は赤牌を含まない
        /// ・this.TileArray[<paramref name="index"/>].Numberが7～9であれば、7～9の3つの牌を返す
        /// </summary>
        /// <param name="index">選択されたTileArray配列のインデックス</param>
        /// <returns><paramref name="index"/>に対応するチーの3牌</returns>
        private Tile[] GetChowTiles(int index)
        {
            Tile[] tiles;

            if (this.TileArray[index].Number <= 7)
            {
                tiles = new Tile[3];
                tiles[0] = this.TileArray[index];

                int skip = 0;
                for (int i = 1; i < 3; i++)
                {
                    if (this.TileArray[index + i + skip].IsRed)
                    {
                        skip++;
                    }
                    tiles[i] = this.TileArray[index + i + skip];
                }
            }
            else if (this.TileArray[index].Number == 8)
            {
                tiles = new Tile[] { this.TileArray[index - 1], this.TileArray[index], this.TileArray[index + 1] };
            }
            else if (this.TileArray[index].Number == 9)
            {
                tiles = new Tile[] { this.TileArray[index - 2], this.TileArray[index - 1], this.TileArray[index] };
            }
            else
            {
                throw new Exception("実装ミス");
            }

            return tiles;
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
            this.DoraIndicatorArray[row, column] = newTile;
            return this.DoraIndicatorArray;
        }

        /// <summary>
        /// 手牌を1枚捨てる
        /// </summary>
        /// <param name="index">捨て牌の手牌インデックス</param>
        public Tile[] DiscardConcealedTile(int index)
        {
            if (index < 0 || ConcealedTileArray.Count() <= index)
            {
                throw new ArgumentOutOfRangeException(nameof(index), string.Format("Argument '{0}'({1}) is out of range.", nameof(index), index));
            }

            for (int i = index; i < ConcealedTileArray.Count() - 1; i++)
            {
                ConcealedTileArray[i] = ConcealedTileArray[i + 1];
            }
            ConcealedTileArray[ConcealedTileArray.Count() - 1] = null;
            this.concealedTileNum--;

            return this.ConcealedTileArray;
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
