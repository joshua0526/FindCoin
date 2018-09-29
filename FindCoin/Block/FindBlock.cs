﻿using FindCoin.core;
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

        private int batchInterval = 50;
        private void initConfig() {

        }

        private void run() {
            if (Directory.Exists("block") == false)
            {
                Directory.CreateDirectory("block");
            }
            if (Directory.Exists("transaction") == false)
            {
                Directory.CreateDirectory("transaction");
            }
            if (Directory.Exists("utxo") == false)
            {
                Directory.CreateDirectory("utxo");
            }
            while (Helper.blockHeight < 500) {
                if (Helper.blockHeight > getBlockHeightFromRpc()) {
                    continue;
                }

                getBlockFromRpc();

                ping();

                Helper.blockHeight++;
            }
        }

        static WebClient wc = new WebClient();

        private int getBlockHeightFromRpc() {
            var getcounturl = getUrl() + "?jsonrpc=2.0&id=1&method=getblockcount&params=[]";
            var info = wc.DownloadString(getcounturl);
            var json = JObject.Parse(info);
            var result = json["result"];

            return int.Parse(result[0]["blockcount"].ToString());
        }

        private void getBlockFromRpc() {
            var getcounturl = getUrl() + "?jsonrpc=2.0&id=1&method=getblock&params=[" + Helper.blockHeight + ",1]";
            var info = wc.DownloadString(getcounturl);
            var json = JObject.Parse(info);
            var result = json["result"];

            foreach (var r in result) {
                var path = "block" + Path.DirectorySeparatorChar + Helper.blockHeight.ToString("D08") + "_" + r.Path + ".txt";
                SaveBlock.getInstance().Save(r as JObject, path);
            }           
        } 

        private void ping()
        {
            LogHelper.ping(batchInterval, name());
        }
    }
}
