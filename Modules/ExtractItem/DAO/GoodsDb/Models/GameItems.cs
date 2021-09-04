using SqlSugar;

using System;
using System.Linq;
using System.Text;

namespace ExtractItem.DAO.GoodsDb.Models
{
    ///<summary>
    ///
    ///</summary>
    public partial class GameItems
    {
        public GameItems()
        {


        }
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        [SugarColumn(IsPrimaryKey = true)]
        public int ItemId { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string GameItemKey { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string GameItemData { get; set; }

    }
}
