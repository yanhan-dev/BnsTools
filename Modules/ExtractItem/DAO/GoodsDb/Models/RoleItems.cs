using System;
using System.Linq;
using System.Text;

namespace ExtractItem.DAO.GoodsDb.Models
{
    ///<summary>
    ///
    ///</summary>
    public partial class RoleItems
    {
           public RoleItems(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int ItemId {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int ItemAppGroupId {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public short RoleId {get;set;}

    }
}
