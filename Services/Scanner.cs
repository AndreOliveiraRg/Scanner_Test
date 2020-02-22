using Services.Configs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Services
{
    public class Scanner
    {
        public static void Init(Config cfg)
        {
            var watcher = new FileSystemWatcher(cfg.PathFileIn, cfg.Filters);
            watcher.Created += (s, e) => OnChanged(s, e, cfg);

            watcher.EnableRaisingEvents = true;
        }

        private static void OnChanged(object source, FileSystemEventArgs e, Config cfg)
        {
            ProcessFile(e.FullPath, cfg);
        }

        private static void ProcessFile(string path, Config cfg)
        {
            string text = File.ReadAllText(path);

            if (!string.IsNullOrEmpty(text) && text.Contains("\n") && text.Contains("ç"))
            {
                var list = text.Split("\r\n");

                if (list != null && list.Count() > 0)
                {
                    var sales = list.Where(x => x.Contains("003ç")).ToList();

                    var items = sales.SelectMany(x => x.Split("ç")
                                      .Where(y => y.Contains("["))).ToList();

                    var itemsValue = new List<SaleItem>();

                    foreach (var i in items)
                    {
                        var v = i.Split(",");

                        if (v != null && v.Count() == 3)
                        {
                            foreach (var v2 in v)
                            {
                                foreach (var v3 in v2.Replace("[", "").Replace("]", "").Split(","))
                                {
                                    var v4 = v3.Split("-");

                                    if (v4 != null && v4.Count() == 3)
                                    {
                                        itemsValue.Add(new SaleItem()
                                        {
                                            ItemID = Convert.ToInt32(v4[0]),
                                            ItemQuantity = Convert.ToInt32(v4[1]),
                                            ItemPrice = Convert.ToDecimal(v4[2].Replace(".", ","))
                                        });
                                    }
                                }
                            }
                        }
                    }

                    // • Quantidade de vendedores no arquivo de entrada
                    var totalSalesMan = list.Where(x => x.Contains("001ç")).Count();

                    // • Quantidade de clientes no arquivo de entrada
                    var totalCustumer = list.Where(x => x.Contains("002ç")).Count();

                    // • ID da venda mais cara
                    var item = itemsValue.OrderByDescending(x => x.ItemPrice).FirstOrDefault();
                    var idSale = sales.FirstOrDefault(x => x.Replace(".", ",").Contains($"{item.ItemID}-{item.ItemQuantity}-{item.ItemPrice}")).Split("ç")?[1];

                    // • O pior vendedor
                    var item2 = itemsValue.OrderBy(x => x.FullPrice).FirstOrDefault();
                    var saleMan = sales.FirstOrDefault(x => x.Replace(".", ",").Contains($"{item2.ItemID}-{item2.ItemQuantity}-{item2.ItemPrice}")).Split("ç")?.LastOrDefault();

                    File.Delete($"{cfg.PathFileOut}/result.txt");

                    using (StreamWriter file = new StreamWriter($"{cfg.PathFileOut}/result.txt", true))
                    {
                        file.WriteLine($"Quantidade de vendedores no arquivo de entrada: {totalSalesMan}");
                        file.WriteLine($"Quantidade de clientes no arquivo de entrada:  {totalCustumer}");
                        file.WriteLine($"ID da venda mais cara:  {idSale} do ID de item: {item2.ItemID}");
                        file.WriteLine($"O pior vendedor: {saleMan}");
                    }
                }
            }
        }

    }
    public class SaleItem
    {
        public int ItemID { get; set; }
        public int ItemQuantity { get; set; }
        public decimal ItemPrice { get; set; }
        public decimal FullPrice { get { return ItemPrice * ItemQuantity; } }
    }
}




