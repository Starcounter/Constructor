using System.Linq;
using Constructor.Database;
using Starcounter.Palindrom;
using Starcounter.Palindrom.Transient;

namespace Constructor.ViewModels
{
    public class ModuleModel : TransientViewModel
    {
        public string Name
        {
            get => Module.Name;
            set
            {
                Module.Name = value;
                this.MemberChanged(p => p.Name);
            }
        }

        public string Description
        {
            get => Module.Description;
            set
            {
                Module.Description = value;
                this.MemberChanged(p => p.Description);
            }
        }

        public int Price
        {
            get => Module.Price;
            set
            {
                Module.Price = value;
                this.MemberChanged(p => p.Price);
            }
        }

        public int Quantity
        {
            get => Module.Quantity;
            set
            {
                Module.Quantity = value;
                this.MemberChanged(p => p.Quantity);
            }
        }

        public string ImageUrl
        {
            get => Module.ImageUrl;
            set
            {
                Module.ImageUrl = value;
                this.MemberChanged(p => p.ImageUrl);
            }
        }

        public long TotalAmount => Module.TotalAmount;

        public bool IsModified
        {
            get
            {
                if (Module == null) return false;
                var changes = Module.Repository.CurrentCommit.Properties.ToList();
                return changes.Any(x => x.Item.Equals(Module));
            }
        }

        internal bool IsDeleted => Module?.IsDeleted != false;

        private ProductModel ProductModel { get; }
        private Module Module { get; }

        public ModuleModel(Module module, ProductModel parent, IPalindromContext context) : base(context)
        {
            Module = module;
            ProductModel = parent;
        }

        public void Delete()
        {
            var index = ProductModel.Modules.IndexOf(this);
            ProductModel.Modules.RemoveAt(index);
            ProductModel.RemovedFromCollection(p => p.Modules, index);
            Module.IsDeleted = true;
        }
    }
}