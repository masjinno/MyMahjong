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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HandCheckToolWPF.ViewModel
{
    class MainViewModel : BindableBase
    {
        #region 型定義
        /// <summary>
        /// 牌選択モード
        /// </summary>
        enum TileSelectionMode
        {
            ConcealedTile,  // 手牌選択
            WinningTile,    // 和了牌選択
            OpenedTile,     // 鳴き牌選択
            DoraIndicator   // ドラ表示牌選択
        }

        /// <summary>
        /// ドラ表示牌選択モード
        /// </summary>
        enum DoraIndicatorSelectionMode
        {
            Obverse,    // 表ドラ
            Reverse     // 裏ドラ
        }
        #endregion

        #region Bindingプロパティ
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
        /// 【Bindingプロパティ】
        /// 和了牌
        /// </summary>
        public Tile WinningTile
        {
            get { return this._winningTile; }
            set { SetProperty(ref this._winningTile, value); }
        }
        private Tile _winningTile;

        /// <summary>
        /// 【Bindingプロパティ】
        /// 門前手牌
        /// </summary>
        public Tile[] ConcealedTileArray
        {
            get { return this._concealedTileArray; }
            set { SetProperty(ref this._concealedTileArray, value); }
        }
        private Tile[] _concealedTileArray;

        /// <summary>
        /// 【Bindingプロパティ】
        /// ドラ表示牌
        /// </summary>
        public Tile[,] DoraIndicatorArray
        {
            get { return this._doraIndicatorArray; }
            set { SetProperty(ref this._doraIndicatorArray, value); }
        }
        private Tile[,] _doraIndicatorArray;

        /// <summary>
        /// 【Bindingプロパティ】
        /// 門前牌選択モードか
        /// </summary>
        public bool IsSelectingConcealedTileMode
        {
            get { return this._isSelectingConcealedTileMode; }
            set
            {
                if (value)
                {
                    tileSelectionMode = TileSelectionMode.ConcealedTile;
                }
                SetProperty(ref this._isSelectingConcealedTileMode, value);
            }
        }
        private bool _isSelectingConcealedTileMode;

        /// <summary>
        /// 【Bindingプロパティ】
        /// 和了牌選択モードか
        /// </summary>
        public bool IsSelectingWinningTileMode
        {
            get { return this._isSelectingWinningTileMode; }
            set
            {
                if (value)
                {
                    tileSelectionMode = TileSelectionMode.WinningTile;
                }
                SetProperty(ref this._isSelectingWinningTileMode, value);
            }
        }
        private bool _isSelectingWinningTileMode;

        /// <summary>
        /// 【Bindingプロパティ】
        /// 鳴き牌選択モードか
        /// </summary>
        public bool IsSelectingOpenedTileMode
        {
            get { return this._isSelectingOpenedTileMode; }
            set
            {
                if (value)
                {
                    tileSelectionMode = TileSelectionMode.OpenedTile;
                }
                SetProperty(ref this._isSelectingOpenedTileMode, value);
            }
        }
        private bool _isSelectingOpenedTileMode;

        /// <summary>
        /// 【Bindingプロパティ】
        /// ドラ表示牌選択モードか
        /// </summary>
        public bool IsSelectingDoraIndicatorMode
        {
            get { return this._isSelectingDoraIndicatorMode; }
            set
            {
                if (value)
                {
                    tileSelectionMode = TileSelectionMode.DoraIndicator;
                }
                SetProperty(ref this._isSelectingDoraIndicatorMode, value);
            }
        }
        private bool _isSelectingDoraIndicatorMode;
        #endregion

        #region Bindingコマンド
        /// <summary>
        /// タイル削除コマンド
        /// </summary>
        public ICommand ClearTilesCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    this.ClearTiles();
                });
            }
        }

        /// <summary>
        /// 【Bindingコマンド】
        /// 聴牌判定コマンド
        /// </summary>
        public ICommand CheckWaitingHandCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    bool ret = MahjongLogicUtility.IsWaitingHand(ConcealedTileArray, new TileSet[0]);
                    System.Windows.MessageBox.Show(ret.ToString());
                });
            }
        }

        /// <summary>
        /// 【Bindingコマンド】
        /// 和了宣言コマンド
        /// </summary>
        public ICommand DeclareWinCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    bool ret = MahjongLogicUtility.IsWonHand(ConcealedTileArray, new TileSet[0], WinningTile);
                    System.Windows.MessageBox.Show(ret.ToString());
                });
            }
        }

        /// <summary>
        /// 【Bindingコマンド】
        /// 牌ボタン選択コマンド
        /// </summary>
        public ICommand SelectTileCommand
        {
            get
            {
                return new DelegateCommand<object>((object parameter) =>
                {
                    int index;  /// 選択された牌の、牌一覧におけるインデックス

                    /// コマンドパラメータからインデックスを取得
                    if (!int.TryParse(parameter as string, out index))
                    {
                        throw new ArgumentException(string.Format("To parse argument'{0}'({1}) as int failed.", nameof(parameter), parameter));
                    }

                    /// インデックスのバリデーション
                    if (index < 0 || TileArray.Count() <= index)
                    {
                        throw new ArgumentOutOfRangeException(string.Format("Argument'{0}'({1}) is out of range.\r\nRange is [{2},{3}).", nameof(parameter), index, 0, TileArray.Count()));
                    }

                    /// 牌選択モードによって牌セット先を変更
                    switch (tileSelectionMode)
                    {
                        case TileSelectionMode.ConcealedTile:
                            AddConcealedTile(TileArray[index]);
                            break;
                        case TileSelectionMode.WinningTile:
                            DrawWinningTile(TileArray[index]);
                            break;
                        case TileSelectionMode.OpenedTile:
                            // TODO: 鳴き牌選択処理実装
                            break;
                        case TileSelectionMode.DoraIndicator:
                            // TODO: ドラ表示牌選択処理実装
                            break;
                        default:
                            throw new Exception("実装ミス");
                    }
                });
            }
        }

        /// <summary>
        /// 【Bindingコマンド】
        /// 手牌を捨てるコマンド
        /// </summary>
        public ICommand DiscardCommand
        {
            get
            {
                return new DelegateCommand<object>((object parameter) =>
                {
                    int index;  /// 選択された牌の、手牌におけるインデックス

                    /// コマンドパラメータからインデックスを取得
                    if (!int.TryParse(parameter as string, out index))
                    {
                        throw new ArgumentException(string.Format("To parse argument'{0}'({1}) as int failed.", nameof(parameter), parameter));
                    }

                    this.DiscardConcealedTile(index);

                    RaisePropertyChanged(nameof(this.ConcealedTileArray));
                });
            }
        }

        /// <summary>
        /// 和了牌削除コマンド
        /// </summary>
        public ICommand RemoveWinningTileCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    this.WinningTile = null;
                    RaisePropertyChanged(nameof(this.WinningTile));
                });
            }
        }
        #endregion

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
        /// 配選択モード
        /// </summary>
        private TileSelectionMode tileSelectionMode;

        /// <summary>
        /// コンストラクタ
        /// ・変数の初期化
        /// </summary>
        public MainViewModel()
        {
            /// TileArrayメンバ初期化
            this.InitializeTileArray();

            /// 手牌の初期化
            concealedTileNumMax = 13;
            concealedTileNum = 0;
            this.ConcealedTileArray = new Tile[13];

            /// ドラ表示牌の初期化
            this.DoraIndicatorArray = new Tile[2, 5];
            for (int i = 0; i < this.DoraIndicatorArray.GetLength(0); i++)
            {
                for (int j = 0; j < this.DoraIndicatorArray.GetLength(1); j++)
                {
                    /// 
                    this.DoraIndicatorArray[i, j] = this.TileArray[0];
                }
            }

            /// 牌選択モードの初期化
            InitializeSelectingTileMode();
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
        /// 牌をクリアする。
        /// 対象は以下の牌
        /// ・面前手牌
        /// ・和了牌
        /// ・鳴いた牌
        /// ・ドラ表示牌
        /// </summary>
        private void ClearTiles()
        {
            /// 面前手牌クリア
            for (int i = 0; i < ConcealedTileArray.Count(); i++)
            {
                ConcealedTileArray[i] = null;
            }
            RaisePropertyChanged(nameof(this.ConcealedTileArray));

            // 和了牌クリア
            WinningTile = null;
            RaisePropertyChanged(nameof(this.WinningTile));

            // TODO: 鳴いた牌クリア処理

            // TODO: ドラ表示牌クリア処理

        }

        /// <summary>
        /// 牌選択モードの初期化
        /// </summary>
        private void InitializeSelectingTileMode()
        {
            this.IsSelectingConcealedTileMode = true;
            this.IsSelectingWinningTileMode = false;
            this.IsSelectingOpenedTileMode = false;
            this.IsSelectingDoraIndicatorMode = false;
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

        /// <summary>
        /// 面前手牌に牌を追加する
        /// </summary>
        /// <param name="addedTile">追加される牌</param>
        private void AddConcealedTile(Tile addedTile)
        {
            if (this.concealedTileNum < this.concealedTileNumMax)
            {
                /// 手配に追加
                this.ConcealedTileArray[this.concealedTileNum] = addedTile;
                //System.Collections.IComparer a = new delegate((a, b) => a.Index - b.Index);
                Array.Sort(ConcealedTileArray, 0, this.concealedTileNum + 1, new Tile.TileIndexCompare());
                RaisePropertyChanged(nameof(ConcealedTileArray));
                this.concealedTileNum++;
            }
            else
            {
                System.Windows.MessageBox.Show("手配枚数が上限に達しています。", "操作NG", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// 和了牌を設定する
        /// </summary>
        /// <param name="drawnTile">新たな和了牌</param>
        private void DrawWinningTile(Tile drawnTile)
        {
            WinningTile = drawnTile;
        }

        /// <summary>
        /// 手牌を1枚捨てる
        /// </summary>
        /// <param name="index">捨て牌の手牌インデックス</param>
        private void DiscardConcealedTile(int index)
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
        }
    }
}
