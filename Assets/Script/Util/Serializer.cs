using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Util {
    public static class Serializer {

        public static object LoadFromBinaryFile(string path) {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read)) {
                var bf = new BinaryFormatter();
                object obj = bf.Deserialize(fs);
                fs.Close();
                return obj;
            }
        }

        public static void SaveToBinaryFile(object obj,string directory,string filename ,string extention) {
            var path = CheckExists(directory, filename, extention);
            using (var fs = new FileStream(Path.Combine(directory,path), FileMode.Create, FileAccess.Write)) {
                var bf = new BinaryFormatter();
                bf.Serialize(fs, obj);
                fs.Close();
            }
        }

        private static string CheckExists(string directory, string filename,string extention) {
            var di = new DirectoryInfo(directory);
            var max = di.GetFiles(filename + "_???" + extention)                    // パターンに一致するファイルを取得する
                .Select(fi => Regex.Match(fi.Name, @"(?i)_(\d{3})\" + extention))   // ファイルの中で数値のものを探す
                .Where(m => m.Success)                                              // 該当するファイルだけに絞り込む
                .Select(m => Int32.Parse(m.Groups[1].Value))                        // 数値を取得する
                .DefaultIfEmpty(0)                                                  // １つも該当しなかった場合は 0 とする
                .Max();                                                             // 最大値を取得する
            return String.Format("{0}_{1:d3}" + extention, filename, max + 1);      //https://ja.stackoverflow.com/questions/4312
        }
    }
}
