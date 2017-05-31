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
            get { return _tileArray; }
            set { SetProperty(ref _tileArray, value); }
        }
        private Tile[] _tileArray;

        /// <summary>
        /// コンストラクタ
        /// ・変数の初期化
        /// </summary>
        public MainViewModel()
        {
            TileArray = new Tile[37];
            for (int i = 0; i < TileArray.Count(); i++)
            {
                TileArray[i] = new Tile();
            }
            TileArray[ 0].Kind = Tile.Kinds.Character1;  TileArray[ 0].FrontImage = this.GetImageSource(@"..\..\Resource\Image\Tile\tile_00_1m.png");
            TileArray[ 1].Kind = Tile.Kinds.Character2;  TileArray[ 1].FrontImage = this.GetImageSource(@"..\..\Resource\Image\Tile\tile_01_2m.png");
            TileArray[ 2].Kind = Tile.Kinds.Character3;  TileArray[ 2].FrontImage = this.GetImageSource(@"..\..\Resource\Image\Tile\tile_02_3m.png");
            TileArray[ 3].Kind = Tile.Kinds.Character4;  TileArray[ 3].FrontImage = this.GetImageSource(@"..\..\Resource\Image\Tile\tile_03_4m.png");
            TileArray[ 4].Kind = Tile.Kinds.Character5;  TileArray[ 4].FrontImage = this.GetImageSource(@"..\..\Resource\Image\Tile\tile_04_5m.png");
            TileArray[ 5].Kind = Tile.Kinds.Character5;  TileArray[ 5].FrontImage = this.GetImageSource(@"..\..\Resource\Image\Tile\tile_04_5m_r.png");
            TileArray[ 6].Kind = Tile.Kinds.Character6;  TileArray[ 6].FrontImage = this.GetImageSource(@"..\..\Resource\Image\Tile\tile_05_6m.png");
            TileArray[ 7].Kind = Tile.Kinds.Character7;  TileArray[ 7].FrontImage = this.GetImageSource(@"..\..\Resource\Image\Tile\tile_06_7m.png");
            TileArray[ 8].Kind = Tile.Kinds.Character8;  TileArray[ 8].FrontImage = this.GetImageSource(@"..\..\Resource\Image\Tile\tile_07_8m.png");
            TileArray[ 9].Kind = Tile.Kinds.Character9;  TileArray[ 9].FrontImage = this.GetImageSource(@"..\..\Resource\Image\Tile\tile_08_9m.png");
            TileArray[10].Kind = Tile.Kinds.Circle1;     TileArray[10].FrontImage = this.GetImageSource(@"..\..\Resource\Image\Tile\tile_09_1p.png");
            TileArray[11].Kind = Tile.Kinds.Circle2;     TileArray[11].FrontImage = this.GetImageSource(@"..\..\Resource\Image\Tile\tile_10_2p.png");
            TileArray[12].Kind = Tile.Kinds.Circle3;     TileArray[12].FrontImage = this.GetImageSource(@"..\..\Resource\Image\Tile\tile_11_3p.png");
            TileArray[13].Kind = Tile.Kinds.Circle4;     TileArray[13].FrontImage = this.GetImageSource(@"..\..\Resource\Image\Tile\tile_12_4p.png");
            TileArray[14].Kind = Tile.Kinds.Circle5;     TileArray[14].FrontImage = this.GetImageSource(@"..\..\Resource\Image\Tile\tile_13_5p.png");
            TileArray[15].Kind = Tile.Kinds.Circle5;     TileArray[15].FrontImage = this.GetImageSource(@"..\..\Resource\Image\Tile\tile_13_5p_r.png");
            TileArray[16].Kind = Tile.Kinds.Circle6;     TileArray[16].FrontImage = this.GetImageSource(@"..\..\Resource\Image\Tile\tile_14_6p.png");
            TileArray[17].Kind = Tile.Kinds.Circle7;     TileArray[17].FrontImage = this.GetImageSource(@"..\..\Resource\Image\Tile\tile_15_7p.png");
            TileArray[18].Kind = Tile.Kinds.Circle8;     TileArray[18].FrontImage = this.GetImageSource(@"..\..\Resource\Image\Tile\tile_16_8p.png");
            TileArray[19].Kind = Tile.Kinds.Circle9;     TileArray[19].FrontImage = this.GetImageSource(@"..\..\Resource\Image\Tile\tile_17_9p.png");
            TileArray[20].Kind = Tile.Kinds.Bamboo1;     TileArray[20].FrontImage = this.GetImageSource(@"..\..\Resource\Image\Tile\tile_18_1s.png");
            TileArray[21].Kind = Tile.Kinds.Bamboo2;     TileArray[21].FrontImage = this.GetImageSource(@"..\..\Resource\Image\Tile\tile_19_2s.png");
            TileArray[22].Kind = Tile.Kinds.Bamboo3;     TileArray[22].FrontImage = this.GetImageSource(@"..\..\Resource\Image\Tile\tile_20_3s.png");
            TileArray[23].Kind = Tile.Kinds.Bamboo4;     TileArray[23].FrontImage = this.GetImageSource(@"..\..\Resource\Image\Tile\tile_21_4s.png");
            TileArray[24].Kind = Tile.Kinds.Bamboo5;     TileArray[24].FrontImage = this.GetImageSource(@"..\..\Resource\Image\Tile\tile_22_5s.png");
            TileArray[25].Kind = Tile.Kinds.Bamboo5;     TileArray[25].FrontImage = this.GetImageSource(@"..\..\Resource\Image\Tile\tile_22_5s_r.png");
            TileArray[26].Kind = Tile.Kinds.Bamboo6;     TileArray[26].FrontImage = this.GetImageSource(@"..\..\Resource\Image\Tile\tile_23_6s.png");
            TileArray[27].Kind = Tile.Kinds.Bamboo7;     TileArray[27].FrontImage = this.GetImageSource(@"..\..\Resource\Image\Tile\tile_24_7s.png");
            TileArray[28].Kind = Tile.Kinds.Bamboo8;     TileArray[28].FrontImage = this.GetImageSource(@"..\..\Resource\Image\Tile\tile_25_8s.png");
            TileArray[29].Kind = Tile.Kinds.Bamboo9;     TileArray[29].FrontImage = this.GetImageSource(@"..\..\Resource\Image\Tile\tile_26_9s.png");
            TileArray[30].Kind = Tile.Kinds.East;        TileArray[30].FrontImage = this.GetImageSource(@"..\..\Resource\Image\Tile\tile_27_e.png");
            TileArray[31].Kind = Tile.Kinds.South;       TileArray[31].FrontImage = this.GetImageSource(@"..\..\Resource\Image\Tile\tile_28_s.png");
            TileArray[32].Kind = Tile.Kinds.West;        TileArray[32].FrontImage = this.GetImageSource(@"..\..\Resource\Image\Tile\tile_29_w.png");
            TileArray[33].Kind = Tile.Kinds.North;       TileArray[33].FrontImage = this.GetImageSource(@"..\..\Resource\Image\Tile\tile_30_n.png");
            TileArray[34].Kind = Tile.Kinds.WhiteDragon; TileArray[34].FrontImage = this.GetImageSource(@"..\..\Resource\Image\Tile\tile_31_wd.png");
            TileArray[35].Kind = Tile.Kinds.GreenDragon; TileArray[35].FrontImage = this.GetImageSource(@"..\..\Resource\Image\Tile\tile_32_gd.png");
            TileArray[36].Kind = Tile.Kinds.RedDragon;   TileArray[36].FrontImage = this.GetImageSource(@"..\..\Resource\Image\Tile\tile_33_rd.png");
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
