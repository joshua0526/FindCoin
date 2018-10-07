using FindCoin.core;
using FindCoin.Mysql;
using FindCoin.thinneo;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FindCoin.Block
{
    class SaveBlock : ISave
    {
        private static SaveBlock instance = null;
        public  static SaveBlock getInstance() {
            if (instance == null) {
                return new SaveBlock();
            }
            return instance;
        }

        public override void Save(JToken jObject, string path)
        {
            JObject result = new JObject();
            result["hash"] = jObject["hash"];
            result["size"] = jObject["size"];
            result["version"] = jObject["version"];
            result["previousblockhash"] = jObject["previousblockhash"];
            result["merkleroot"] = jObject["merkleroot"];
            result["time"] = jObject["time"];
            result["index"] = jObject["index"];
            result["nonce"] = jObject["nonce"];
            result["nextconsensus"] = jObject["nextconsensus"];
            result["script"] = jObject["script"];

            List<string> slist = new List<string>();
            slist.Add(jObject["hash"].ToString());
            slist.Add(jObject["size"].ToString());
            slist.Add(jObject["version"].ToString());
            slist.Add(jObject["previousblockhash"].ToString());
            slist.Add(jObject["merkleroot"].ToString());
            slist.Add(jObject["time"].ToString());
            slist.Add(jObject["index"].ToString());
            slist.Add(jObject["nonce"].ToString());
            slist.Add(jObject["nextconsensus"].ToString());
            slist.Add(jObject["script"].ToString());
            MysqlConn.ExecuteDataInsert("block", slist);

            Helper.blockTime = int.Parse(result["time"].ToString());

            File.Delete(path);
            File.WriteAllText(path, result.ToString(), Encoding.UTF8);

            foreach (var tx in jObject["tx"])
            {
                var txPath = "transaction" + Path.DirectorySeparatorChar + result["hash"] + ".txt";
                SaveTransaction.getInstance().Save(tx as JObject, txPath);
            }
        }
    }
}
