using System.Collections.Generic;
using System.Linq;
using Constructor.Database;
using Starcounter.Palindrom;
using Starcounter.Palindrom.Transient;

namespace Constructor.ViewModels
{
    public class ProductModel : TransientViewModel
    {
        public string Name
        {
            get => Product.Name;
            set
            {
                Product.Name = value;
                this.MemberChanged(p => p.Name);
            }
        }

        public string ImageUrl
        {
            get => Product.ImageUrl;
            set
            {
                Product.ImageUrl = value;
                this.MemberChanged(p => p.ImageUrl);
            }
        }

        public string Description
        {
            get => Product.Description;
            set
            {
                Product.Description = value;
                this.MemberChanged(p => p.Description);
            }
        }

        public List<ModuleModel> Modules { get; }

        internal void ReloadModules()
        {
            var moduleModels = Product.Modules
                .AsEnumerable()
                .Select(m => new ModuleModel(m, this, Context));
            Modules.Clear();
            Modules.AddRange(moduleModels);
            this.MemberChanged(p => p.Modules);
        }

        internal void AddModule(Module module)
        {
            Modules.Add(new ModuleModel(module, this, Context));
            this.AddedToCollection(p => p.Modules);
        }

        public long TotalAmount => Modules.Sum(x => x.TotalAmount);

        internal Product Product { get; }

        public ProductModel(Product product, IPalindromContext context) : base(context)
        {
            Product = product;
            Modules = new List<ModuleModel>();
            ReloadModules();
        }
    }
}