using FindCoin.core;
using FindCoin.thinneo;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FindCoin.Block
{
    class SaveUTXO:ISave
    {
        private static SaveUTXO instance = null;
        public static SaveUTXO getInstance()
        {
            if (instance == null)
            {
                return new SaveUTXO();
            }
            return instance;
        }

        public override void Save(JToken jObject, string path)
        {
            foreach (JObject vout in jObject["vout"]) {
                JObject result = new JObject();
                result["addr"] = vout["address"];
                result["txid"] = jObject["txid"];
                result["n"] = vout["n"];
                result["asset"] = vout["asset"];
                result["value"] = vout["value"];
                result["createHeight"] = Helper.blockHeight;
                result["used"] = 0;
                result["useHeight"] = "";
                result["claimed"] = "";

                var utxoPath = "utxo" + Path.DirectorySeparatorChar + result["txid"] + "_" + result["n"] + ".txt";
                File.Delete(utxoPath);
                File.WriteAllText(utxoPath, result.ToString(), Encoding.UTF8);
            }
            foreach (JObject vin in jObject["vin"]) {
                var inPath = "utxo" + Path.DirectorySeparatorChar + vin["txid"] + "_" + vin["vout"] + ".txt";
                ChangeUTXO(inPath);
            }
        }

        public void ChangeUTXO(string path) {
            JObject result = JObject.Parse(File.ReadAllText(path, Encoding.UTF8));
            result["used"] = 1;
            result["useHeight"] = Helper.blockHeight;
            File.WriteAllText(path, result.ToString(), Encoding.UTF8);
        }
    }
}
