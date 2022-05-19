using Prism.Mvvm;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RandomStore.POJO.DO
{
    public class RandomStoreItem : BindableBase
    {
        public event EventHandler<RandomStoreItem> ItemChanged;

        private int _ParentId;
        public int ParentId
        {
            get { return _ParentId; }
            set { SetProperty(ref _ParentId, value); }
        }

        private int _Id;
        public int Id
        {
            get { return _Id; }
            set { SetProperty(ref _Id, value); }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { SetProperty(ref _Name, value); }
        }

        private string _Alias;
        public string Alias
        {
            get { return _Alias; }
            set { SetProperty(ref _Alias, value); ItemChanged?.Invoke(null, this); }
        }

        private int _Count;
        public int Count
        {
            get { return _Count; }
            set { SetProperty(ref _Count, value); ItemChanged?.Invoke(null, this); }
        }

        private int _Money;
        public int Money
        {
            get { return _Money; }
            set { SetProperty(ref _Money, value); ItemChanged?.Invoke(null, this); }
        }
    }
}
