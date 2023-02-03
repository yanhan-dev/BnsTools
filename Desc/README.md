``` JSON
{
  "Type": "ItemTransformRecipe", //xml文件的type
  "TitleAttr": "alias", //列表的标题取自哪个字段
  "DescAttr": ["title-item", "rare-item-1", "random-item-1"] //列表的解释取自哪个字段, 按照顺序匹配查找
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
  "DescAttr": ["alias"],
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

```JSON
{
  "Type": "item",
  "TitleAttr": "alias",
  "DescAttr": ["alias"],
  "AttrDesc": [
    {
      "Attrs": {
        "set-item": "套装"
      },
      "LocalDesc": null,
      "TextDesc": [
        {
          "Action": "Direct", //Direct 不进行任何处理直接去翻译查找
        }
      ]
    },
        {
      "Attrs": {
        "exceptional-usable-attraction": "Dungeon:Dungeon_LightningStorm",
        "valid-attraction-1": "Field-Zone:YeolSaJeeDea",
        "valid-attraction-3": "Classic-Field-Zone:PoHwaRan_Classic",
        "valid-attraction-4": "Faction-battle-field-zone:BuYuDo",
        "valid-attraction-test1": "Guild-battle-field-zone:HellScroll",
        "valid-attraction-test2": "Jackpot-Boss-Zone:BloodWindplain",
        "valid-attraction-2": "Attraction-group:Suwal_Classic"
      },
      "LocalDesc": null,
      "TextDesc": [
        {
          "Action": "Replace", //Replace 用Map的Value替换掉Key
          "Params": {
            "Maps": {
              "Dungeon:": "Dungeon.Name2.",
              "Field-Zone:": "FieldZone.Name2."
            }
          }
        }
      ]
    }
  ]
}
```

```JSON
{
  "Type": "ItemTransformRecipe",
  "TitleAttr": "alias",
  "DescAttr": [
    "title-item",
    "rare-item-1",
    "random-item-1",
    "normal-item-1"
  ],
  "AttrDesc": [
    {
      "Attrs": {
        "main-ingredient": "主物品",
        "sub-ingredient-1": "子物品-1"
      },
      "LocalDesc": null,
      "TextDesc": [
        {
          "Action": "Replace",
          "Params": {
            "Maps": {
              "item:": "Item.Name2.",
              "item-brand:": "IBN."
            }
          }
        },
        {
          "Action": "SelectAdd", //匹配添加
          "Params": {
            "Selector": [
              {
                "Start": "",
                "End": "_Weapon",
                "AndContains": [ //如果值中存在ItemBrand且存在Weapon, 在开始和结束分别添加字符
                  "ItemBrand",
                  "Weapon"
                ]
              }
            ]
          }
        }
      ]
    }
  ]
}

```