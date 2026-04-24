using System;
using System.Text.RegularExpressions;
using System.IO;

public class CSVDateLoader
{
    /// <summary> CSVデータを2次元配列に変換する関数 </summary>
    /// <param name="csvData">  取得したCSVのデータ </param>
    /// <returns> 取得したCSVデータを格納した2次元配列 </returns>
    public string[,] ParseCsv(string csvData)
    {
        // 行ごとに分割（\r\n か \n を考慮）
        string[] rows = csvData.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        // 1行目の列数を基準にする
        string[] firstRow = SplitCsvLine(rows[0]);
        int rowCount = rows.Length;
        int colCount = firstRow.Length;

        // 2次元配列を作成
        string[,] result = new string[rowCount, colCount];

        for (int i = 0; i < rowCount; i++)
        {
            string[] cols = SplitCsvLine(rows[i]);

            for (int j = 0; j < colCount; j++)
            {
                // 配列の範囲を超えた場合は空文字をセット
                result[i, j] = j < cols.Length ? cols[j] : "";
            }
        }

        return result;
    }

    /// <summary> CSVの1行を分割する関数 </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    private string[] SplitCsvLine(string line)
    {
        // 正規表現でCSVのフィールドを抽出
        MatchCollection matches = Regex.Matches(line, "\"([^\"]*)\"|([^,]+)");

        string[] fields = new string[matches.Count];

        for (int i = 0; i < matches.Count; i++)
        {
            fields[i] = matches[i].Value.Trim('"'); // ダブルクォートを削除
        }

        return fields;
    }

    /// <summary> 外部から生成したCSVファイルを2次元配列でロードする関数 </summary>
    /// <param name="filePath"> 生成先のPath </param>
    public string[,] LoadCsvAs2DArray(string filePath)
    {
        string[] lines = File.ReadAllLines(filePath); // 行ごとに読み込む
        int rows = lines.Length;
        int cols = lines[0].Split(',').Length; // 1行目の列数を基準にする
        string[,] array = new string[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            string[] cells = lines[i].Split(','); // カンマ区切りで分割
            for (int j = 0; j < cols; j++)
            {
                array[i, j] = cells[j].Trim('\"'); // 余分な " を削除
            }
        }

        return array;
    }
}
