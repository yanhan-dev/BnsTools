using Common.Action;
using Common.Model;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Desc
    {
        public static Dictionary<string, string> FileNamesDesc { get; set; }

        private static Dictionary<string, FileSchemeDescModel> _FileSchemeDescs;

        public static Dictionary<string, FileSchemeDescModel> FileSchemeDescs => _FileSchemeDescs ??= LoadSchemeDesc(Config.DescPath);

        private static Dictionary<string, FileSchemeDescModel> LoadSchemeDesc(string path)
        {
            var dict = new Dictionary<string, FileSchemeDescModel>();
            var files = Directory.EnumerateFiles(path, "*.json", SearchOption.TopDirectoryOnly);

            foreach (var file in files)
            {
                string text = File.ReadAllText(file, Encoding.UTF8);
                var obj = JsonConvert.DeserializeObject<FileSchemeDescModel>(text);
                dict[obj.Type] = obj;
            }

            return dict;
        }

        public static AttrAndValueModel FindAttrAndValueDesc(string attr, string value, string type)
        {
            AttrAndValueModel attrAndValueModel = new() { Attr = attr, Value = value };

            var schema = FileSchemeDescs.GetValueOrDefault(type, null);
            if (schema == null)
            {
                return attrAndValueModel;
            }
            string desc = string.Empty;
            var attrDescSchema = schema.AttrDesc.FirstOrDefault(ss => ss.Attrs.TryGetValue(attr, out desc));
            if (attrDescSchema == null)
            {
                return attrAndValueModel;
            }

            attrAndValueModel.AttrDesc = desc;


            if (attrDescSchema.LocalDesc != null && attrDescSchema.LocalDesc.Count > 0)
            {
                attrAndValueModel.ValueDesc = attrDescSchema.LocalDesc.GetValueOrDefault(value, string.Empty);

                return attrAndValueModel;
            }

            if (attrDescSchema.TextDesc?.Count > 0)
            {
                foreach (var descAction in attrDescSchema.TextDesc)
                {
                    switch (descAction.Action)
                    {
                        case AddAction.ACTION:
                            value = AddAction.Do(value, descAction.Params.GetValueOrDefault(AddAction.START), descAction.Params.GetValueOrDefault(AddAction.END));
                            break;
                        case DeleteAction.ACTION:
                            value = DeleteAction.Do(value, descAction.Params.GetValueOrDefault(DeleteAction.START), descAction.Params.GetValueOrDefault(AddAction.END));
                            break;
                        case SplitAction.ACTION:
                            value = SplitAction.Do(value, descAction.Params.GetValueOrDefault(SplitAction.CHAR), int.Parse(descAction.Params.GetValueOrDefault(AddAction.START)), int.Parse(descAction.Params.GetValueOrDefault(AddAction.END)));
                            break;
                        default:
                            throw new InvalidOperationException("肯定不会走这里");
                    }
                }

                attrAndValueModel.ValueDesc = Translation.Translate.GetValueOrDefault(value, string.Empty);

                return attrAndValueModel;
            }

            return attrAndValueModel;
        }

        public static string FindTitleAttr(string type)
        {
            return FileSchemeDescs[type]?.TitleAttr;
        }

    }
}
