using System;
using System.IO; // ファイル入出力用の名前空間
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting; // Chartクラスの名前空間

namespace Plotter {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
            this.Load += new EventHandler(Form1_Load); // 追加
        }

        private void Form1_Load(object sender, EventArgs e) {
            // Chartコントロールが既にフォームに追加されている前提です

            // Chartの設定をクリア
            chart1.ChartAreas.Clear();
            chart1.Series.Clear();

            // ChartAreaを作成
            ChartArea chartArea = new ChartArea("誤差");
            chart1.ChartAreas.Add(chartArea);

            // Seriesを作成
            Series series = new Series("誤差");

            // Seriesの種類をポイントプロット（散布図）に設定
            series.ChartType = SeriesChartType.Point;
            series.MarkerStyle = MarkerStyle.Circle; // マーカーを円に設定

            // テキストファイルからデータを読み込む
            string filePath = Path.Combine("..", "..", "..", "data.txt");

            string graphName = ""; // グラフの名前を保存する変数
            try {
                string[] lines = File.ReadAllLines(filePath); // ファイルの全行を読み込み

                // 1行目をグラフ名として取得
                if (lines.Length > 0) {
                    graphName = lines[0]; // 1行目を取得
                }

                // 2行目以降のデータを読み込む
                for (int i = 1; i < lines.Length; i++) { // 1行目をスキップ
                    if (double.TryParse(lines[i], out double error)) {
                        // データポイントを追加
                        series.Points.AddXY(i, error); // i-1でエポック数を合わせる
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show($"データの読み込み中にエラーが発生しました: {ex.Message}");
            }

            // SeriesをChartに追加
            chart1.Series.Add(series);

            // 軸ラベルの設定
            chart1.ChartAreas[0].AxisX.Title = "エポック数";
            chart1.ChartAreas[0].AxisY.Title = "誤差";

            // グラフを保存する
            SaveChartImage(@".\..\..\..\" + graphName + ".png"); // 保存するファイル名を指定
        }

        private void SaveChartImage(string filePath) {
            try {
                chart1.SaveImage(filePath, ChartImageFormat.Jpeg); // 画像として保存
                MessageBox.Show($"グラフが保存されました。");
            }
            catch (Exception ex) {
                MessageBox.Show($"グラフの保存中にエラーが発生しました: {ex.Message}");
            }
        }
    }
}
