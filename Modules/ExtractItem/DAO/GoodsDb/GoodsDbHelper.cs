
using Common;

using ExtractItem.DAO.GoodsDb.Models;
using ExtractItem.POJO.VO;

using SqlSugar;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExtractItem.DAO.GoodsDb
{
    public class GoodsDbHelper
    {

        private readonly SqlSugarScope db;

        public GoodsDbHelper(string connString)
        {

            db = new SqlSugarScope(new ConnectionConfig()
            {
                ConnectionString = connString,//连接符字串
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true //不设成true要手动close
            });
        }

        public async Task ImportItems(List<ItemVO> itemVOList)
        {
            List<Items> itemsList = itemVOList.Select(itemVO => new Items
            {
                ItemId = itemVO.Id,
                ItemName = itemVO.Name,
                ItemDescription = itemVO.Alias,
                ItemAppGroupCode = "bns",
                ItemType = 3,
                IsConsumable = false,
                BasicPrice = 0,
                BasicCurrencyGroupId = 21,
                Changed = DateTime.Now,
                ChangerAdminAccount = "AutoBuildAdmin",
            }).ToList();


            List<ItemDisplay> itemDisplayList = itemVOList.Select(itemVO => new ItemDisplay
            {
                ItemId = itemVO.Id,
                LanguageCode = 1,
                ItemDisplayName = itemVO.Name,
                ItemDisplayDescription = null,
            }).ToList();


            List<GameItems> gameItemsList = itemVOList.Select(itemVO => new GameItems
            {
                ItemId = itemVO.Id,
                GameItemKey = GameItemKeyHelper.Get(itemVO.Id),
                GameItemData = "AAAAAAAAAAA=",

            }).ToList();


            try
            {
                db.BeginTran();

                await db.Utilities.PageEachAsync(itemsList, 1000, async pagelist =>
                {
                    var x = db.Storageable(pagelist)
                    .SplitUpdate(it => it.Any())
                    .SplitInsert(it => true)
                    .ToStorage();

                    await x.AsInsertable.ExecuteCommandAsync();
                    await x.AsUpdateable.ExecuteCommandAsync();
                });


                await db.Utilities.PageEachAsync(itemDisplayList, 1000, async pagelist =>
                {
                    var x = db.Storageable(pagelist)
                    .SplitUpdate(it => it.Any())
                    .SplitInsert(it => true)
                    .ToStorage();

                    await x.AsInsertable.ExecuteCommandAsync();
                    await x.AsUpdateable.ExecuteCommandAsync();
                });

                await db.Utilities.PageEachAsync(gameItemsList, 1000, async pagelist =>
                {
                    var x = db.Storageable(pagelist)
                    .SplitUpdate(it => it.Any())
                    .SplitInsert(it => true)
                    .ToStorage();

                    await x.AsInsertable.ExecuteCommandAsync();
                    await x.AsUpdateable.ExecuteCommandAsync();
                });


                db.CommitTran();
            }
            catch (Exception)
            {
                db.RollbackTran();
                throw;
            }
        }
    }
}
