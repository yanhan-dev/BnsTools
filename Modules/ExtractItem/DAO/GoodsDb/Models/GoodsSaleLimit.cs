using System;
using System.Linq;
using System.Text;

namespace ExtractItem.DAO.GoodsDb.Models
{
    ///<summary>
    ///
    ///</summary>
    public partial class GoodsSaleLimit
    {
           public GoodsSaleLimit(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int GoodsId {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public short SaleLimitType {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int SaleLimitValue {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string SaleLimitExternalKey {get;set;}

    }
}
