using System;
using System.Linq;
using System.Text;

namespace ExtractItem.DAO.GoodsDb.Models
{
    ///<summary>
    ///
    ///</summary>
    public partial class Categories
    {
           public Categories(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public short CategoryId {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string CategoryName {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string AppGroupCode {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public bool IsDisplayable {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int DisplayOrder {get;set;}

           /// <summary>
           /// Desc:
           /// Default:sysdatetimeoffset()
           /// Nullable:False
           /// </summary>           
           public DateTimeOffset Changed {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string ChangerAdminAccount {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public short? CurrencyGroupId {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public short? ParentCategoryId {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string CategoryData {get;set;}

    }
}
