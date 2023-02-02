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
                string valueBackup = value;

                foreach (var descAction in attrDescSchema.TextDesc)
                {
                    value = ActionHandler.Do(value, descAction);
                }

                attrAndValueModel.ValueDesc = Translation.Translate.GetValueOrDefault(value, null) ?? Translation.TranslateLower.GetValueOrDefault(value.ToLowerInvariant(), string.Empty);

                if (!string.IsNullOrEmpty(attrAndValueModel.ValueDesc))
                {
                    return attrAndValueModel;
                }


                //else search
                foreach (var descAction in attrDescSchema.TextDesc)
                {
                    valueBackup = ActionHandler.Do(valueBackup, descAction, true);
                }
                attrAndValueModel.ValueDesc = Translation.Translate.GetValueOrDefault(valueBackup, null) ?? Translation.TranslateLower.GetValueOrDefault(valueBackup.ToLowerInvariant(), string.Empty);
            }

            return attrAndValueModel;
        }

        public static string FindTitleAttr(string type)
        {
            return FileSchemeDescs.GetValueOrDefault(type, null)?.TitleAttr;
        }

        public static List<string> FindDescAttr(string type)
        {
            return FileSchemeDescs.GetValueOrDefault(type, null)?.DescAttr;
        }
    }
}
