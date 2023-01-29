``` JSON
{
  "Type": "ItemTransformRecipe", //xml文件的type
  "TitleAttr": "alias", //列表的标题取自哪个字段
  "DescAttr": "title-item", //列表的解释取自哪个字段
  "AttrDesc": [ //字段解析
    {
      "Attrs": { //字段解释, 同类型字段放一起
        "main-ingredient": "主物品", 
        "sub-ingredient-1": "子物品-1"
      },
      "TextDesc": [ //TextDesc是指的去翻译文件里查找, 先按照Action的顺序对字段进行处理, 之后去翻译搜索
        {
          "Action": "Delete", //Delete 在字段开头和结束删除某些字段
          "Params": {
            "Start": [
              "item:"
            ],
            "End": []
          }
        },
        {
          "Action": "Add", //Add 在字段的开头和结束添加某些字段
          "Params": {
            "Start": "Item.Name2.", //先添加Start和End, 如果查找不到尝试添加ElseStart ElseEnd
            "End": "",
            "ElseStart": "Npc.Name2.", // Else 跟Start和End不叠加
            "ELseEnd": ""
          }
        }
      ]
    },
    {
      "Attrs": {
        "consume-fixed-ingredient": "失败时消耗-固定道具"
      },
      "LocalDesc": { //枚举解释
        "y": "是",
        "n": "否"
      }
    }
  ]
}
```

```JSON
{
  "Type": "QuestReward",
  "TitleAttr": "alias",
  "DescAttr": "alias",
  "AttrDesc": [
    {
      "Attrs": {
        "alias": "别名"
      },
      "LocalDesc": null,
      "TextDesc": [
        {
          "Action": "Split", //Split 用一个字符对字段进行分割, 左闭右开取索引
          "Params": {
            "Char": "_",
            "Start": 0,
            "End": 1
          }
        },
        {
          "Action": "Add",
          "Params": {
            "Start": "Quest.Name2.",
            "End": ""
          }
        }
      ]
    }
  ]
}
```