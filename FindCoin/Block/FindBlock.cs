using FindCoin.core;
using FindCoin.helper;
using FindCoin.thinneo;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace FindCoin.Block
{
    class FindBlock : ContractTask
    {
        private JObject config;

        public FindBlock(string name) : base(name) {

        }

        public override void initConfig(JObject config)
        {
            this.config = config;
            initConfig();
        }

        public override void startTask()
        {
            run();
        }

        private int batchInterval = 20;
        private void initConfig() {

        }

        private void run() {
            while (true) {
                ping();



                Helper.blockHeight++;
            }
        }

        static WebClient wc = new WebClient();
        private void getBlockFromRpc() {
            var getcounturl = getUrl() + "?jsonrpc=2.0&id=1&method=getblock&params=[" + Helper.blockHeight + ",1]";
            var info = wc.DownloadString(getcounturl);
            var json = JObject.Parse(info);
            var result = json["result"] as JObject;

            saveBlock(result);
        }

        private void saveBlock(JObject jObject) {
            if (Directory.Exists("block") == false)
            {
                Directory.CreateDirectory("block");
            }
            var path = "block" + Path.DirectorySeparatorChar + Helper.blockHeight.ToString("D08") + ".txt";
            File.Delete(path);
            File.WriteAllText(path, jObject.ToString(), Encoding.UTF8);
        }

        private void saveTx(JObject jObject) {
            if(Directory.Exists("transaction") == false)
            {
                Directory.CreateDirectory("transaction");
            }
            var path = "transaction" + Path.DirectorySeparatorChar + Helper.blockHeight.ToString("D08") + ".txt";
            File.Delete(path);
            File.WriteAllText(path, jObject.ToString(), Encoding.UTF8);
        }

        private void ping()
        {
            LogHelper.ping(batchInterval, name());
        }
    }
}
