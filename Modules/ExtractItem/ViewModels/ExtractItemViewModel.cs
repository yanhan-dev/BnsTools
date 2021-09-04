using Prism.Commands;
using Prism.Mvvm;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractItem.ViewModels
{
    public class ExtractItemViewModel : BindableBase
    {
        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public ExtractItemViewModel()
        {
            Message = "View A from your Prism Module";
        }

        /**
         * 1.加载翻译
         * 2.加载服务端所有Item
         * 3.提取所有ItemId和Alias
         * 4.根据Alias找汉化
         * 5.更新数据库
         * 
         */
    }
}
