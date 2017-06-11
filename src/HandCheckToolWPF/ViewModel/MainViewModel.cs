using MyMahjong;
using HandCheckToolWPF.Model;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HandCheckToolWPF.ViewModel
{
    class MainViewModel : BindableBase
    {
        #region 型定義
        /// <summary>
        /// 牌選択モード
        /// </summary>
        public enum TileSelectionMode
        {
            ConcealedTile,  // 手牌選択
            WinningTile,    // 和了牌選択
            OpenedTile,     // 鳴き牌選択
            DoraIndicator   // ドラ表示牌選択
        }

        /// <summary>
        /// ドラ表示牌選択モード
        /// </summary>
        public enum DoraIndicatorSelectionMode
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
        /// 【Bindingコマンド】
        /// タイル削除コマンド
        /// </summary>
        public ICommand ClearTilesCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    var changedValues = this.mainModel.ClearTiles();

                    this.ConcealedTileArray = changedValues.Item1;
                    this.WinningTile = changedValues.Item2;
                    //this. = changedValues.Item3;
                    this.DoraIndicatorArray = changedValues.Item4;

                    RaisePropertyChanged(nameof(this.ConcealedTileArray));
                    RaisePropertyChanged(nameof(this.WinningTile));
                    //RaisePropertyChanged(nameof(this.));
                    RaisePropertyChanged(nameof(this.DoraIndicatorArray));
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
                    if (ret)
                    {
                        System.Windows.MessageBox.Show("聴牌！");
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("不聴...");
                    }
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
                    bool ret = MahjongLogicUtility.IsWonHand(this.ConcealedTileArray, new TileSet[0], this.WinningTile);
                    if (ret)
                    {
                        System.Windows.MessageBox.Show("和了！");
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("錯和...");
                    }
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
                        throw new ArgumentOutOfRangeException(string.Format("Argument'{0}'({1}) is out of range.\r\nRange is [{2},{3}).", nameof(index), index, 0, TileArray.Count()));
                    }

                    /// 牌選択モードによって牌セット先を変更
                    switch (this.tileSelectionMode)
                    {
                        case TileSelectionMode.ConcealedTile:
                            {
                                var ret = this.mainModel.AddConcealedTile(TileArray[index]);
                                if (ret.Item1)
                                {
                                    this.ConcealedTileArray = ret.Item2;
                                    RaisePropertyChanged(nameof(this.ConcealedTileArray));
                                }
                                else
                                {
                                    System.Windows.MessageBox.Show("手配枚数が上限に達しています。", "操作NG", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                                }
                            }
                            break;
                        case TileSelectionMode.WinningTile:
                            this.WinningTile = this.mainModel.DrawWinningTile(TileArray[index]);
                            RaisePropertyChanged(nameof(this.WinningTile));
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

                    this.ConcealedTileArray = this.mainModel.DiscardConcealedTile(index);

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
                    this.WinningTile = this.mainModel.ClearWinningTile();
                    RaisePropertyChanged(nameof(this.WinningTile));
                });
            }
        }
        #endregion

        /// <summary>
        /// モデルクラスのインスタンス
        /// </summary>
        private Model.MainModel mainModel;

        /// <summary>
        /// 牌選択モード
        /// </summary>
        private TileSelectionMode tileSelectionMode;

        /// <summary>
        /// コンストラクタ
        /// ・変数の初期化
        /// </summary>
        public MainViewModel()
        {
            this.mainModel = new Model.MainModel();

            /// TileArrayメンバ初期化
            this.TileArray = this.mainModel.InitializeTileArray();

            /// ドラ表示牌の初期化
            this.DoraIndicatorArray = this.mainModel.doraIndicatorArray;
            for (int i = 0; i < this.DoraIndicatorArray.GetLength(0); i++)
            {
                for (int j = 0; j < this.DoraIndicatorArray.GetLength(1); j++)
                {
                    this.mainModel.SetDoraIndicator(i, j, null);
                }
            }
            this.DoraIndicatorArray = this.mainModel.doraIndicatorArray;

            /// 牌選択モードの初期化
            this.InitializeSelectingTileMode();
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
    }
}
